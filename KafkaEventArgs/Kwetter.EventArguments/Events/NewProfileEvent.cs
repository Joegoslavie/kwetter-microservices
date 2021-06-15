namespace Kwetter.Messaging.Events
{
    using Confluent.Kafka;
    using Kwetter.Messaging.Arguments;
    using Kwetter.Messaging.Interfaces;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class NewProfileEvent : IProfileEvent, IDisposable
    {
        private readonly IProducer<string, string> producer;
        private readonly List<string> topics;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewProfileEvent"/> class.
        /// </summary>
        /// <param name="producer">Producer.</param>
        /// <param name="topicName">Topic string.</param>
        public NewProfileEvent(IProducer<string, string> producer, List<string> topicNames)
        {
            this.producer = producer;
            this.topics = topicNames;
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
                var message = new Message<string, string> { Value = jsonContents };
                this.topics.ForEach(topic => this.producer.ProduceAsync(topic, message).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {

        }
    }
}
