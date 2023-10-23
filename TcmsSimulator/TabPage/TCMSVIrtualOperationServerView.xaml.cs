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
using TcmsSimulator.TMCSVirtual;

namespace TcmsSimulator.TabPage
{
    /// <summary>
    /// TCMSVIrtualOperationServerView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TCMSVIrtualOperationServerView : UserControl
    {
        public TMCSVirtualOperationServer VirtualOperationServer = new TMCSVirtualOperationServer();
        int SequenceCounter = 1;

        public TCMSVIrtualOperationServerView()
        {
            InitializeComponent();
            init();
            SetLocalIP();
            SetStatusVirtualServerPort();
        }

        private void init()
        {
            TrainQuantityCombo.Items.Add("8칸");
            TrainQuantityCombo.Items.Add("10칸");
            TrainQuantityCombo.SelectedIndex = 0;
        }

        private void OperationTCMSStart_Click(object sender, RoutedEventArgs e)
        {
            int bufferSize = 0;
            int port = 0;

            string IP = IPListCombo.Text;
            int.TryParse(OperationPortBox.Text, out port);
            //int.TryParse(ConfigurationManager.AppSettings["BufferSize"], out bufferSize);
            //Operation Data Server
            VirtualOperationServer.TmcsVirtualClient.SetData(bufferSize, IP, port);
            VirtualOperationServer.TmcsVirtualClient.Create();

            VirtualOperationServer.StartOperation();
        }
        private void SetLocalIP()
        {
            IPListCombo.ItemsSource = TcmsSimulator.Util.Utils.GetLocalIPList();
            if (IPListCombo.Items.Count > 0)
                IPListCombo.SelectedItem = IPListCombo.Items[0];
        }
        private void SetStatusVirtualServerPort()
        {
            OperationPortBox.Text = ConfigurationManager.AppSettings["OperationPort"];
        }

        private void OperationTCMSStop_Click(object sender, RoutedEventArgs e)
        {
            VirtualOperationServer.StopOperation();
        }

        private void OperationDataChange_Click(object sender, RoutedEventArgs e)
        {
            GridUpdate();
        }

        private void OperationDateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? dt = OperationDateCalendar.SelectedDate;
        }

