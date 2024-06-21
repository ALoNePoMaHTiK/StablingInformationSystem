using StablingApiClient;
using StablingClientWPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StablingClientWPF.ViewModels
{
    public class AbonementsViewModel : BaseViewModel
    {
        private AbonementsHttpClient _abonementsHttpClient;
        public AbonementsViewModel(AbonementsHttpClient abonementsHttpClient)
        {
            _abonementsHttpClient = abonementsHttpClient;
            GetAbonements();
        }

        private ObservableCollection<AbonementForShow> _Abonements;
        public ObservableCollection<AbonementForShow> Abonements
        {
            get { return _Abonements; }
            set { _Abonements = value; OnPropertyChanged(); }
        }

        public AsyncDelegateCommand GetAbonementsCommand
        {
            get
            {
                return new AsyncDelegateCommand(async o => { await GetAbonements(); }, ex => MessageBox.Show(ex.ToString()));
            }
        }
        private async Task GetAbonements()
        {
            Abonements = new ObservableCollection<AbonementForShow>(await _abonementsHttpClient.GetAllForShowAsync());
        }

    }
}
