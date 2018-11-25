using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace LoginServer
{
    public class IPCItem
    {
        public UInt16 resultCode;
        public string banReason;
    }
    class Program
    {
        //public static Network.Servers.IPCServer IPCServer;
        public static Dictionary<UInt16, IPCItem> IPCResultList = new Dictionary<UInt16, IPCItem>();
        public static UInt16 IPCNewId = 0;
        public static int IPCPort = 5999;
        public static bool debug = true;
        #region MYSQL CONFIG
        public static string MYSQL_IP = "127.0.0.1";
        public static string MYSQL_USER = "root";
        public static string MYSQL_PASS = "";
        public static string MYSQL_DATA = "godswar";
        public static int MYSQL_PORT = 3306;
        public static MySqlBase _SQL = new MySqlBase();
        #endregion
        static void Main(string[] args)
        {
            Program pro = new Program();
            //Definitions.Bootlogo._Load();
            Systems.Ini ini = null;
            LogConsole.Init();

            #region Default Settings
            int LSPort = 5999;
            int IPCPort = 5999;
            string LSIP = "127.0.0.1";
            string IPCIP = "127.0.0.1";
            #endregion

            #region Load Settings
            try
            {
                if (File.Exists(Environment.CurrentDirectory + @"\Settings\Settings.ini"))
                {
                    ini = new Systems.Ini(Environment.CurrentDirectory + @"\Settings\Settings.ini");
                    LSPort = Convert.ToInt32(ini.GetValue("SERVER", "port", 5999));
                    LSIP = ini.GetValue("SERVER", "ip", "127.0.0.1").ToString();
                    IPCPort = Convert.ToInt32(ini.GetValue("IPC", "port", 5999));
                    IPCIP = ini.GetValue("IPC", "ip", "127.0.0.1").ToString();
                    debug = ini.GetValue("CONSOLE", "debug", false);
                    MYSQL_USER = ini.GetValue("MYSQL", "user", "root");
                    MYSQL_PASS = ini.GetValue("MYSQL", "pass", "123456");
                    MYSQL_DATA = ini.GetValue("MYSQL", "data", "godswar");
                    MYSQL_IP = ini.GetValue("MYSQL", "ip", "127.0.0.1");
                    MYSQL_PORT = ini.GetValue("MYSQL", "port", 3306);
                    ini = null;
                    LogConsole.Show("Has loaded your ip settings successfully");
                }
                else
                {
                    LogConsole.Show("Settings.ini could not be found, using default setting");
                }
            }
            catch (Exception)
            {
                return;
            }
            #endregion

            _SQL.Init(MYSQL_IP, MYSQL_USER, MYSQL_PASS, MYSQL_DATA, MYSQL_PORT);

            Systems.Server net = new Systems.Server();

            net.OnConnect += new Systems.Server.dConnect(pro._OnClientConnect);
            net.OnError += new Systems.Server.dError(pro._ServerError);

            Systems.Client.OnReceiveData += new Systems.Client.dReceive(pro._OnReceiveData);
            Systems.Client.OnDisconnect += new Systems.Client.dDisconnect(pro._OnClientDisconnect);

            try
            {
                net.Start(LSIP, LSPort);
            }
            catch (Exception ex)
            {
                LogConsole.Show("Starting Server error: {0}", ex);
            }

            #region Load GameServers
            //Systems.LoadServers("GameServers.ini", 6001);
            #endregion

            #region IPC Listener
            /*IPCServer = new Network.Servers.IPCServer();
            IPCServer.OnReceive += new Network.Servers.IPCServer.dOnReceive(pro.OnIPC);
            try
            {
                IPCServer.Start(IPCIP, IPCPort);
                foreach (KeyValuePair<int, Systems.SRX_Serverinfo> GS in Systems.GSList)
                {
                    byte[] rqPacket = IPCServer.PacketRequestServerInfo(IPCPort);
                    Network.Servers.IPCenCode(ref rqPacket, GS.Value.code);
                    IPCServer.Send(GS.Value.ip, GS.Value.ipcport, rqPacket);
                    rqPacket = null;
                }
            }
            catch (Exception ex)
            {
                LogConsole.Show("Error start ICP: {0}", ex);
            }*/
            #endregion

            //LogConsole.Show("Ready for gameserver connection...");
            #region Loop Update GameServers
            while (true)
            {
                Thread.Sleep(100);
                /*foreach (KeyValuePair<int, Systems.SRX_Serverinfo> SSI in Systems.GSList)
                {
                    if (SSI.Value.status != 0 && SSI.Value.lastPing.AddMinutes(5) < DateTime.Now) // server unavailable
                    {
                        SSI.Value.status = 0;
                        LogConsole.Show("Server: {0}:({1}) has timed out, status changed to check", SSI.Value.id, SSI.Value.name);
                    }
                }*/
            }
            #endregion
        }
        #region IPC
        public void OnIPC(System.Net.Sockets.Socket aSocket, System.Net.EndPoint ep, byte[] data)
        {
            try
            {
                if (data.Length >= 6)
                {
                    /*string str;
                    System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                    str = enc.GetString(data);*/

                    string[] ip_s = ep.ToString().Split(':');

                    Systems.SRX_Serverinfo remoteGameServer = Systems.GetServerByEndPoint(ip_s[0], UInt16.Parse(ip_s[1]));
                    if (remoteGameServer != null)
                    {
                        // decode data
                        //Network.Servers.IPCdeCode(ref data, remoteGameServer.code);

                        Systems.PacketReader pack = new Systems.PacketReader(data);
                        short pcmd = pack.Int16();
                        if (data.Length >= 6)
                        {
                            switch (pcmd)
                            {
                                default:
                                    LogDebug.Show("[IPC] unknown command recevied {0:x}", pcmd);
                                    break;

                            }

                        }
                        else
                        {
                            LogDebug.Show("[IPC] data to short");
                        }
                    }
                    else
                    {
                        LogDebug.Show("[IPC] can't find the GameServer {0}:{1}", ((IPEndPoint)ep).Address.ToString(), ip_s[1]);
                    }

                }
                else
                {
                    LogDebug.Show("[IPC] packet to short from {0}", ep.ToString());
                }
            }
            catch (Exception ex)
            {
                LogDebug.Show("[IPC.OnIPC] {0}", ex);
            }
        }
        #endregion
        #region ServerD
        public void _OnReceiveData(Systems.Decode de)
        {
            Systems.oPCode(de);
        }
        public void _OnClientConnect(ref object de, Systems.Client net)
        {
            LogConsole.Show("ClientConnect");
            de = new Systems(net);
        }

        public void _OnClientDisconnect(object o)
        {
            try
            {
                Systems s = (Systems)o;
                s.client.clientSocket.Close();
            }
            catch { }
        }
        private void _ServerError(Exception ex)
        {
            LogDebug.Show(ex.ToString());
        }
        #endregion
    }
}
