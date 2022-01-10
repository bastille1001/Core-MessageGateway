using System;

namespace Domain.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Text { get; set; }
        public string Tag { get; set; }
    }
}