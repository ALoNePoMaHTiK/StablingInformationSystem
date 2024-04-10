using StablingApiClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StablingClientMAUI.ViewModels
{
    public class TrainingTypesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private TrainingTypesHttpClient _client;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<TrainingType> _TrainingTypes;
        public ObservableCollection<TrainingType> TrainingTypes
        {
            get { return _TrainingTypes; }
            set
            {
                _TrainingTypes = value; OnPropertyChanged();
            }
        }

        public TrainingTypesViewModel(TrainingTypesHttpClient client)
        {
            _client = client;
            GetTrainingTypesAsync();
        }

        private async Task GetTrainingTypesAsync()
        {
            TrainingTypes = new ObservableCollection<TrainingType>(await _client.GetAllAsync());
        }
    }
}
