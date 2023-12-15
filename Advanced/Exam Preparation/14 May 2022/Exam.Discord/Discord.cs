using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.Discord
{
    public class Discord : IDiscord
    {
        private Dictionary<string, Message> messagesById;
        private Dictionary<string, HashSet<Message>> messagesByChanel;

        public Discord()
        {
            this.messagesById = new Dictionary<string, Message>();
            this.messagesByChanel = new Dictionary<string, HashSet<Message>>();
        }

        public void SendMessage(Message message)
        {
            if (this.messagesById.ContainsKey(message.Id))
            {
                throw new ArgumentException();
            }

            this.messagesById.Add(message.Id, message);

            if (!this.messagesByChanel.ContainsKey(message.Channel))
            {
                this.messagesByChanel.Add(message.Channel, new HashSet<Message>());
            }

            this.messagesByChanel[message.Channel].Add(message);
        }

        public bool Contains(Message message)
            => this.messagesById.ContainsKey(message.Id);

        public int Count => this.messagesById.Count;

        public Message GetMessage(string messageId)
        {
            if (!this.messagesById.ContainsKey(messageId))
            {
                throw new ArgumentException();
            }

            return this.messagesById[messageId];
        }

        public void DeleteMessage(string messageId)
        {
            if (!this.messagesById.ContainsKey(messageId))
            {
                throw new ArgumentException();
            }

            var message = this.messagesById[messageId];
            this.messagesById.Remove(messageId);
            this.messagesByChanel[message.Channel].Remove(message);

            if (this.messagesByChanel[message.Channel].Count == 0)
            {
                this.messagesByChanel.Remove(message.Channel);
            }
        }

        public void ReactToMessage(string messageId, string reaction)
        {
            if (!this.messagesById.ContainsKey(messageId))
            {
                throw new ArgumentException();
            }

            this.messagesById[messageId].Reactions.Add(reaction);
        }

        public IEnumerable<Message> GetChannelMessages(string channel)
        {
            if (!this.messagesByChanel.ContainsKey(channel))
            {
                throw new ArgumentException();
            }

            return this.messagesByChanel[channel];
        }

        public IEnumerable<Message> GetMessagesByReactions(List<string> reactions)
            => this.messagesById.Values
                .Where(m => reactions.Except(m.Reactions).Count() == 0)
                .OrderByDescending(m => m.Reactions.Count)
                .ThenBy(m => m.Timestamp);

        public IEnumerable<Message> GetMessageInTimeRange(int lowerBound, int upperBound)
            => this.messagesById.Values
                .Where(m => m.Timestamp >= lowerBound && m.Timestamp <= upperBound)
                .OrderByDescending(m => this.messagesByChanel[m.Channel].Count);

        public IEnumerable<Message> GetTop3MostReactedMessages()
            => this.messagesById.Values
                .OrderByDescending(m => m.Reactions.Count)
                .Take(3);

        public IEnumerable<Message> GetAllMessagesOrderedByCountOfReactionsThenByTimestampThenByLengthOfContent()
            => this.messagesById.Values
                .OrderByDescending(m => m.Reactions.Count)
                .ThenBy(m => m.Timestamp)
                .ThenBy(m => m.Content.Length);
    }
}
