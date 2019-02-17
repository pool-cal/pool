using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace 그림판
{
    // 비동기 작업에서 사용하는 소켓과 해당 작업에 대한 데이터 버퍼를 저장하는 클래스
    public class AsyncObject
    {
        public byte[] _buffer;
        public Socket _workingSocket;
        public readonly int _bufferSize;

        public AsyncObject(int bufferSize)
        {
            _bufferSize = bufferSize;
            _buffer = new byte[_bufferSize];
        }

        public void ClearBuffer()
        {
            Array.Clear(_buffer, 0, _bufferSize);
        }
    }
}
