using System;

namespace ijw.MessageModel {
    public class Message {
        public readonly Guid Id;
        public readonly string Name;
        public readonly string Body;
        public readonly DateTime CreatedTime;

        public Message(string name, string body) {
            this.Id = new Guid();
            this.CreatedTime = DateTime.Now;
            this.Name = name;
            this.Body = body;
        }

        public void Deconstruct(out Guid id, out string name, out string body, out DateTime createdTime) {
            id = this.Id;
            name = this.Name;
            body = this.Body;
            createdTime = this.CreatedTime;
        }
    }
}