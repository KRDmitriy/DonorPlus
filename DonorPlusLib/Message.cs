using System;

namespace DonorPlusLib
{
    public class Message
    {
        private readonly int id;
        private readonly string text;
        private readonly DateTime time;

        public int Id => id;
        public string Text => text;
        public DateTime Time => time;

        public Message(string text, DateTime time) : this(0, text, time) { }

        public Message(int id, string text, DateTime time)
        {
            this.id = id;
            this.text = text;
            this.time = time;
        }

        public override string ToString()
        {
            return $"ID = {Id}\nText = {Text}\nTime = {Time}";
        }
    }
}
