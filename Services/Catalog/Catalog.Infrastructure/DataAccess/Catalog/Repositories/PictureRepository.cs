using Catalog.Application.Dto.Pictures;
using Catalog.Application.Interfaces.DataAccess;
using Catalog.Domian.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Catalog.Infrastructure.DataAccess.Catalog.Repositories;

public class PictureRepository : IPictureRepository
{
    public const string PICTURES_FOLDER_NAME = "Pictures";

    private readonly ICatalogDbContext _catalogDbContext;
    private readonly IFileProvider _webRootFileProvider;

    public PictureRepository(ICatalogDbContext catalogDbContext, IWebHostEnvironment env)
    {
        _catalogDbContext = catalogDbContext;
        _webRootFileProvider = env.WebRootFileProvider;
    }

    public async Task<bool> CatalogItemHasPicture(Guid itemId)
    {
        CatalogItem item = await _catalogDbContext.CatalogItems.FindAsync(itemId);

        if (string.IsNullOrEmpty(item?.PictureName)) return false;

        return _webRootFileProvider.GetFileInfo(GetPicturePath(item)).Exists;
    }

    public async Task<PictureDto> GetCatalogItemPicture(Guid itemId)
    {
        CatalogItem item = await _catalogDbContext.CatalogItems.FindAsync(itemId);

        IFileInfo file = _webRootFileProvider.GetFileInfo(GetPicturePath(item));

        return new(item.PictureName, file.CreateReadStream());
    }

    private string GetPicturePath(CatalogItem item)
        => Path.Combine(PICTURES_FOLDER_NAME, item.PictureName);
}
