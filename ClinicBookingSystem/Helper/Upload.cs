
using Microsoft.AspNetCore.Http;

namespace FinalProject.BLL.Helper
{
    public static class Upload
    {
        public static string UploadFile(string FolderName, IFormFile File)
        {
            if (File == null || File.Length == 0)
                throw new Exception("No file was uploaded.");

            string FolderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                FolderName
            );

            // Create folder if it doesn't exist
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            string FileName = Guid.NewGuid().ToString()
                              + Path.GetExtension(File.FileName);

            string FilePath = Path.Combine(FolderPath, FileName);

            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                File.CopyTo(stream);
            }

            return FileName;
        }

        public static string RemoveFile(string FolderName, string fileName)
        {
            string FolderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                FolderName
            );

            string FilePath = Path.Combine(FolderPath, fileName);

            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
                return "File Deleted Successfully";
            }

            return "File Not Found";
        }
    }
}
