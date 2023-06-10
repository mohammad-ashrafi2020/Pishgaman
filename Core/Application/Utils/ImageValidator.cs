using Application.Utils.FileUtil;
using Microsoft.AspNetCore.Http;

namespace Application.Utils;

public static class ImageValidator
{
    public static bool IsImage(this IFormFile? file)
    {
        if (file == null)
            return false;
        return FileValidation.IsValidImageFile(file.FileName);
    }
}