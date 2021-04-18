using Kwetter.FollowingService.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.FollowingService.Business
{
    public class FollowManager
    {
        private readonly FollowingContext context;
        public FollowManager(FollowingContext context)
        {
            this.context = context;
        }
    }
}
