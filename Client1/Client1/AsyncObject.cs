using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Client1
{
    // 비동기 작업에서 사용하는 소켓과 해당 작업에 대한 데이터 버퍼를 저장하는 클래스
    public class AsyncObject
    {
        public byte[] _buffer;
        public Socket _workingSocket;
        public readonly int _bufferSize;

        private System.Net.EndPoint _endPoint;

        public AsyncObject(int bufferSize, System.Net.EndPoint endPoint)
        {
            _bufferSize = bufferSize;
            _buffer = new byte[_bufferSize];

            _endPoint = endPoint;
        }

        public void ClearBuffer()
        {
            Array.Clear(_buffer, 0, _bufferSize);
        }
    }
}
