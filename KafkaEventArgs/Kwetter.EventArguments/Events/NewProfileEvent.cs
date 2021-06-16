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
    public class ProfileEvent : IProfileEvent, IDisposable
    {
        private readonly IProducer<string, string> producer;
        private readonly List<string> topics;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewProfileEvent"/> class.
        /// </summary>
        /// <param name="producer">Producer.</param>
        /// <param name="topicName">Topic string.</param>
        public ProfileEvent(IProducer<string, string> producer, List<string> topicNames)
        {
            this.producer = producer;
            this.topics = topicNames;
        }

        /// <inheritdoc/>
        public void Invoke(int userId, string username, string displayName, string avatarUrl)
        {
            this.Fire(new ProfileEventArgs {
                UserId = userId,
                Username = username,
                DisplayName = displayName,
                AvatarUrl = avatarUrl
            });
        }

        /// <inheritdoc/>
        public void Invoke(ProfileEventArgs args)
        {
            this.Fire(args);
        }

        /// <inheritdoc/>
        private void Fire(ProfileEventArgs args)
        {
            try
            {
                var message = new Message<string, string> 
                { 
                    Value = JsonConvert.SerializeObject(args) 
                };

                this.topics.ForEach(topic => 
                    this.producer.ProduceAsync(topic, message).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {

        }

    }
}
