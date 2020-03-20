using System;

namespace datingApp.API.DTOs
{
    public class MessageForCreationDto
    {
        public int senderId { get; set; }

        public int recipientId { get; set; }

        public DateTime messageSent { get; set; }

        public string content { get; set; }

        public MessageForCreationDto()
        {
            messageSent = DateTime.Now;
        }
    }
}