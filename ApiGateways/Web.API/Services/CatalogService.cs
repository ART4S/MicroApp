using System.Text.RegularExpressions;
using Web.API.Models.Catalog.CatalogBrands;
using Web.API.Models.Catalog.CatalogItems;
using Web.API.Models.Catalog.CatalogTypes;
using Web.API.Models.Catalog.Pictures;
using Web.API.Pagination;
using Web.API.Settings;
using Web.API.Utils;

namespace Web.API.Services;

public class CatalogService : ICatalogService
{
    private readonly ILogger _logger;
    private readonly HttpClient _catalogClient;
    private readonly CatalogUrls _urls;

    public CatalogService(
        ILogger<CatalogService> logger,
        HttpClient catalogClient, CatalogUrls urls)
    {
        _logger = logger;
        _catalogClient = catalogClient;
        _urls = urls;
    }

    public async Task<CatalogItemInfoDto> GetItem(Guid id)
    {
        string uri = Regex.Replace(_urls.GetItemUrl, "{id}", id.ToString());

        HttpResponseMessage response = await _catalogClient.GetAsync(uri);

        await HttpUtils.HandleErrorStatusCodes(response);

        return await response.Content.ReadFromJsonAsync<CatalogItemInfoDto>();
    }

    public async Task<PagedResponse<CatalogItemDto>> GetItems(PagedRequest request)
    {
        string uri = _urls.GetItemsUrl + request.ToQueryString();

        HttpResponseMessage response = await _catalogClient.GetAsync(uri);

        await HttpUtils.HandleErrorStatusCodes(response);

        return await response.Content.ReadFromJsonAsync<PagedResponse<CatalogItemDto>>();
    }

    public async Task<ICollection<CatalogBrandDto>> GetBrands()
    {
        HttpResponseMessage response = await _catalogClient.GetAsync(_urls.GetBrandsUrl);

        await HttpUtils.HandleErrorStatusCodes(response);

        return await response.Content.ReadFromJsonAsync<ICollection<CatalogBrandDto>>();
    }

    public async Task<PictureDto> GetPicture(string pictureName)
    {
        string uri = Regex.Replace(_urls.GetPictureUrl, "{pictureName}", pictureName);

        HttpResponseMessage response = await _catalogClient.GetAsync(uri);

        await HttpUtils.HandleErrorStatusCodes(response);

        string? contentType = response.Content.Headers.ContentType?.MediaType;

        if (contentType is null)
        {
            contentType = "image/png";
            _logger.LogInformation(""); // TODO: log
        }

        return new()
        {
            ContentType = contentType,
            Content = await response.Content.ReadAsStreamAsync()
        };
    }

    public async Task<ICollection<CatalogTypeDto>> GetTypes()
    {
        HttpResponseMessage response = await _catalogClient.GetAsync(_urls.GetTypesUrl);

        await HttpUtils.HandleErrorStatusCodes(response);

        return await response.Content.ReadFromJsonAsync<ICollection<CatalogTypeDto>>();
    }

    public async Task<Guid> CreateItem(CatalogItemEditDto item)
    {
        HttpResponseMessage response = await _catalogClient.PostAsJsonAsync(_urls.CreateItemUrl, item);

        await HttpUtils.HandleErrorStatusCodes(response);

        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    public async Task UpdateItem(Guid id, CatalogItemEditDto item)
    {
        string uri = Regex.Replace(_urls.UpdateItemUrl, "{id}", id.ToString());

        HttpResponseMessage response = await _catalogClient.PutAsJsonAsync(uri, item);

        await HttpUtils.HandleErrorStatusCodes(response);
    }

    public async Task DeleteItem(Guid id)
    {
        string uri = Regex.Replace(_urls.DeleteItemUrl, "{id}", id.ToString());

        HttpResponseMessage response = await _catalogClient.DeleteAsync(uri);

        await HttpUtils.HandleErrorStatusCodes(response);
    }
}