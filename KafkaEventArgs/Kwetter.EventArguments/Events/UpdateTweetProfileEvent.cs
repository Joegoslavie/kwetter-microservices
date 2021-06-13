using Confluent.Kafka;
using Kwetter.Messaging.Arguments;
using Kwetter.Messaging.Interfaces.Tweet;
using Newtonsoft.Json;
using System;

namespace Kwetter.Messaging.Events
{
    public class UpdateTweetProfileEvent : ITweetUpdateEvent, IDisposable
    {
        private readonly IProducer<string, string> producer;
        private readonly string topic;

        /// <summary>
        /// Initializes a new instance of the <see cref="ITweetProfileEvent"/> class.
        /// </summary>
        /// <param name="producer">Producer.</param>
        /// <param name="topicName">Topic string.</param>
        public UpdateTweetProfileEvent(IProducer<string, string> producer, string topicName)
        {
            this.producer = producer;
            this.topic = topicName;
        }

        /// <inheritdoc/>
        public void Invoke(int userId, string username, string displayName)
        {
            try
            {
                var content = new ProfileEventArgs
                {
                    UserId = userId,
                    Username = username,
                    DisplayName = displayName,
                };

                var jsonContents = JsonConvert.SerializeObject(content);
                this.producer.ProduceAsync(this.topic, new Message<string, string> { Value = jsonContents }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {

        }
    }
}
