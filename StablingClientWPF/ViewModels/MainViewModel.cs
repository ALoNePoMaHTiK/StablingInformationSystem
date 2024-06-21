using MaterialDesignThemes.Wpf;
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
        public AbonementsViewModel AbonementsViewModel { get; }
        public TrainingsViewModel TrainingsViewModel { get; }
        public AdministrationViewModel AdministrationViewModel { get; }
        public DayOperationsViewModel DayOperationsViewModel { get; }
        public MainViewModel(ClientsViewModel _ClientsViewModel,
                    AbonementsViewModel _AbonementsViewModel,
                    TrainingsViewModel _TrainingsViewModel,
                    AdministrationViewModel _AdministrationViewModel,
                    DayOperationsViewModel _DayOperationsViewModel)
        {
            ClientsViewModel = _ClientsViewModel;
            AbonementsViewModel = _AbonementsViewModel;
            TrainingsViewModel = _TrainingsViewModel;
            AdministrationViewModel = _AdministrationViewModel;
            DayOperationsViewModel = _DayOperationsViewModel;

        }

    }
}
