using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjekatGrafovi.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        AddNewGraphViewModel addNewGraphViewModel = new AddNewGraphViewModel();
        CanvasWindowViewModel canvasWindowViewModel = new CanvasWindowViewModel();

        BindableBase currentViewModel;
        public MyICommand<string> NavCommand { get; set; }
        public BindableBase CurrentViewModel
        {
            get
            {
                return currentViewModel;
            }

            set
            {
                SetProperty<BindableBase>(ref currentViewModel, value);
            }
        }

        public MainWindowViewModel()
        {

            NavCommand = new MyICommand<string>(OnNav);

            addNewGraphViewModel = new AddNewGraphViewModel();
            canvasWindowViewModel = new CanvasWindowViewModel();
            CurrentViewModel = addNewGraphViewModel;
        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "addGraph":
                    CurrentViewModel = addNewGraphViewModel;
                    break;
                case "showMap":
                    CurrentViewModel = canvasWindowViewModel;
                    break;
            }
        }
    }
}
