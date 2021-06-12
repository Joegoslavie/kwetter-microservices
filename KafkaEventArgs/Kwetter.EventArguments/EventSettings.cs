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
        /// 
        /// </summary>
        public static string NewProfileEventTopic { get; } = "NewProfileEvent";

        /// <summary>
        /// 
        /// </summary>
        public static string TweetProfileEventTopic { get; } = "TweetProfileEvent";

        /// <summary>
        /// 
        /// </summary>
        public static string TweetProfileUpdateEventTopic { get; } = "TweetProfileUpdateEvent";
    }
}
