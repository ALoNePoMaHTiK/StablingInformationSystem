using StablingApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class EditClientViewModel : BaseViewModel
    {
        private ClientsHttpClient _httpClient;
        public EditClientViewModel(ClientsHttpClient httpClient)
        {
            _httpClient = httpClient;
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
                    ProcessClient();
                });
            }
        }
        private void ProcessClient()
        {
            try
            {
                if (CurrentClient.ClientId != 0)
                    _httpClient.UpdateClientAsync(CurrentClient);
                else
                    _httpClient.CreateClientAsync(CurrentClient);
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
