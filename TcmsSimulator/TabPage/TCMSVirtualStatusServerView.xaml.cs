using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TcmsSimulator.TMCSVirtual;
using TcmsSimulator.UIModel;

namespace TcmsSimulator.TabPage
{
    /// <summary>
    /// TCMSVirtualStatusServerView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TCMSVirtualStatusServerView : UserControl
    {
        private delegate void SetTextSafeDelegate(string text);
        public TMCSVirtualStatusServer VirtualServer = new TMCSVirtualStatusServer();
        
        public TCMSVirtualStatusServerView()
        {
            InitializeComponent();
            SetLocalIP();
            SetStatusVirtualServerPort();
            InitEvent();
        }

        private void InitEvent()
        {
            VirtualServer.TCMSStatusServer.NetworkReadHandler += ReadCompleteCallback;
        }

        /// <summary>
        /// 가상서버 IP설정
        /// </summary>
        private void SetLocalIP()
        {
            IPListCombo.ItemsSource = TcmsSimulator.Util.Utils.GetLocalIPList();
            if (IPListCombo.Items.Count > 0)
                IPListCombo.SelectedItem = IPListCombo.Items[0]; 
        }

        /// <summary>
        /// 가상서버 포트 Default값 설정
        /// </summary>
        private void SetStatusVirtualServerPort()
        {
            StatusPortBox.Text = ConfigurationManager.AppSettings["StatusPort"];
        }

        private void StatusTCMSStop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StatusTCMSStart_Click(object sender, RoutedEventArgs e)
        {
            if (VirtualServer.TCMSStatusServer.isRunning)
            {
                LogBox.Text += "TCMS virtual Status Server is running" + Environment.NewLine;
                return;
            }

            int bufferSize ;
            int.TryParse(ConfigurationManager.AppSettings["BufferSize"], out bufferSize);
            int port = 0;
            int.TryParse(StatusPortBox.Text, out port);

            ProtocolType protocolType = TcmsSimulator.Util.EnumUtil<ProtocolType>.Parse(ConfigurationManager.AppSettings["ProtocolType"]);
            SocketType socketType = TcmsSimulator.Util.EnumUtil<SocketType>.Parse(ConfigurationManager.AppSettings["SocketType"]);

            VirtualServer.TCMSStatusServer.SetData(bufferSize, IPListCombo.SelectedItem.ToString(), port, protocolType, socketType);
            LogBox.Text += "TCMS virtual Status Server has started" + Environment.NewLine;
            VirtualServer.TCMSStatusServer.Start();
        }

        /// <summary>
        /// 읽기 완료 후 동작
        /// </summary>
        /// <param name="receivedData"></param>
        private void ReadCompleteCallback(byte[] receivedData)
        {
            byte[] data = new byte[TCMSData.StatusData.StatusDataSize];
            Array.Copy(receivedData, data, data.Length);

            StringBuilder sb = new StringBuilder();
            foreach(byte b in data)
            {
                sb.Append("0x" + b.ToString("x2") + " ");
            }
            
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                StatusGrid.ItemsSource = TcmsSimulator.Util.Utils.ConvertToBitGridData(data);
                LogBox.Text = "[ ReadComplete ]" + sb.ToString() + Environment.NewLine;
            });
        }
    }
}
