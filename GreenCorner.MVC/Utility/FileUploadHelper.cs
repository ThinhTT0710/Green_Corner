namespace GreenCorner.MVC.Utility
{
    public class FileUploadHelper
    {
        public static async Task<(bool IsSuccess, List<string> Paths, string ErrorMessage)> UploadImagesStrictAsync(
    IFormFileCollection files,
    string folderName,
    string filePrefix,
    long maxFileSizeInBytes = 5 * 1024 * 1024,
    bool onlyOneFile = false)
        {
            var resultPaths = new List<string>();

            if (files == null || files.Count == 0)
                return (false, resultPaths, "Không có ảnh nào được chọn.");

            if (onlyOneFile && files.Count > 1)
                return (false, resultPaths, "Chỉ được phép tải lên 1 ảnh.");

            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };

            foreach (var file in files)
            {
                if (file.Length == 0)
                    return (false, resultPaths, $"Ảnh {file.FileName} trống.");
                if (file.Length > maxFileSizeInBytes)
                    return (false, resultPaths, $"Ảnh {file.FileName} vượt quá 5MB.");
                if (!allowedMimeTypes.Contains(file.ContentType.ToLower()))
                    return (false, resultPaths, $"Ảnh {file.FileName} không đúng định dạng.");
            }

            string relativeFolder = $"imgs/{folderName}";
            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativeFolder);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            int index = 1;
            foreach (var file in files)
            {
                string extension = Path.GetExtension(file.FileName).ToLower();
                string fileName = $"{filePrefix}{index}{extension}";
                string filePath = Path.Combine(uploadPath, fileName);

                int suffix = 1;
                while (System.IO.File.Exists(filePath))
                {
                    fileName = $"{filePrefix}{index}_{suffix}{extension}";
                    filePath = Path.Combine(uploadPath, fileName);
                    suffix++;
                }

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                resultPaths.Add($"/{relativeFolder}/{fileName}");
                index++;

                if (onlyOneFile)
                    break;
            }

            return (true, resultPaths, null);
        }
    }
}
