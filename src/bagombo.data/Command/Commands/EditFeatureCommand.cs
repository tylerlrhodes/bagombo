using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Command.Commands
{
  public class EditFeatureCommand
  {
    public long Id { get; set; }
    public string NewTitle { get; set; }
    public string NewDescription { get; set; }
  }
}
