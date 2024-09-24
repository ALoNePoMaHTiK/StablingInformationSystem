using StablingApiClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StablingClientWPF.ViewModels
{
    public class AbonementCreationFormViewModel : BaseViewModel
    {
        public AbonementCreationFormViewModel(Abonement abonement,
            IEnumerable<Client> clients, IEnumerable<AbonementType> abonementTypes, IEnumerable<Trainer> trainers)
        {
            Abonement = abonement;
            Clients = new ObservableCollection<Client>(clients);
            AbonementTypes = new ObservableCollection<AbonementType>(abonementTypes);
            Trainers = new ObservableCollection<Trainer>(trainers);
        }

        private Abonement _Abonement;
        public Abonement Abonement
        {
            get { return _Abonement; }
            set { _Abonement = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Client> _Clients;
        public ObservableCollection<Client> Clients
        {
            get { return _Clients; }
            set { _Clients = value; OnPropertyChanged(); }
        }

        private ObservableCollection<AbonementType> _AbonementTypes;
        public ObservableCollection<AbonementType> AbonementTypes
        {
            get { return _AbonementTypes; }
            set { _AbonementTypes = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Trainer> _Trainers;
        public ObservableCollection<Trainer> Trainers
        {
            get { return _Trainers; }
            set { _Trainers = value; OnPropertyChanged(); }
        }
    }
}
