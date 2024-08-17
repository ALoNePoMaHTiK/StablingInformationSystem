using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StablingClientWPF.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string ROOT_IDENTIFIER { get { return "Root"; } }
        public string DAY_OPERATIONS_IDENTIFIER { get { return "DayOperations"; } }
    }
}