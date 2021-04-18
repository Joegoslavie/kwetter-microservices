namespace Kwetter.TweetService.Persistence.Context
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class TweetContext : DbContext
    {
        public TweetContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
