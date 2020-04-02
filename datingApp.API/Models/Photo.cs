using System;

namespace datingApp.API.Models
{
    public class Photo
    {
        public int id { get; set; }
        public string url { get; set; }

        public string description { get; set; }

        public DateTime dateAdded { get; set; }

        public bool isMain { get; set; }

        public string publicId {get; set; }

        public virtual User user { get; set; }

        public int userId { get; set; }
    }
}