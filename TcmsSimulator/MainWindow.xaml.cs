using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TcmsSimulator.TabPage;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace TcmsSimulator
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitData();
        }

        // <summary>
        // 초기값 설젇
        // </summary>
        private void InitData()
        {
            TabItem item1 = new TabItem();
            item1.Header = TcmsSimulator.Util.Utils.GetResourceString("T00001");//"TCMS Status Send Testor";
            item1.Content = new TCMSTestorView();

            TabItem item2 = new TabItem();
            item2.Header = TcmsSimulator.Util.Utils.GetResourceString("T00002");
            item2.Content = new TCMSVIrtualOperationServerView();

            TabItem item3 = new TabItem();
            item3.Header = TcmsSimulator.Util.Utils.GetResourceString("T00003");
            item3.Content = new TCMSVirtualStatusServerView();

            TCMS_ViewTab.Items.Add(item1);
            TCMS_ViewTab.Items.Add(item2);
            TCMS_ViewTab.Items.Add(item3);
        }
    }
}
