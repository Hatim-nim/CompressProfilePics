using System;
using System.IO;
using System.Linq;
using ImageMagick;

namespace CompressProfilePics
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceDirectory = @"C:\Users\Hamad\Saudi Air Navigation Services\NewUAT2023 - Upload files";
            string destinationDirectory = @"C:\Users\Hamad\Desktop\CompressedPics";

            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
            var filesToCompress = Directory
                .EnumerateFiles(sourceDirectory)
                .Where(file => allowedExtensions.Any(ext => file.ToLower().EndsWith(ext)))
                .ToList();

            foreach (string filePath in filesToCompress)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    string outputPath = Path.Combine(destinationDirectory, fileInfo.Name);

                    if (!File.Exists(outputPath))// if it exists, it would result in if (!true) which translates to false, hence, skip being processed.
                    {
                        if (fileInfo.Length > 1500000) // If larger than 1MB
                        {
                            CompressImage(filePath, outputPath);
                            
                            if (fileInfo.Length>1500000) // if still it is more than 1 MB
                            {
                                CompressImage(filePath, outputPath);
                                Console.WriteLine($"Compressed: {fileInfo.Name}");
                            }
                        }
                        else
                        {
                            File.Copy(filePath, outputPath, true);
                            Console.WriteLine($"Copied without compression: {fileInfo.Name}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Skipped (already exists): {fileInfo.Name}");
                    }
                }
                catch (Exception ex) 
                {
                    Console.WriteLine($"Error processing {filePath}: {ex.Message}");
                }
            }
        }

            static void CompressImage(string inputPath, string outputPath)
        {
            using (MagickImage image = new MagickImage(inputPath))
            {
                image.Quality = 10; // Adjust quality to control compression. The lower the number, the lesser the quality
                image.Write(outputPath);
            }

        }
    }
}
