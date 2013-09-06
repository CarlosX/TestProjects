using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using LoginServer.Utily;

namespace LoginServer
{
    public partial class Systems
    {
        public class Decode
        {
            private ushort OPCODE;

            private byte[] BUFFER;
            private Socket socket;
            private object NET;
            private object packet;
            public UInt16 dataSize;
            public byte[] tempbuff;
            public ushort opcode
            {
                get { return OPCODE; }
            }
            public byte[] buffer
            {
                get { return BUFFER; }
            }
            public Socket Client
            {
                get { return socket; }
            }
            public object Networking
            {
                get { return NET; }
            }
            public object Packet
            {
                get { return packet; }
            }
            MemoryStream ms;
            BinaryReader br;

            public Decode(byte[] buffer)
            {
                try
                {
                    byte[] _tempbuff = new byte[buffer.Length];
                    EncDcd endc = new EncDcd();
                    _tempbuff = endc.Crypt(buffer);
                    ms = new MemoryStream(_tempbuff);
                    br = new BinaryReader(ms);
                    dataSize = (ushort)br.ReadInt16();
                    tempbuff = new byte[dataSize];
                    Buffer.BlockCopy(_tempbuff, 0, tempbuff, 0, dataSize);
                    br.Close();
                    ms.Close();
                    br.Dispose();
                    ms.Dispose();
                    _tempbuff = null;
                    endc = null;
                }
                catch (Exception) { }
            }

            public Decode(Socket wSock, byte[] buffer, Client net, object packetf)
            {
                try
                {
                    packet = packetf;

                    ms = new MemoryStream(buffer);
                    br = new BinaryReader(ms);

                    dataSize = (ushort)br.ReadInt16();

                    byte[] b = new byte[dataSize-4];
                    Array.Copy(buffer, 4, b, 0, dataSize-4);

                    BUFFER = b;
                    OPCODE = br.ReadUInt16();

                    socket = wSock;
                    NET = net;
                }
                catch (Exception)
                {
                }
            }
            public Decode(Socket wSock, byte[] buffer, Client net, object packetf, int size)
            {
                try
                {
                    packet = packetf;

                    ms = new MemoryStream(buffer);
                    br = new BinaryReader(ms);

                    byte[] b = new byte[size];
                    Array.Copy(buffer, 0, b, 0, size);

                    BUFFER = b;
                    OPCODE = 0;

                    socket = wSock;
                    NET = net;
                }
                catch (Exception)
                {
                }
            }
            public static string StringToPack(byte[] buff)
            {
                string s = null;
                foreach (byte b in buff) s += b.ToString("X2");
                return s;
            }
        }
    }
}
