namespace Kwetter.Messaging.Interfaces
{
    using Kwetter.Messaging.Arguments;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IProfileEvent
    {
        void Invoke(int userId, string username, string displayName, string avatarUrl);

        void Invoke(ProfileEventArgs args);
    }
}
