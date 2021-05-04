using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.AuthenticationService.Events
{
    interface ISetupProfileEvent
    {
        void Invoke(int userId, string username);
    }
}
