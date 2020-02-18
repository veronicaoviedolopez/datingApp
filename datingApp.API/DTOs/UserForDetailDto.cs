using System;
using System.Collections.Generic;
using datingApp.API.Models;

namespace datingApp.API.DTOs
{
  public class UserForDetailDto
  {

    public int Id { get; set; }
    public string Username { get; set; }

    public string gender { get; set; }

    public int Age { get; set; }

    public string knownAs { get; set; }

    public DateTime created { get; set; }

    public DateTime lastActive { get; set; }

    public string introduction { get; set; }

    public string lookingFor { get; set; }

    public string interests { get; set; }

    public string city { get; set; }

    public string country { get; set; }

    public string photoUrl { get; set; }

    public ICollection<PhotoForDetailDto> photos { get; set; }

  }
}