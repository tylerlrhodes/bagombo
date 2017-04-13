using Bagombo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Command.Commands
{
  public class AddAuthorCommand
  {
    public string ApplicatoinUserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Return Author
    public Author Author { get; set; }
  }
}
