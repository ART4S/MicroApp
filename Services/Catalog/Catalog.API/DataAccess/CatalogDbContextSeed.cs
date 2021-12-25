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

        await _catalogDb.SaveChangesAsync();
    }

    private async Task AddBrands()
    {
        var items = await LoadEntitiesFromXml<CatalogBrand>("CatalogBrands.xml");

        foreach (var item in items)
            await _catalogDb.CatalogBrands.Create(item);
    }

    private async Task AddTypes()
    {
        var items = await LoadEntitiesFromXml<CatalogType>("CatalogTypes.xml");

        foreach (var item in items)
            await _catalogDb.CatalogTypes.Create(item);
    }

    private async Task AddProducts()
    {
        var items = await LoadEntitiesFromXml<CatalogItem>("CatalogItems.xml");

        foreach (var item in items)
            await _catalogDb.Products.Create(item);
    }

    private async Task<List<TEntity>> LoadEntitiesFromXml<TEntity>(string fileName)
        where TEntity : class
    {
        string filePath = Path.Combine(DATA_FOLDER_NAME, fileName);

        if (!File.Exists(filePath))
        {
            // TODO: log
            throw new Exception($"File {filePath} is missing");
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

//private List<object> LoadEntitiesFromXml()
//{
//    List<object> result = new();

//    Dictionary<string, Type> entityTypesByName = GetEntityTypes().ToDictionary(x => x.Name);

//    _logger.LogInformation($"CatalogDbContext entity types:{Environment.NewLine}{string.Join(Environment.NewLine, entityTypesByName.Keys)}");

//    string dataFolderPath = Path.Combine(_contentRootPath, INITIALDATA_FOLDER_NAME);

//    if (!Directory.Exists(dataFolderPath))
//    {
//        _logger.LogError("Folder not found {Folder}", dataFolderPath);

//        throw new Exception("Folder not found");
//    }

//    string[] files = Directory.GetFiles(dataFolderPath, "*.xml", SearchOption.AllDirectories);

//    foreach (string file in files)
//    {
//        if (!entityTypesByName.TryGetValue(Path.GetFileNameWithoutExtension(file), out var entityType))
//        {
//            _logger.LogWarning("Cannot find mathing entity type for file {File}", file);
//            continue;
//        }

//        XmlDocument doc = new();

//        try
//        {
//            _logger.LogInformation("Loading {File}", file);
//            doc.Load(file);
//            _logger.LogInformation("Loaded {File}", file);

//            XmlElement root = doc.DocumentElement;

//            if (root is null)
//            {
//                _logger.LogError("File {File} has missing root element", file);

//                throw new Exception($"File {file} has missing root element");
//            }

//            if (root.ChildNodes.Count == 0)
//            {
//                _logger.LogWarning("File {File} has no elements", file);
//                continue;
//            }

//            XmlSerializer serializer = new(entityType);

//            foreach (XmlNode node in root.ChildNodes)
//            {
//                using StringReader sr = new(node.OuterXml);

//                object? entity = serializer.Deserialize(sr);

//                if (entity is null)
//                {
//                    _logger.LogError("Cannot parse text {Text} from file {File}", node.OuterXml, file);

//                    throw new Exception($"Entity of type {entityType.Name} is null");
//                }

//                result.Add(entity);
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error while processing file {File}", file);

//            throw;
//        }
//    }

//    return result;
//}

//private static IEnumerable<Type> GetEntityTypes() =>
//    typeof(CatalogDbContext).GetProperties()
//        .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
//        .Select(x => x.PropertyType.GetGenericArguments().First());
