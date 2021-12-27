using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;
using Catalog.API.Models;
using Catalog.API.DataAccess.Repositories;

namespace Catalog.API.DataAccess;

public class CatalogDbContextSeed
{
    private const string DATA_FOLDER_NAME = "SeedData";

    private readonly ILogger _logger;
    private readonly ICatalogDbContext _catalogDb;
    private readonly string _contentRootPath;
    private readonly string _webRootPath;

    public CatalogDbContextSeed(
        ILogger<CatalogDbContextSeed> logger,
        IWebHostEnvironment environment,
        ICatalogDbContext catalogDb)
    {
        _logger = logger;
        _catalogDb = catalogDb;
        _contentRootPath = environment.ContentRootPath;
        _webRootPath = environment.WebRootPath;
    }

    public async Task Seed()
    {
        await AddBrands();
        await AddTypes();
        await AddProducts();
        AddPictures();

        await _catalogDb.SaveChanges();
    }

    private async Task AddBrands()
    {
        var items = await LoadEntitiesFromXml<CatalogBrand>("CatalogBrands.xml");

        await _catalogDb.CatalogBrands.CreateMany(items);
    }

    private async Task AddTypes()
    {
        var items = await LoadEntitiesFromXml<CatalogType>("CatalogTypes.xml");

        await _catalogDb.CatalogTypes.CreateMany(items);
    }

    private async Task AddProducts()
    {
        var items = await LoadEntitiesFromXml<CatalogItem>("CatalogItems.xml");

        await _catalogDb.Products.CreateMany(items);
    }

    private async Task<List<TEntity>> LoadEntitiesFromXml<TEntity>(string fileName)
        where TEntity : class
    {
        string filePath = Path.Combine(DATA_FOLDER_NAME, fileName);

        if (!File.Exists(filePath))
        {
            _logger.LogError("File not found {FilePath}", filePath);

            throw new Exception($"File {filePath} not found");
        }

        List<TEntity> entities = new();
        try
        {
            _logger.LogInformation("Loading {File}", filePath);
            XmlDocument doc = new();
            doc.Load(filePath);
            _logger.LogInformation("Loaded {File}", filePath);

            XmlElement root = doc.DocumentElement;

            if (root is null)
            {
                _logger.LogError("File {File} has missing root element", fileName);

                throw new Exception($"File {fileName} has missing root element");
            }

            if (root.ChildNodes.Count == 0)
            {
                _logger.LogWarning("File {File} has no elements", fileName);
                return entities;
            }

            XmlSerializer serializer = new(typeof(TEntity));

            foreach (XmlNode node in root.ChildNodes)
            {
                using StringReader sr = new(node.OuterXml);

                TEntity? entity = serializer.Deserialize(sr) as TEntity;

                if (entity is null)
                {
                    _logger.LogError("Cannot parse text {Text} from file {File}", node.OuterXml, fileName);

                    throw new Exception($"Entity of type {typeof(TEntity).Name} is null");
                }

                entities.Add(entity);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while processing file {File}", fileName);

            throw;
        }

        return entities;
    }

    private void AddPictures()
    {
        string sourcePath = Path.Combine(_contentRootPath, DATA_FOLDER_NAME, "ItemPictures.zip");
        string destinationPath = Path.Combine(_webRootPath, PictureRepository.PICTURES_FOLDER_NAME);

        _logger.LogInformation("Extracting pictures from {SourcePath} to {DestinationPath}", sourcePath, destinationPath);

        if (Directory.Exists(destinationPath))
            Directory.Delete(path: destinationPath, recursive: true);

        ZipFile.ExtractToDirectory(sourcePath, destinationPath);

        _logger.LogInformation("Extracting pictures succeed");
    }
}
