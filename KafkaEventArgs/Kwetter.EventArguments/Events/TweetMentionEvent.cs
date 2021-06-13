using Confluent.Kafka;
using Kwetter.Messaging.Arguments;
using Kwetter.Messaging.Interfaces.Tweet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.Messaging.Events
{
    /// <summary>
    /// s
    /// </summary>
    public class TweetMentionEvent : ITweetMentionEvent, IDisposable
    {
        private readonly IProducer<string, string> producer;
        private readonly string topic;

        /// <summary>
        /// Initializes a new instance of the <see cref="ITweetProfileEvent"/> class.
        /// </summary>
        /// <param name="producer">Producer.</param>
        /// <param name="topicName">Topic string.</param>
        public TweetMentionEvent(IProducer<string, string> producer, string topicName)
        {
            this.producer = producer;
            this.topic = topicName;
        }

        /// <inheritdoc/>
        public void Dispose()
        {

        }

        public void Invoke(int tweetId, int authorId, int mentionUserId)
        {
            try
            {
                var content = new MentionEventArgs
                {
                    TweetId = tweetId,
                    AuthorId = authorId,
                    MentionUserId = mentionUserId,
                };

                var jsonContents = JsonConvert.SerializeObject(content);
                this.producer.ProduceAsync(this.topic, new Message<string, string> { Value = jsonContents }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
