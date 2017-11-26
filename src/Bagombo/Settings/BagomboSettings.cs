using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bagombo
{
  public class BagomboSettings
  {
    public string Brand { get; set; } = "Bagombo Demo";
    public string HomePageTitle { get; set; } = "Bagombo Blog";
    public string CopyRight { get; set; } = "Change me";
    public string Description { get; set; } = "A technical blog on various topics";
    public string ContactEmail { get; set; } = "info@bagombo.org";
    public int PostsOnHomePage { get; set; } = 6;
    public int LinesPerPost { get; set; } = 5;
    public string PostImagesRelativePath { get; set; } = "PostImages";
  }
}
