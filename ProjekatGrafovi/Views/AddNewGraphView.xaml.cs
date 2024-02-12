using ProjekatGrafovi.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjekatGrafovi.Views
{
    /// <summary>
    /// Interaction logic for AddNewGraphView.xaml
    /// </summary>
    public partial class AddNewGraphView : UserControl
    {
        public static AddNewGraphViewModel addNewGraphViewModel = new AddNewGraphViewModel();
        public AddNewGraphView()
        {
            InitializeComponent();

            this.DataContext = addNewGraphViewModel;
        }
    }
}
