using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TcmsSimulator.UIModel;

namespace TcmsSimulator.Util
{

	/// <summary>
	/// string으로 enum값 찾기
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public static class EnumUtil<T>
	{
		public static T Parse(string s)
		{
			return (T)Enum.Parse(typeof(T), s);
		}
	}

	public static class Utils
    {
        private static ILog log = LogManager.GetLogger("Program");
        public static void WriteError(string str, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0)
        {
            str = string.Format("[{0}][{1}][line:{2}][{3}]", Path.GetFileName(file), method, lineNumber, str);
            TcmsSimulator.Util.Utils.log.Error(str);
        }
        public static void WriteInfo(string str, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0)
        {
            str = string.Format("[{0}][{1}][line:{2}][{3}]", Path.GetFileName(file), method, lineNumber, str);
            TcmsSimulator.Util.Utils.log.Info(str);
        }
        public static void WriteDebug(string str, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0)
        {
            str = string.Format("[{0}][{1}][line:{2}][{3}]", Path.GetFileName(file), method, lineNumber, str);
            TcmsSimulator.Util.Utils.log.Debug(str);
        }

        public static bool IsPingTest(string ip)
        {
            try
            {
                Ping ping = new Ping();

                PingReply result = ping.Send(ip);

                if (result.Status == IPStatus.Success)
                    return true;
                return false;
            }
            catch(Exception e)
            {
                TcmsSimulator.Util.Utils.WriteError(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Resources파일에서 문자열읽기
        /// </summary>
        /// <param name="key"></param>
        /// <returns>문자열</returns>
        public static string GetResourceString(string key)
        {
            ResourceManager rm = new ResourceManager(typeof(TcmsSimulator.Properties.Resources));
            String str = "";
            try
            {
                str = rm.GetString(key);
            }
            catch(Exception e)
            {
                TcmsSimulator.Util.Utils.WriteError(e.Message);
                str = "error";
            }

            return str;
        }

        public static ObservableCollection <BitGridData> ConvertToBitGridData(byte[] data)
        {
            ObservableCollection<BitGridData> bitGridData = new ObservableCollection<BitGridData>();
            int index = 0;

            foreach(byte b in data)
            {
                BitGridData gridData = new BitGridData();
                gridData.text = index.ToString();
                gridData.Byte = "0x" + b.ToString("x2");
                gridData.Bit0 = (b & (0x1     )) > 0 ? 1: 0;
                gridData.Bit1 = (b & (0x1 << 1)) > 0 ? 1 : 0;            
                gridData.Bit2 = (b & (0x1 << 2)) > 0 ? 1 : 0;
                gridData.Bit3 = (b & (0x1 << 3)) > 0 ? 1 : 0;
                gridData.Bit4 = (b & (0x1 << 4)) > 0 ? 1 : 0;
                gridData.Bit5 = (b & (0x1 << 5)) > 0 ? 1 : 0;
                gridData.Bit6 = (b & (0x1 << 6)) > 0 ? 1 : 0;
                gridData.Bit7 = (b & (0x1 << 7)) > 0 ? 1 : 0;
                gridData.ETC = b.ToString();
                bitGridData.Add(gridData);
                index++;
            }

            return bitGridData;
        }

        public static IPAddress[] GetLocalIPList()
        {
            IPAddress[] addr;
            String strHostName = string.Empty;
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());

            addr = ipEntry.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToArray();

            return addr;
        }
    }
}
