using StablingApiClient;
using System.Collections.ObjectModel;

namespace StablingClientWPF.ViewModels.Dialogs
{
    public class TrainingDialogViewModel : BaseViewModel
    {
        public TrainingDialogViewModel(IEnumerable<Trainer> trainers,
            IEnumerable<Client> clients,
            IEnumerable<TrainingType> trainingTypes,
            IEnumerable<Horse> horses,DateTime dateTime)
        {
            Trainers = new ObservableCollection<Trainer>(trainers);
            Clients = new ObservableCollection<Client>(clients);
            TrainingTypes = new ObservableCollection<TrainingType>(trainingTypes);
            Horses = new ObservableCollection<Horse>(horses);
            Training = new Training() { TrainingDateTime = dateTime };
        }

        public TrainingDialogViewModel(IEnumerable<Trainer> trainers,
            IEnumerable<Client> clients,
            IEnumerable<TrainingType> trainingTypes,
            IEnumerable<Horse> horses, Training training)
        {
            Trainers = new ObservableCollection<Trainer>(trainers);
            Clients = new ObservableCollection<Client>(clients);
            TrainingTypes = new ObservableCollection<TrainingType>(trainingTypes);
            Horses = new ObservableCollection<Horse>(horses);
            Training = training;
        }

        private Training _Training;
        public Training Training
        {
            get { return _Training; }
            set { _Training = value; OnPropertyChanged(); }
        }

        #region Коллекции

        private ObservableCollection<Trainer> _Trainers;
        public ObservableCollection<Trainer> Trainers
        {
            get { return _Trainers; }
            set { _Trainers = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Client> _Clients;
        public ObservableCollection<Client> Clients
        {
            get { return _Clients; }
            set { _Clients = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TrainingType> _TrainingTypes;
        public ObservableCollection<TrainingType> TrainingTypes
        {
            get { return _TrainingTypes; }
            set { _TrainingTypes = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Horse> _Horses;
        public ObservableCollection<Horse> Horses
        {
            get { return _Horses; }
            set { _Horses = value; OnPropertyChanged(); }
        }
        #endregion
    }
}
