using System;
using System.Collections.Generic;

namespace datingApp.API.Models
{
    public class User
    {
         public int Id { get; set; }
         public string Username { get; set; }

          public byte[] PasswordHash { get; set; }

           public byte[] PasswordSalt { get; set; }

           public string gender { get; set; }

           public DateTime dateOfBirth { get; set; }

           public string knownAs { get; set; }

           public DateTime created { get; set; }

           public DateTime lastActive { get; set; }

           public string introduction { get; set; }

           public string lookingFor { get; set; }

           public string interests { get; set; }

           public string city { get; set; }

           public string country { get; set; }

           public ICollection<Photo> photos { get; set; }


    }
}