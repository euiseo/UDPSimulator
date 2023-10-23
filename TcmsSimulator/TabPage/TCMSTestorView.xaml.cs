using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
using TcmsSimulator.TCMSData;

namespace TcmsSimulator.TabPage
{
    /// <summary>
    /// TCMSTestorView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TCMSTestorView : UserControl
    {
        public TCMSInterfaceManager interfaceManager = new TCMSInterfaceManager();
        int SequenceCounter = 0;
        int bufferSize = 0;
        public TCMSTestorView()
        {
            InitializeComponent();
            InitData();
        }

        enum InspectorType
        {
            Inspector = 1 << 7,
            FastnerSleeperModule = 1 << 4,
            HeightModule = 1 << 3,
            PantaModule = 1 << 2,
            RailModule = 1 << 1,
            TunnelWallModule = 1
        }
        /// <summary>
        /// 초기값 설젇
        /// </summary>
        private void InitData()
        {
            OperationIP.Text = ConfigurationManager.AppSettings["OperationIP"];
            OperationPort.Text = ConfigurationManager.AppSettings["OperationPort"];
            StatusIP.Text = ConfigurationManager.AppSettings["StatusIP"];
            StatusPort.Text = ConfigurationManager.AppSettings["StatusPort"];
            SerialNoBox.Text = ConfigurationManager.AppSettings["SericalNo"];
            SWVersionBox.Text = ConfigurationManager.AppSettings["Version"];

            interfaceManager.ReadComplete += DataReceived;

            OperationIPLabel.Content = TcmsSimulator.Util.Utils.GetResourceString("T00101");
            OperationPortLabel.Content = TcmsSimulator.Util.Utils.GetResourceString("T00102");
            TCMSServerLabel.Content = TcmsSimulator.Util.Utils.GetResourceString("T00103");
            StatusPortLabel.Content = TcmsSimulator.Util.Utils.GetResourceString("T00104");
            StatusIPLabel.Content = TcmsSimulator.Util.Utils.GetResourceString("T00105");
        }

        private void OperationIP_TextChanged(object sender, TextChangedEventArgs e)
        {TcmsSimulator.Util.Utils.GetResourceString("T00003");
            //   interfaceManager.TCMSReceiver.IP = ((TextBox)sender).Text;
        }

        private void DataReceived(byte[] receivedData)
        {
            //byte[] data = new byte[TCMSData.StatusData.StatusDataSize];
            //Array.Copy(receivedData, data, data.Length);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in receivedData)
            {
                sb.Append("0x" + b.ToString("x2") + " ");
            }

            DateTime dt = DateTime.Now;
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                ReceiveOperationDataLabel.Text += "["+ dt.ToString() + "]Operation Data Received : " + sb.ToString() + Environment.NewLine;
                StatusDataGrid.ItemsSource = TcmsSimulator.Util.Utils.ConvertToBitGridData(receivedData);
            });
        }

        private void SendStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int Port = 0;
                bufferSize = int.Parse(ConfigurationManager.AppSettings["BufferSize"]);
                Port = int.Parse(StatusPort.Text);
                interfaceManager.StatusSender.SetData(bufferSize, StatusIP.Text, Port);
            }
            catch
            {
                //history.Text += e.Message + Environment.NewLine;
                return;
            }
            interfaceManager.StatusSender.Create();

            StatusData sd = new StatusData();
            sd.SetData(ConfigurationManager.AppSettings["SimulatorIP"],
                SequenceCounter++,
                SWVersionBox.Text,
                0,
                SerialNoBox.Text,
                (bool)RadioInspectorY.IsChecked ? true : false,
                (bool)RadioFastnerSleeperY.IsChecked ? true : false,
                (bool)RadioTunnelWallY.IsChecked ? true : false,
                (bool)RadioRailY.IsChecked ? true : false,
                (bool)RadioPantaY.IsChecked ? true : false,
                (bool)RadioHeightY.IsChecked ? true : false);

            byte[] sendData = sd.ToByte();

            interfaceManager.StatusSender.Send(sendData);

            DateTime dt = DateTime.Now;
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                StringBuilder sb = new StringBuilder();
                foreach (byte b in sendData)
                {
                    sb.Append("0x" + b.ToString("x2") + " ");
                }
                ReceiveOperationDataLabel.Text += "[" + dt.ToString() + "]Status Data Sent : " + sb.ToString() + Environment.NewLine;
                StatusDataGrid.ItemsSource = TcmsSimulator.Util.Utils.ConvertToBitGridData(sendData);
            });
        }
        private void OperationReceiveButton_Click(object sender, RoutedEventArgs e)
        {
            bufferSize = int.Parse(ConfigurationManager.AppSettings["BufferSize"]);
            interfaceManager.TCMSReceiver.SetData(bufferSize, OperationIP.Text, int.Parse(OperationPort.Text));
            interfaceManager.TCMSReceiver.Start();
        }
        private void OperationStopButton_Click(object sender, RoutedEventArgs e)
        {
            interfaceManager.TCMSReceiver.Stop();
        }

        public void WriteMessage(string msg)
        {
            ReceiveOperationDataLabel.Text += msg;
        }

        private void StatusIP_TextChanged(object sender, TextChangedEventArgs e)
        {
            //    interfaceManager.StatusSender.IP = ((TextBox)sender).Text;
        }
    }
}
