using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thong.Net
{
    public interface IServerHandle
    {
        void ClientDisconnected(Client client);
        void ClientConnected(Client client);
        
    }
}
