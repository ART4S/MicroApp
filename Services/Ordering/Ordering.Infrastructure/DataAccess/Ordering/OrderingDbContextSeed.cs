using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ordering.Infrastructure.DataAccess.Ordering;

public class OrderingDbContextSeed
{
    private const string INITIALDATA_FOLDER_NAME = "SeedData";

    private readonly ILogger _logger;
    private readonly OrderingDbContext _dbContext;
    private readonly string _contentRootPath;
    private readonly string _webRootPath;

    public OrderingDbContextSeed(IServiceProvider services, OrderingDbContext dbContext)
    {
        _logger = services.GetRequiredService<ILogger<OrderingDbContextSeed>>();
        _dbContext = dbContext;

        var env = services.GetRequiredService<IWebHostEnvironment>();

        _contentRootPath = env.ContentRootPath;
        _webRootPath = env.WebRootPath;
    }

    public void Seed()
    {
        using var transaction = _dbContext.Database.BeginTransaction();

        try
        {
            foreach (var entry in LoadEntitiesFromXml())
            {
                _dbContext.AddRange(entry.Entities);

                if (HasIdInt32(entry.EntityType))
                {
                    _dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [dbo].[{entry.TableName}] ON;");
                    _dbContext.SaveChanges();
                    _dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [dbo].[{entry.TableName}] OFF;");
                }
                else _dbContext.SaveChanges();
            }

            transaction.Commit();
        }
        catch(Exception ex)
        {
            _logger.LogError("", ex); // TODO: log

            transaction.Rollback();

            throw;
        }
    }

    private List<(string TableName, Type EntityType, List<object> Entities)> LoadEntitiesFromXml()
    {
        List<(string TableName, Type EntityType, List<object> Entities)> result = new();

        var entityTypes = GetTables().ToDictionary(x => x.EntityType.Name, x => new { x.EntityType, x.TableName });

        _logger.LogInformation($"OrderingDbContext entity types:{Environment.NewLine}{string.Join(Environment.NewLine, entityTypes.Keys)}");

        string dataFolderPath = Path.Combine(_contentRootPath, INITIALDATA_FOLDER_NAME);

        if (!Directory.Exists(dataFolderPath))
        {
            _logger.LogError("Folder not found {Folder}", dataFolderPath);

            throw new Exception("Folder not found");
        }

        string[] files = Directory.GetFiles(dataFolderPath, "*.xml", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            if (!entityTypes.TryGetValue(Path.GetFileNameWithoutExtension(file), out var entityInfo))
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

                List<object> entities = new();

                XmlSerializer serializer = new(entityInfo.EntityType);

                foreach (XmlNode node in root.ChildNodes)
                {
                    using StringReader sr = new(node.OuterXml);

                    object? entity = serializer.Deserialize(sr);

                    if (entity is null)
                    {
                        _logger.LogError("Cannot parse text {Text} from file {File}", node.OuterXml, file);

                        throw new Exception($"Entity of type {entityInfo.EntityType.Name} is null");
                    }

                    entities.Add(entity);
                }

                result.Add((entityInfo.TableName, entityInfo.EntityType, entities));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing file {File}", file);

                throw;
            }
        }

        return result;
    }

    private static IEnumerable<(string TableName, Type EntityType)> GetTables() =>
        typeof(OrderingDbContext).GetProperties()
            .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
            .Select(x => (x.Name, x.PropertyType.GetGenericArguments().First()));

    private static bool HasIdInt32(Type type) =>
        type.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance) is PropertyInfo prop &&
        prop?.PropertyType == typeof(int);
}
