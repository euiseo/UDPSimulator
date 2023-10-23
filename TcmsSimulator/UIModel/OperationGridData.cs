using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcmsSimulator.UIModel
{
    public class OperationGridData
    {
        public ObservableCollection<BitGridData> Operationlist { get; set; }
        public OperationGridData()
        {
            Operationlist = new ObservableCollection<BitGridData>()
            {
            };
        }
    }
}
