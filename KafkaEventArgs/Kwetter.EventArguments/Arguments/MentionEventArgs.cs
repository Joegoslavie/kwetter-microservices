using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.Messaging.Arguments
{
    public class MentionEventArgs
    {
        public int TweetId { get; set; }
        public int MentionUserId { get; set; }
        public int AuthorId { get; set; }
    }
}
