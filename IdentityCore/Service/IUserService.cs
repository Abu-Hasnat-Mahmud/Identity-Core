using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityCore.Service
{
    public interface IUserService
    {
        string GetUserId();
        bool IsAuthenticated();
    }
}
