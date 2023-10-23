using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcmsSimulator.Socket
{
    class StateObject : BaseStateObject
    {
        public StateObject(int headerBufferSize)
        {
            DataBufferSize = headerBufferSize;
            DataBuffer = new byte[headerBufferSize];
        }
    }
}
