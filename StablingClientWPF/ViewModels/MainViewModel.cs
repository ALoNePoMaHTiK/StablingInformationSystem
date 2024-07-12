using MaterialDesignThemes.Wpf;
using StablingApiClient;
using StablingClientWPF.Helpers;
using StablingClientWPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private Mediator _mediator;

        public MainViewModel(Mediator mediator,ClientsViewModel _ClientsViewModel,
                    AbonementsViewModel _AbonementsViewModel,
                    TrainingsViewModel _TrainingsViewModel,
                    AdministrationViewModel _AdministrationViewModel,
                    DayOperationsViewModel _DayOperationsViewModel)
        {
            _mediator = mediator;

            _mediator.GetClientInfo += OpenClientsTab;

            ClientsViewModel = _ClientsViewModel;
            AbonementsViewModel = _AbonementsViewModel;
            TrainingsViewModel = _TrainingsViewModel;
            AdministrationViewModel = _AdministrationViewModel;
            DayOperationsViewModel = _DayOperationsViewModel;
        }

        private int _SelectedTab;
        public int SelectedTab
        {
            get { return _SelectedTab; }
            set { _SelectedTab = value; OnPropertyChanged(); }
        }

        private void ChangeTab(int tabIndex) => SelectedTab = tabIndex;
        private void OpenClientsTab(int clientId) => ChangeTab(0);      //ДАЛЕКО НЕ ЛУЧШЕЕ РЕШЕНИЕ ПРИВЯЗЫВАТЬСЯ К индексу, который может меняться

    }
}
