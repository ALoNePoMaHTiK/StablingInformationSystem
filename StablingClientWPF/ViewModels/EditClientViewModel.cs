using StablingApiClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class EditClientViewModel : BaseViewModel
    {
        private ClientsHttpClient _clientsHttpClient;
        private TrainersHttpClient _trainersHttpClient;
        public EditClientViewModel(ClientsHttpClient clientsHttpClient,
            TrainersHttpClient trainersHttpClient)
        {
            _clientsHttpClient = clientsHttpClient;
            _trainersHttpClient = trainersHttpClient;

            GetTrainers();
        }

        private ObservableCollection<Trainer> _Trainers;
        public ObservableCollection<Trainer> Trainers
        {
            get { return _Trainers; }
            set { _Trainers = value; OnPropertyChanged(); }
        }

        private async Task GetTrainers()
        {
            Trainers = new ObservableCollection<Trainer>(await _trainersHttpClient.GetAllAsync());
        }

        private Client _CurrentClient;
        public Client CurrentClient
        {
            get { return _CurrentClient; }
            set { _CurrentClient = value; OnPropertyChanged(); }
        }

        public DelegateCommand ProcessClientCommand
        {
            get
            {
                return new DelegateCommand(o => {
                    ProcessClientAsync();
                });
            }
        }
        private async Task ProcessClientAsync()
        {
            try
            {
                if (CurrentClient.ClientId != 0)
                    await _clientsHttpClient.UpdateAsync(CurrentClient);
                else
                    await _clientsHttpClient.CreateAsync(CurrentClient);
            }
            catch(ApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
            CloseWindow();
        }
        private void CloseWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
            currentWindow?.Close();
        }
    }
}
