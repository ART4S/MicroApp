using Catalog.Application.Dto.Pictures;
using Catalog.Application.Services.DataAccess;
using Catalog.Domian.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Catalog.Infrastructure.DataAccess.Repositories;

public class ItemPictureRepository : IItemPictureRepository
{
    public const string PICTURES_FOLDER_NAME = "Pictures";

    private readonly ICatalogDbContext _catalogDbContext;
    private readonly IFileProvider _webRootFileProvider;

    public ItemPictureRepository(ICatalogDbContext catalogDbContext, IWebHostEnvironment env)
    {
        _catalogDbContext = catalogDbContext;
        _webRootFileProvider = env.WebRootFileProvider;
    }

    public async Task<PictureDto?> GetPicture(Guid itemId)
    {
        CatalogItem? item = await _catalogDbContext.CatalogItems.FindAsync(itemId);

        if (string.IsNullOrEmpty(item?.PictureName)) return null;

        IFileInfo file = _webRootFileProvider.GetFileInfo(GetPathToPicture(item.PictureName));

        return new(item.PictureName, file.CreateReadStream());
    }

    public async Task RemovePictureByName(string pictureName)
    {
        IFileInfo file = _webRootFileProvider.GetFileInfo(GetPathToPicture(pictureName));

        if (file.Exists)
            File.Delete(file.PhysicalPath);
    }

    private string GetPathToPicture(string pictureName)
        => Path.Combine(PICTURES_FOLDER_NAME, pictureName);
}
