using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mime;
using System.Text.RegularExpressions;
using Web.API.Exceptions;
using Web.API.Models.Catalog.CatalogBrand;
using Web.API.Models.Catalog.CatalogItem;
using Web.API.Models.Catalog.CatalogType;
using Web.API.Pagination;
using Web.API.Settings;

namespace Web.API.Services.Catalog;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _httpClient;
    private readonly CatalogUrls _urls;

    public CatalogService(HttpClient httpClient, IOptions<CatalogUrls> urls)
    {
        _httpClient = httpClient;
        _urls = urls.Value;
    }

    public async Task<CatalogItemInfoDto> GetItem(Guid id)
    {
        string uri = Regex.Replace(_urls.GetItemUrl, "{id}", id.ToString());

        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        await HandleErrorStatusCodes(response);

        return await response.Content.ReadFromJsonAsync<CatalogItemInfoDto>();
    }

    public async Task<PaginationResponse<CatalogItemDto>> GetItems(PaginationRequest request)
    {
        string uri = _urls.GetItemsUrl + request.ToQueryString();

        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        await HandleErrorStatusCodes(response);

        return await response.Content.ReadFromJsonAsync<PaginationResponse<CatalogItemDto>>();
    }

    public async Task<ICollection<CatalogBrandDto>> GetBrands()
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_urls.GetBrandsUrl);

        await HandleErrorStatusCodes(response);

        return await response.Content.ReadFromJsonAsync<ICollection<CatalogBrandDto>>();
    }

    public async Task<ICollection<CatalogTypeDto>> GetTypes()
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_urls.GetTypesUrl);

        await HandleErrorStatusCodes(response);

        return await response.Content.ReadFromJsonAsync<ICollection<CatalogTypeDto>>();
    }

    public async Task<Guid> CreateItem(CatalogItemEditDto item)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_urls.CreateItemUrl, item);

        await HandleErrorStatusCodes(response);

        return new Guid(await response.Content.ReadAsStringAsync());
    }

    public async Task UpdateItem(Guid id, CatalogItemEditDto item)
    {
        string uri = Regex.Replace(_urls.UpdateItemUrl, "{id}", id.ToString());

        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, item);

        await HandleErrorStatusCodes(response);
    }

    public async Task DeleteItem(Guid id)
    {
        string uri = Regex.Replace(_urls.DeleteItemUrl, "{id}", id.ToString());

        HttpResponseMessage response = await _httpClient.DeleteAsync(uri);

        await HandleErrorStatusCodes(response);
    }

    private static async Task HandleErrorStatusCodes(HttpResponseMessage response)
    {
        switch (response.StatusCode)
        {
            case HttpStatusCode.NoContent:
            case HttpStatusCode.Created:
            case HttpStatusCode.OK:
                return;

            default:
                throw new InvalidRequestException
                (
                    statusCode: (int)response.StatusCode,
                    content: await response.Content.ReadAsStreamAsync(),
                    contentType: response.Headers.TryGetValues("Content-Type", out var values) && values.Any() 
                        ? values.First() 
                        : MediaTypeNames.Application.Json
                );
        }
    }
}