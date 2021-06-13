namespace Kwetter.TweetService.Business
{
    using Kwetter.TweetService.Persistence.Context;
    using Kwetter.TweetService.Persistence.Entity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TweetManager
    {
        private readonly TweetContext context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public TweetManager(TweetContext context)
        {
            this.context = context;
        }
    }
}
