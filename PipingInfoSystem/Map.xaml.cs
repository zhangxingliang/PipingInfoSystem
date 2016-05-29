using BasicService;
using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PipingInfoSystem
{
    /// <summary>
    /// Map.xaml 的交互逻辑
    /// </summary>
    public partial class Map : Window
    {
        private List<PipingInfo> data = new List<PipingInfo>();
        public Map(List<PipingInfo> list)
        {
            InitializeComponent();
            if(list!=null)
            data = list;
        }

        private void InitMarker(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            foreach(var i in data)
            { 
                 this.mapFrame.InvokeScript("searchByStationName", new object[] { i.DetectionAddress });
            }
        }
    }
}