        private void sld_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rate1.Text = e.NewValue.ToString();
        }

        private void sld2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rate2.Text = e.NewValue.ToString();
        }

        private void sld3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rate3.Text = e.NewValue.ToString();
        }

        private void sld4_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rate4.Text = e.NewValue.ToString();
        }

        private void sld5_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rate5.Text = e.NewValue.ToString();
        }

        private void sld6_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rate6.Text = e.NewValue.ToString();
        }

        private void sld7_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rate7.Text = e.NewValue.ToString();
        }

        private void sld0_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rate0.Text = e.NewValue.ToString();
        }

        private void Backward_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BackwardRateBox.Text = e.NewValue.ToString();
        }

        private void StopRate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            StopRateBox.Text = e.NewValue.ToString();
        }

        private void HCR0Box_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            HCR0Box.Text = e.NewValue.ToString();
        }

        private void HCR1Box_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            HCR1Box.Text = e.NewValue.ToString();
        }

        private void GridUpdate()
        {
            string Address;
            int Yy, Mm, Dd, Hh, Minute, Sec;
            int Quantity;

            int Backward, StopRate;
            int TotalKM, Speed, AtcSpeed, TotalStationDistance;
            int Boarding1,  Boarding2,  Boarding3,  Boarding4,  Boarding5, Boarding6,  Boarding7,  Boarding0;
            int Hcr0, Hcr1;

            Address = ConfigurationManager.AppSettings["OperationSenderIP"];

            DateTime dt = OperationDateCalendar.DisplayDate;
            Yy = dt.Year;
            Mm = dt.Month;
            Dd = dt.Day; 
            Hh = dt.Hour;

            Minute = 0; 
            Sec = 0;

            if (TrainQuantityCombo.SelectedIndex == 0)
                Quantity = 0x01;
            else if (TrainQuantityCombo.SelectedIndex == 1)
                Quantity = 0x10;
            else
                Quantity = 0x0;

            int.TryParse(TotalKMBox.Text, out TotalKM);
            int.TryParse(SpeedBox.Text, out Speed);
            int.TryParse(AtcSpeedBox.Text, out AtcSpeed);
            int.TryParse(TotalStationDistanceBox.Text, out TotalStationDistance);

            int.TryParse(BackwardRateBox.Text, out Backward);
            int.TryParse(StopRateBox.Text, out StopRate);

            int.TryParse(rate1.Text, out Boarding1);
            int.TryParse(rate2.Text, out Boarding2);
            int.TryParse(rate3.Text, out Boarding3);
            int.TryParse(rate4.Text, out Boarding4);
            int.TryParse(rate5.Text, out Boarding5);
            int.TryParse(rate6.Text, out Boarding6);
            int.TryParse(rate7.Text, out Boarding7);
            int.TryParse(rate0.Text, out Boarding0);

            int.TryParse(rate0.Text, out Hcr0);
            int.TryParse(rate0.Text, out Hcr1);

            OperationData operationdata = new OperationData();
            operationdata.SetData(Address, SequenceCounter,
                Yy, Mm, Dd, Hh, Minute, Sec,
                Quantity,
                TrainNoBox.Text, TrainOrganBox.Text,
                CurrentStationCodeBox.Text, NextStationCodeBox.Text, FinalStationCodeBox.Text,
                TotalKM, Speed, AtcSpeed, TotalStationDistance,
                Backward, StopRate,
                Boarding1, Boarding2, Boarding3, Boarding4,
                Boarding5, Boarding6, Boarding7, Boarding0,
                Hcr0, Hcr1
                );
            SequenceCounter++;

            VirtualOperationServer.SetOperationData(operationdata);
            byte[] data = new byte[TCMSData.OperationData.OperationDataSize];

            Array.Copy(operationdata.ToByte(), 0, data, 0, TCMSData.OperationData.OperationDataSize);

            OperationGrid.ItemsSource = TcmsSimulator.Util.Utils.ConvertToBitGridData(data);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in data)
            {
                sb.Append("0x" + b.ToString("x2") + " ");
            }

            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                OperationGrid.ItemsSource = TcmsSimulator.Util.Utils.ConvertToBitGridData(data);
                LogBox.Text = "[ Data Set ] " + sb.ToString() + Environment.NewLine;
            });
        }

        private void rate1_TextChanged(object sender, TextChangedEventArgs e)
        {
            double db = 0.0f;
            double.TryParse(rate1.Text, out db);
            sld.Value = db;
        }

        private void rate2_TextChanged(object sender, TextChangedEventArgs e)
        {
            double db = 0.0f;
            double.TryParse(rate2.Text, out db);
            sld2.Value = db;
        }

        private void rate3_TextChanged(object sender, TextChangedEventArgs e)
        {
            double db = 0.0f;
            double.TryParse(rate3.Text, out db);
            sld3.Value = db;
        }

        private void rate4_TextChanged(object sender, TextChangedEventArgs e)
        {
            double db = 0.0f;
            double.TryParse(rate4.Text, out db);
            sld4.Value = db;
        }

        private void rate5_TextChanged(object sender, TextChangedEventArgs e)
        {
            double db = 0.0f;
            double.TryParse(rate5.Text, out db);
            sld5.Value = db;
        }

        private void rate6_TextChanged(object sender, TextChangedEventArgs e)
        {
            double db = 0.0f;
            double.TryParse(rate6.Text, out db);
            sld6.Value = db;
        }

        private void rate7_TextChanged(object sender, TextChangedEventArgs e)
        {
            double db = 0.0f;
            double.TryParse(rate7.Text, out db);
            sld7.Value = db;
        }

        private void rate0_TextChanged(object sender, TextChangedEventArgs e)
        {
            double db = 0.0f;
            double.TryParse(rate0.Text, out db);
            sld0.Value = db;
        }

        private void Backward_TextChanged(object sender, TextChangedEventArgs e)
        {
            double db = 0.0f;
            double.TryParse(BackwardRateBox.Text, out db);
            BackwardSlider.Value = db;
        }

        private void StopRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            double db = 0.0f;
            double.TryParse(StopRateBox.Text, out db);
            StopSlider.Value = db;
        }

        private void HCR0Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            double db = 0.0f;
            double.TryParse(HCR0Box.Text, out db);
            HCR0Slider.Value = db;
        }

        private void HCR1Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            double db = 0.0f;
            double.TryParse(HCR1Box.Text, out db);
            HCR1Slider.Value = db;
        }
    }
}
