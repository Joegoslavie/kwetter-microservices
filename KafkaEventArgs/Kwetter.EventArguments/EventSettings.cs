using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.Messaging
{
    public static class EventSettings
    {
        /// <summary>
        /// New profile event topic.
        /// </summary>
        public static string NewProfileEventTopic { get; } = "NewProfileEvent";

        /// <summary>
        /// New tweet profile event topic.
        /// </summary>
        public static string NewTweetProfileEventTopic { get; } = "TweetProfileEvent";

        /// <summary>
        /// New mention in tweet event topic.
        /// </summary>
        public static string NewTweetMentionEventTopic { get; } = "NewTweetMentionEvent";

        /// <summary>
        /// Hashtag in tweet event topic.
        /// </summary>
        public static string TweetContainsHashtagEventTopic { get; } = "TweetContainsHashtagEvent";

        /// <summary>
        /// Update tweet reference profile event topic.
        /// </summary>
        public static string TweetProfileUpdateEventTopic { get; } = "TweetProfileUpdateEvent";
    }
}
