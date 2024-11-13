using StablingApiClient;
using StablingClientWPF.ViewModels.Dialogs;
using StablingClientWPF.Views;
using StablingClientWPF.Views.Dialogs;

namespace StablingClientWPF.Helpers
{
    public class DialogManager
    {
        private readonly AbonementsHttpClient _abonementsHttpClient;
        private readonly ClientsHttpClient _clientsHttpClient;
        private readonly TrainersHttpClient _trainersHttpClient;
        private readonly BalanceWithdrawingsHttpClient _balanceWithdrawingsHttpClient;
        private readonly Mediator _mediator;

        public DialogManager(Mediator mediator,AbonementsHttpClient abonementsHttpClient,
            ClientsHttpClient clientsHttpClient, TrainersHttpClient trainersHttpClient, BalanceWithdrawingsHttpClient balanceWithdrawingsHttpClient)
        {
            _mediator = mediator;
            _abonementsHttpClient = abonementsHttpClient;
            _clientsHttpClient = clientsHttpClient;
            _trainersHttpClient = trainersHttpClient;
            _balanceWithdrawingsHttpClient = balanceWithdrawingsHttpClient;
        }

        private const string RootIdentifier = "Root";

        public async Task<Abonement?> OpenAbonementCreationFormAsync()
        {
            return (Abonement?)await MaterialDesignThemes.Wpf.DialogHost.Show(
                new AbonementCreationDialog(
                    new AbonementCreationDialogViewModel(
                    new Abonement()
                    {
                        OpenDateTime = DateTime.Now,
                        IsAvailable = true,
                    },
                    await _clientsHttpClient.GetByAvailabilityAsync(true),
                    await _abonementsHttpClient.GetTypesAsync(),
                    await _trainersHttpClient.GetAllAsync())), RootIdentifier);
        }

        public async Task<Abonement?> OpenAbonementCreationFormAsync(int clientId)
        {
            return (Abonement?)await MaterialDesignThemes.Wpf.DialogHost.Show(
                new AbonementCreationDialog(
                    new AbonementCreationDialogViewModel(
                    new Abonement()
                    {
                        ClientId = clientId,
                        OpenDateTime = DateTime.Now,
                        IsAvailable = true,
                    },
                    await _clientsHttpClient.GetByAvailabilityAsync(true),
                    await _abonementsHttpClient.GetTypesAsync(),
                    await _trainersHttpClient.GetAllAsync())), RootIdentifier);
        }

        public async Task OpenAbonementDetailsDialogAsync(int abonementId)
        {
            await MaterialDesignThemes.Wpf.DialogHost.Show(
                new AbonementsDetailsDialog(new AbonementsDetailsDialogViewModel(_mediator,
                    _abonementsHttpClient, _balanceWithdrawingsHttpClient, DateTime.Now, abonementId)), RootIdentifier);
        }

        public async Task<int?> OpenAbonementListForCreateUsageDialog(IEnumerable<AbonementForShow> abonements)
        {
            return (int?)await MaterialDesignThemes.Wpf.DialogHost.Show(
                 new AbonementUsageDialog(new AbonementUsageDialogViewModel(
                    abonements)), RootIdentifier);
        }

        public async Task<Training?> OpenTrainingCreateDialog(IEnumerable<Trainer> trainers,
            IEnumerable<Client> clients,
            IEnumerable<TrainingType> trainingTypes,
            IEnumerable<Horse> horses,DateTime dateTime)
        {
            return (Training?)await MaterialDesignThemes.Wpf.DialogHost.Show(
                 new TrainingDialog(new TrainingDialogViewModel(
                    trainers,clients,trainingTypes,horses, dateTime)), RootIdentifier);
        }

        public async Task<Training?> OpenTrainingUpdateDialog(IEnumerable<Trainer> trainers,
            IEnumerable<Client> clients,
            IEnumerable<TrainingType> trainingTypes,
            IEnumerable<Horse> horses,Training training)
        {
            return (Training?)await MaterialDesignThemes.Wpf.DialogHost.Show(
                 new TrainingDialog(new TrainingDialogViewModel(
                    trainers, clients, trainingTypes, horses,training)), RootIdentifier);
        }

        public async Task<Training?> OpenTrainingCopyDialog(IEnumerable<Trainer> trainers,
            IEnumerable<Client> clients,
            IEnumerable<TrainingType> trainingTypes,
            IEnumerable<Horse> horses, Training training)
        {
            training.TrainingId = 0;
            return (Training?)await MaterialDesignThemes.Wpf.DialogHost.Show(
                 new TrainingDialog(new TrainingDialogViewModel(
                    trainers, clients, trainingTypes, horses, training)), RootIdentifier);
        }
    }
}
