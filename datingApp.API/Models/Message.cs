using System;

namespace datingApp.API.Models
{
    public class Message
    {
        public int id { get; set; }

        public int senderId { get; set; }

        public virtual User sender { get; set; }

        public int recipientId { get; set; }

        public virtual User recipient { get; set; }

        public string content { get; set; }

        public bool isRead { get; set; }

        public DateTime? dateRead { get; set; }

        public DateTime messageSent { get; set; }

        public bool senderDeleted { get; set; }

        public bool recipientDeleted { get; set; }

    }
}