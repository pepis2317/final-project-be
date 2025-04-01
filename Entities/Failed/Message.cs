//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Entities
//{
//    public class Message
//    {
//        public Guid Id { get; set; } = Guid.NewGuid();

//        public Guid ChatId { get; set; }
//        public virtual Chat Chat { get; set; } = null!;

//        public Guid SenderId { get; set; }
//        public virtual ChatUser Sender { get; set; } = null!;

//        public string MessageText { get; set; } = string.Empty;
//        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

//    }
//}
