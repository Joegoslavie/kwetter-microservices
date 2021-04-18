namespace Kwetter.TweetService.Business
{
    using Kwetter.TweetService.Persistence.Context;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TweetManager
    {
        private readonly TweetContext context;
        public TweetManager(TweetContext context)
        {
            this.context = context;
        }
    }
}
