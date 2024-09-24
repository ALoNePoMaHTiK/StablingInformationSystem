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
        ///     Идентификатор для корнеовго слоя модульных окон
        /// </summary>
        public string ROOT_IDENTIFIER { get { return "Root"; } }
        /// <summary>
        ///     Идентификатор для слоя дневных операций для модульных окон
        /// </summary>
        public string DAY_OPERATIONS_IDENTIFIER { get { return "DayOperations"; } }

        /// <summary>
        ///     Идентификатор для слоя абонементов для модульных окон
        /// </summary>
        public string ABONEMENTS_IDENTIFIER { get { return "Abonements"; } }
    }
}