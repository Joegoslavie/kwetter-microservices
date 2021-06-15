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
        public const string NewProfileEventTopic = "NewProfileEvent";

        /// <summary>
        /// New profile event topic.
        /// </summary>
        public const string NewFollowRefProfileEventTopic = "NewFollowRefProfileEvent";

        /// <summary>
        /// New profile event topic.
        /// </summary>
        public const string ProfileEventTopic = "ProfileEvent";

        /// <summary>
        /// New tweet profile event topic.
        /// </summary>
        public const string NewTweetProfileEventTopic = "TweetProfileEvent";

        /// <summary>
        /// New mention in tweet event topic.
        /// </summary>
        public const string NewTweetMentionEventTopic = "NewTweetMentionEvent";

        /// <summary>
        /// Hashtag in tweet event topic.
        /// </summary>
        public const string TweetContainsHashtagEventTopic = "TweetContainsHashtagEvent";

        /// <summary>
        /// Update tweet reference profile event topic.
        /// </summary>
        public const string TweetProfileUpdateEventTopic = "TweetProfileUpdateEvent";
    }
}
