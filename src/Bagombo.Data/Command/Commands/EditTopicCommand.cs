using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Command.Commands
{
  public class EditTopicCommand
  {
    public long Id { get; set; }
    public string NewTitle { get; set; }
    public string NewDescription { get; set; }
    public bool NewShowOnHomePage { get; set; }
  }
}
