using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.Collections.Generic;
using LoginServer.Utily;

namespace LoginServer
{
    public partial class Systems
    {        
        public static void oPCode(Decode decode)
        {
            try
            {
                Systems sys = (Systems)decode.Packet;
                sys.PacketInformation = decode;
                PacketReader Reader = new PacketReader(sys.PacketInformation.buffer);

                LogDebug.Show("Opcode: {0}", decode.opcode);


                switch (decode.opcode)
                {
                    case 0x01:
                        {
                            LogDebug.HexDump(sys.PacketInformation.buffer);
                            string username_shift = Reader.String(32);
                            string password_md5 = Reader.String(32);
                            Reader.Skip(4);
                            string client_mac = Reader.String(32);
                            Reader.Skip(32);
                            uint unk3 = Reader.UInt32();
                            StringShift shift = new StringShift();
                            string username = shift.Parser(username_shift);
                            LogDebug.Show("username: {0}", username);
                            LogDebug.Show("password: {0}", password_md5);
                            LogDebug.Show("Mac: {0}", client_mac);
                            LogDebug.Show("unk3: {0}", unk3);
                        }
                        break;
                    default:
                        LogConsole.Show("Default Opcode: {0:X}", decode.opcode);
                        break;
                }
            }
            catch (Exception)
            {
            }
        }        
    }
}

