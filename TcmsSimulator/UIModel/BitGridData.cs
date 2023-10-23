using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace TcmsSimulator.UIModel
{
    public class BitGridData
    {

        public string text { get; set; }
        public string Byte { get; set; }
        public int Bit7 { get; set; }
        public int Bit6 { get; set; }
        public int Bit5 { get; set; }
        public int Bit4 { get; set; }
        public int Bit3 { get; set; }
        public int Bit2 { get; set; }
        public int Bit1 { get; set; }
        public int Bit0 { get; set; }
        public string ETC { get; set; }
    }

    public class StatusGridData
    {
        public ObservableCollection<BitGridData> Statuslist { get; set; }

        public StatusGridData()
        {
            Statuslist = new ObservableCollection<BitGridData>()
            {
            };
        }
    }
}
