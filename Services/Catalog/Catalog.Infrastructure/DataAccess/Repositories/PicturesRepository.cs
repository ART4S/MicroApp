using Catalog.Application.Dto.Pictures;
using Catalog.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Catalog.Infrastructure.DataAccess.Repositories;

public class PicturesRepository : IPicturesRepository
{
    public const string PICTURES_FOLDER_NAME = "Pictures";

    private readonly IFileProvider _webRootFileProvider;

    public PicturesRepository(IWebHostEnvironment env)
    {
        _webRootFileProvider = env.WebRootFileProvider;
    }

    public async Task<PictureDto?> GetPicture(string pictureName)
    {
        IFileInfo file = _webRootFileProvider.GetFileInfo(GetPathToPicture(pictureName));

        return new(pictureName, file.CreateReadStream());
    }

    public async Task RemovePicture(string pictureName)
    {
        IFileInfo file = _webRootFileProvider.GetFileInfo(GetPathToPicture(pictureName));

        if (file.Exists)
            File.Delete(file.PhysicalPath);
    }

    private string GetPathToPicture(string pictureName)
        => Path.Combine(PICTURES_FOLDER_NAME, pictureName);
}
