using Identity.API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Xml;
using System.Xml.Serialization;

namespace Identity.API.DataAccess;

class AppDbContextSeed
{
    private const string DATA_FOLDER_NAME = "SeedData";

    private readonly ILogger _logger;
    private readonly string _contentRootPath;
    private readonly UserManager<User> _userManager;

    public AppDbContextSeed(
        ILogger<AppDbContextSeed> logger,
        IWebHostEnvironment environment,
        UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
        _contentRootPath = environment.ContentRootPath;
    }

    public async Task Seed()
    {
        var users = await LoadEntitiesFromXml<UserXml>("Users.xml");

        foreach (var user in users)
            await _userManager.CreateAsync(new()
            {
                Id = user.Id,
                UserName = user.UserName,
            }, user.Password);
    }

    private async Task<List<TEntity>> LoadEntitiesFromXml<TEntity>(string fileName)
        where TEntity : class
    {
        string filePath = Path.Combine(_contentRootPath, DATA_FOLDER_NAME, fileName);

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
}

[XmlRoot("User")]
public class UserXml
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
