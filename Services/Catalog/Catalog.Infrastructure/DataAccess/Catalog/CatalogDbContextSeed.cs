using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.IO.Compression;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using System.Xml.Serialization;
using Catalog.Infrastructure.DataAccess.Catalog.Repositories;

namespace Catalog.Infrastructure.DataAccess.Catalog;

public class CatalogDbContextSeed
{
    private const string INITIALDATA_FOLDER_NAME = "InitialData";

    private readonly ILogger _logger;
    private readonly CatalogDbContext _dbContext;
    private readonly string _contentRootPath;
    private readonly string _webRootPath;

    public CatalogDbContextSeed(IServiceProvider services, CatalogDbContext dbContext)
    {
        _logger = services.GetRequiredService<ILogger<CatalogDbContextSeed>>();
        _dbContext = dbContext;

        var env = services.GetRequiredService<IWebHostEnvironment>();

        _contentRootPath = env.ContentRootPath;
        _webRootPath = env.WebRootPath;

    }

    public void Seed()
    {
        AddEntities();
        AddPictures();
    }

    private void AddEntities()
    {
        _dbContext.AddRange(LoadEntitiesFromXml());
        _dbContext.SaveChanges();
    }

    private List<object> LoadEntitiesFromXml()
    {
        List<object> result = new();

        Dictionary<string, Type> entityTypesByName = GetEntityTypes().ToDictionary(x => x.Name);

        _logger.LogInformation($"CatalogDbContext entity types:{Environment.NewLine}{string.Join(Environment.NewLine, entityTypesByName.Keys)}");

        string dataFolderPath = Path.Combine(_contentRootPath, INITIALDATA_FOLDER_NAME);

        if (!Directory.Exists(dataFolderPath))
        {
            _logger.LogError("Folder not found {Folder}", dataFolderPath);

            throw new Exception("Folder not found");
        }

        string[] files = Directory.GetFiles(dataFolderPath, "*.xml", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            if (!entityTypesByName.TryGetValue(Path.GetFileNameWithoutExtension(file), out var entityType))
            {
                _logger.LogWarning("Cannot find mathing entity type for file {File}", file);
                continue;
            }

            XmlDocument doc = new();

            try
            {
                _logger.LogInformation("Loading {File}", file);
                doc.Load(file);
                _logger.LogInformation("Loaded {File}", file);

                XmlElement root = doc.DocumentElement;

                if (root is null)
                {
                    _logger.LogError("File {File} has missing root element", file);

                    throw new Exception($"File {file} has missing root element");
                }

                if (root.ChildNodes.Count == 0)
                {
                    _logger.LogWarning("File {File} has no elements", file);
                    continue;
                }

                XmlSerializer serializer = new(entityType);

                foreach (XmlNode node in root.ChildNodes)
                {
                    using StringReader sr = new(node.OuterXml);

                    object entity = serializer.Deserialize(sr);

                    if (entity is null)
                    {
                        _logger.LogError("Cannot parse text {Text} from file {File}", node.InnerText, file);

                        throw new Exception($"Entity of type {entityType.Name} is null");
                    }

                    result.Add(entity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing file {File}", file);

                throw;
            }
        }

        return result;
    }

    private IEnumerable<Type> GetEntityTypes()
    {
        return typeof(CatalogDbContext).GetProperties()
            .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
            .Select(x => x.PropertyType.GetGenericArguments().First());
    }

    private void AddPictures()
    {
        string sourcePath = Path.Combine(_contentRootPath, INITIALDATA_FOLDER_NAME, "ItemPictures.zip");
        string destinationPath = Path.Combine(_webRootPath, PictureRepository.PICTURES_FOLDER_NAME);

        _logger.LogInformation("Extracting pictures from {SourcePath} to {DestinationPath}", sourcePath, destinationPath);

        if (Directory.Exists(destinationPath))
            Directory.Delete(path: destinationPath, recursive: true);

        ZipFile.ExtractToDirectory(sourcePath, destinationPath);

        _logger.LogInformation("Extracting pictures succeed");
    }
}
