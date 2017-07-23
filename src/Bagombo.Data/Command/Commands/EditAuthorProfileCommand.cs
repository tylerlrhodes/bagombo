using Bagombo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Command.Commands
{
  public class EditAuthorProfileCommand
  {
    public long Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Blurb { get; set; }

    public string Biography { get; set; }

    public string ImageLink { get; set; }

    public Author Author { get; set; }
  }
}
