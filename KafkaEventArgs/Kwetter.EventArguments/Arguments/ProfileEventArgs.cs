using System;
using System.Collections.Generic;
using System.Text;

namespace Kwetter.Messaging.Arguments
{
    public class ProfileEventArgs
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; }
    }
}
