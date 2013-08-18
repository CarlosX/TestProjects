using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Utily
{
    public class StringShift
    {
        static int[] key_shift = new int[] { 0, 1, -1, -3, 2, 7, 1, 2, -2, 7, 2, 4, 1, 2, 7, 23, 2, 1, 2, 7, 1, 2, 2, 4, 2, 7, 15, -2, 2, 1, 11 };
        static char[] key_f = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        public string Parser(string dt)
        {
            try
            {
                string tmpdata = dt.Replace("\u0000", System.String.Empty);
                string fulldata = "";

                int lng = tmpdata.Length;
                foreach (char dd in tmpdata)
                {
                    int pos = 0;
                    for (int x = 0; x < key_f.Length; x++)
                    {
                        if (dd == key_f[x])
                        {
                            pos = x;
                            break;
                        }
                    }
                    int key = key_shift[lng];
                    int bf = key_f.Length - pos;
                    char nwt = ' ';
                    if ((bf - key) <= 0)
                    {
                        int find = bf - key;
                        if (find < 0)
                        {
                            int nff = Math.Abs(find);
                            nwt = key_f[nff];
                        }
                        else
                        {
                            nwt = key_f[find];
                        }
                    }
                    else
                    {
                        nwt = key_f[pos + key];
                    }
                    fulldata += nwt;
                }

                return fulldata;
            }
            catch (Exception ep)
            {
                LogDebug.Show(ep.ToString());
            }
            return dt;
        }
    }
}
