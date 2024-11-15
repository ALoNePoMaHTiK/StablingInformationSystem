using StablingApiClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StablingClientWPF.ViewModels.Dialogs
{
    public class ClientDialogViewModel : BaseViewModel
    {

        public ClientDialogViewModel(IEnumerable<Trainer> trainers)
        {
            Trainers = new ObservableCollection<Trainer>(trainers);
        }

        public ClientDialogViewModel(IEnumerable<Trainer> trainers,Client client) : this(trainers)
        {
            Client = client;
        }

        public ClientDialogViewModel(IEnumerable<Trainer> trainers, Client client, bool isEditMode)
            : this(trainers,client)
        {
            IsEditMode = isEditMode;
        }

        private bool _IsEditMode = true;
        public bool IsEditMode
        {
            get { return _IsEditMode; }
            set { _IsEditMode = value; OnPropertyChanged(); }
        }

        private Client _Client;
        public Client Client
        {
            get { return _Client; }
            set { _Client = value; OnPropertyChanged(); }
        }

        #region Справочники
        private ObservableCollection<Trainer> _Trainers;
        public ObservableCollection<Trainer> Trainers
        {
            get { return _Trainers; }
            set { _Trainers = value; OnPropertyChanged(); }
        }
        #endregion
    }
}
