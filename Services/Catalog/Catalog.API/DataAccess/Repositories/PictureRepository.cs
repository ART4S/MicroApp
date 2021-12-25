using Catalog.API.Application.Models.Pictures;
using Microsoft.Extensions.FileProviders;

namespace Catalog.API.DataAccess.Repositories;

public class PictureRepository : IPictureRepository
{
    public const string PICTURES_FOLDER_NAME = "Pictures";

    private readonly IFileProvider _webRootFileProvider;

    public PictureRepository(IWebHostEnvironment env)
    {
        _webRootFileProvider = env.WebRootFileProvider;
    }

    public async Task<PictureDto?> GetPicture(string pictureName)
    {
        IFileInfo file = _webRootFileProvider.GetFileInfo(GetPathToPicture(pictureName));

        return file.Exists ? new(pictureName, file.CreateReadStream()) : null;
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
