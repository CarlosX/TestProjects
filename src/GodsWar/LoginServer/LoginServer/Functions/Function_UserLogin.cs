using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer
{
    public partial class Systems
    {
        public static int UserLogin(string _username, string _password, string _mac="")
        {
            int resp = 1;
            SQLResult result = Program._SQL.Select("SELECT * FROM users WHERE username=? AND password=?", _username, _password);
            if (result.Count != 0)
            {
                //
            }
            else
            {
                //error
                resp = 2;
            }
            return resp;
        }
        public static byte[] UserFail(byte _code)
        {
            PacketWriter pack = new PacketWriter();
            pack.Create(0x03);
            pack.Byte(0x00);
            pack.Byte(_code);
            return pack.GetBytes();
        }
    }
}
