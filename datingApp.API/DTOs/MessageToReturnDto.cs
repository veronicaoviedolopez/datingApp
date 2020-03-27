using System;

namespace datingApp.API.DTOs
{
    public class MessageToReturnDto
    {
        public int id { get; set; }

        public int senderId { get; set; }

        public string senderKnownAs { get; set; }

        public string senderPhotoUrl { get; set; }

        public int recipientId { get; set; }

        public string recipientKnownAs { get; set; }

        public string recipientPhotoUrl { get; set; }

        public string content { get; set; }

        public bool isRead { get; set; }

        public DateTime? dateRead { get; set; }

        public DateTime messageSent { get; set; }

    }
}