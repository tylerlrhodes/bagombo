using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bagombo.Services
{
  public class FileSystemImageService : IImageService
  {
    private readonly BagomboSettings _settings;

    public FileSystemImageService(IOptions<BagomboSettings> settings)
    {
      _settings = settings.Value;
    }

    public async Task<string> SaveImage(byte[] bytes, string filename)
    {
      var ext = Path.GetExtension(filename);
      var name = Path.GetFileNameWithoutExtension(filename);

      var absolute = Path.Combine(Directory.GetCurrentDirectory(), _settings.PostImagesRelativePath, $"{name}{ext}");

      var dir = Path.GetDirectoryName(absolute);

      Directory.CreateDirectory(dir);

      if (File.Exists(absolute))
      {
        File.Delete(absolute);
      }

      using (var writer = new FileStream(absolute, FileMode.CreateNew))
      {
        await writer.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
      }

      return $"/{_settings.PostImagesRelativePath}/{name}{ext}";
    }
  }
}
