using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bagombo.Services
{
  public interface IImageService
  {
    Task<string> SaveImage(byte[] bytes, string filename);
  }
}
