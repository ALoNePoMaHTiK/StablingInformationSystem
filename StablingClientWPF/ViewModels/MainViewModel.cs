using StablingApiClient;
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

        public MainViewModel(ClientsViewModel _clientsViewModel,
                    TrainingsViewModel _trainingsViewModel)
        {
            ClientsViewModel = _clientsViewModel;
            TrainingsViewModel = _trainingsViewModel;
        }

    }
}
