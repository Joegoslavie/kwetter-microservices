namespace Kwetter.ProfileService.Business
{
    using Kwetter.ProfileService.Persistence.Context;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProfileManager
    {
        private readonly ProfileContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileManager"/> class.
        /// </summary>
        /// <param name="context">Profile context class.</param>
        public ProfileManager(ProfileContext context)
        {
            this.context = context;
        }
    }
}
