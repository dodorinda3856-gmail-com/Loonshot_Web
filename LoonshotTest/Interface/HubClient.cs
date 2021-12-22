using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoonshotTest.Interface
{
    public interface IHubClient {
        Task BroadcastMessage( int number);     
    }
   
}
