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
        public static string NewProfileEventTopic { get; } = "Kwetter.NewProfileEvent";

        /// <summary>
        /// 
        /// </summary>
        public static string UpdateProfileEventTopic { get; } = "Kwetter.UpdateProfileEvent";
    }
}
