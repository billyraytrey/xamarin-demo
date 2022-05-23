using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XamarinAccountSetupDemoViewModel
{
    public abstract class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected void OnPropertyChangedAuto([CallerMemberName] string propertyName = "")
        {

        }
    }
}
