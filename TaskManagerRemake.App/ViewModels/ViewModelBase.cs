using System.ComponentModel;

// https://stackoverflow.com/questions/15936428/binding-contentcontrol-content-for-dynamic-content#15960843
// https://www.youtube.com/watch?v=N09UM-RPGGM&list=PLA8ZIAm2I03jSfo18F7Y65XusYzDusYu5&index=3

namespace TaskManagerRemake.WPF.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public virtual void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
