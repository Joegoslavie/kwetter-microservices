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
        public const string ProfileEventTopic = "ProfileEvent";

        /// <summary>
        /// New profile event topic.
        /// </summary>
        public const string TweetProfileRefEventTopic = "TweetProfileRefEvent";

        /// <summary>
        /// New profile event topic.
        /// </summary>
        public const string FollowRefProfileEventTopic = "FollowRefProfileEvent";

        /// <summary>
        /// New mention in tweet event topic.
        /// </summary>
        public const string NewTweetMentionEventTopic = "NewTweetMentionEvent";

        /// <summary>
        /// Hashtag in tweet event topic.
        /// </summary>
        public const string TweetContainsHashtagEventTopic = "TweetContainsHashtagEvent";
    }
}
