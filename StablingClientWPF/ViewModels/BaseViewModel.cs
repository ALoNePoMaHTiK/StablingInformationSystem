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

        /// <summary>
        ///     Идентификатор для корневого слоя модульных окон
        /// </summary>
        public string RootLayerIdentifier { get { return "RootLayer"; } }
        /// <summary>
        ///     Идентификатор для слоя дневных операций для модульных окон
        /// </summary>
        public string SecondLayerIdentifier { get { return "SecondLayer"; } }
    }
}