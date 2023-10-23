using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcmsSimulator.Socket
{
    public class BaseStateObject
    {
        public long ParentID = 0;
        // Client  socket.  
        public System.Net.Sockets.Socket Listener = null;
        public System.Net.Sockets.Socket WorkSocket = null;
        // Size of receive buffer.  
        public int DataBufferSize = 0;
        public int RemainDataBufferSize = 0;

        public int ReadDataOffset = 0;
        public int ReadBufferSize = 0;
        public bool IsHeaderOk = false;

        // Receive buffer.  
        public byte[] DataBuffer;
    }
}
