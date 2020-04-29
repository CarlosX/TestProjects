using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.Collections.Generic;
using LoginServer.Utility;
using LoginServer.Definitions;

namespace LoginServer
{
    public partial class Systems
    {        
        public static void OPCode(Decode decode)
        {
            try
            {
                Systems sys = (Systems)decode.Packet;
                sys.PacketInformation = decode;
                PacketReader Reader = new PacketReader(sys.PacketInformation.buffer);
                LogDebug.Show("Opcode: {0}", decode.opcode);
                Opcode opc = (Opcode)decode.opcode;
                switch (opc)
                {
                    case Opcode._MSG_LOGIN:
                        {
                            string username_shift = Reader.String(32);
                            string password_md5 = Reader.String(32);
                            Reader.Skip(4);
                            string client_mac = Reader.String(32);
                            Reader.Skip(32);
                            uint unk3 = Reader.UInt32();
                            StringShift shift = new StringShift();
                            string username = shift.Parser(username_shift);
                            LogDebug.Show("username: {0}", username);
                            LogDebug.Show("password_md5: {0}", password_md5);
                            LogDebug.Show("MAC: {0}", client_mac);
                            LogDebug.Show("unk3: {0}", unk3);
                            int res = UserLogin(username, password_md5, client_mac);
                            switch (res)
                            {
                                case (int)AuthenticationStatus.OK:
                                    sys.client.SendC(ServerList());
                                    break;
                                case (int)AuthenticationStatus.BANNED:
                                    sys.client.SendC(UserFail(0xF0, Reason.BANNED));
                                    break;
                                default:
                                    sys.client.SendC(UserFail(0xF0, Reason.AUTH_FAILED));
                                    break;
                            }
                        }
                        break;
                    default:
                        LogConsole.Show("Default Opcode: {0:X} - {1}", decode.opcode, opc);
                        LogDebug.HexDump(sys.PacketInformation.buffer, 16, true, false);
                        break;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}

