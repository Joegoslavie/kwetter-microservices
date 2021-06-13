﻿using Kwetter.Messaging.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.Messaging.Interfaces.Tweet
{
    public interface ITweetEvent
    {
        void Invoke(int tweetId, int authorId, int mentionUserId);
    }
}
