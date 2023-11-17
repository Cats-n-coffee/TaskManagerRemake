using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerRemake.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        // Main view model to use as central view model for the app
        // manage current view
        // gives bindings to the sidebar/navbar
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
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
