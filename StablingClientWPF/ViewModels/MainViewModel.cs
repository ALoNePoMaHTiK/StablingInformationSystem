using StablingApiClient;
using StablingClientWPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StablingClientWPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ClientsViewModel ClientsViewModel { get; }
        public TrainingsViewModel TrainingsViewModel { get; }
        public AdministrationViewModel AdministrationViewModel { get; }
        public MoneyTransactionsViewModel MoneyTrainsactionsViewModel { get; }

        public MainViewModel(ClientsViewModel _clientsViewModel,
                    TrainingsViewModel _trainingsViewModel,
                    AdministrationViewModel _administrationViewModel,
                    MoneyTransactionsViewModel _moneyTrainsactionsViewModel)
        {
            ClientsViewModel = _clientsViewModel;
            TrainingsViewModel = _trainingsViewModel;
            AdministrationViewModel = _administrationViewModel;
            MoneyTrainsactionsViewModel = _moneyTrainsactionsViewModel;
        }

    }
}
