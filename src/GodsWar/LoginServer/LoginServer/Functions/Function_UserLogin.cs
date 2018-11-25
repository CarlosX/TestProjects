using LoginServer.Definitions;
using LoginServer.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer
{
    public partial class Systems
    {
        static int MAX_ACCOUNT_SIZE = 16;
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

        public static bool JudgeValidStr(string name, string password)
        {
            //Construir mensaje de retorno de inicio de sesión
            MSG_LOGIN_RETURN_INFO Login_info = new MSG_LOGIN_RETURN_INFO();
            Login_info.Head.usSize = 0;
            Login_info.Head.usType = (ushort)Opcode._MSG_LOGIN_RETURN_INFO;

            //Límite de longitud
            if (name.Length > MAX_ACCOUNT_SIZE || password.Length > MAX_ACCOUNT_SIZE)
            {
                Login_info.ucInfo = 0;
                //SEND
                return false;
            }

            //Limite de cuenta
            /*
             * FALTA VALIDAR CHAR TEXTO
             * SEND
             * */
            return true;
        }

        public static void CompleteLogin(Client client)
        {
            MSG_LOGIN_RETURN_INFO Login_info = new MSG_LOGIN_RETURN_INFO();
            Login_info.Head.usSize = 0;
            Login_info.Head.usType = (ushort)Opcode._MSG_LOGIN_RETURN_INFO;
            Login_info.ucInfo = 0x01;
            client.SendC(Login_info.end());
        }

        public static byte[] UserFail(byte _code)
        {
            MSG_LOGIN_RETURN_INFO Login_info = new MSG_LOGIN_RETURN_INFO();

            Login_info.Head.usSize = 0;
            Login_info.Head.usType = (ushort)Opcode._MSG_LOGIN_RETURN_INFO;

            PacketWriter pack = new PacketWriter();
            pack.Create((ushort)Opcode._MSG_LOGIN_RETURN_INFO);
            pack.Byte(0x00);
            /*
             * 0: el ID no está registrado; 
             * 1: el inicio de sesión es exitoso; 
             * 2: inicio de sesión repetido; 
             * 3: error de contraseña; 
             * 4: error de versión 
             */
            pack.Byte(_code);
            return pack.GetBytes();
        }
    }
}
