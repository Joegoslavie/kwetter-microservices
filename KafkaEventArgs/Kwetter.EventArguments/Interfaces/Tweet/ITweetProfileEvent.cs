using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.Messaging.Interfaces.Tweet
{
    public interface ITweetProfileEvent
    {
        void Invoke(int userId, string username, string displayName);
    }
}
