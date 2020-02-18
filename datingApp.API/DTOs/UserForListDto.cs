using System;

namespace datingApp.API.DTOs
{
  public class UserForListDto
  {
    public int Id { get; set; }
    public string Username { get; set; }

    public string gender { get; set; }

    public string knowAs { get; set; }

    public DateTime created { get; set; }

    public DateTime lastActive { get; set; }

    public string city { get; set; }

    public string country { get; set; }
    public string photoUrl { get; set; }

  }
}