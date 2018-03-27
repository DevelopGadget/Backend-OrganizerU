using Microsoft.Extensions.Configuration;

namespace OrganizerU.Models
{
  public class Settings
  {
    public string ConectionString { get; set; }
    public string Database { get; set; }
    public IConfigurationRoot configuration { get; set; }
  }

}