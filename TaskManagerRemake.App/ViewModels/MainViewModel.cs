using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerRemake.WPF.ViewModels;

namespace TaskManagerRemake.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // Main view model to use as central view model for the app
        // manage current view
        // gives bindings to the sidebar/navbar
        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                this.OnPropertyChanged(nameof(CurrentViewModel));
            }
        }
    }
}
