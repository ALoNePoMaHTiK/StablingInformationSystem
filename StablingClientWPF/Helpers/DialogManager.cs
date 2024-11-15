using StablingApiClient;
using StablingClientWPF.ViewModels;
using StablingClientWPF.ViewModels.Dialogs;
using StablingClientWPF.Views;
using StablingClientWPF.Views.Dialogs;

namespace StablingClientWPF.Helpers
{

    /// <summary>
    /// Класс для управления открытием модульных окон
    /// Модульное окно закрывается возвращая результат (какой-либо объект или Null)
    /// Если модульное окно простое, то нужно воздерживаться от передачи в его ViewModel HttpClient`ов
    /// Следует передавать коллекции
    /// </summary>
    public class DialogManager : BaseViewModel
    {
        private readonly AbonementsHttpClient _abonementsHttpClient;
        private readonly ClientsHttpClient _clientsHttpClient;
        private readonly TrainersHttpClient _trainersHttpClient;
        private readonly TrainingsHttpClient _trainingsHttpClient;
        private readonly BalanceWithdrawingsHttpClient _balanceWithdrawingsHttpClient;
        private readonly Mediator _mediator;

        public DialogManager(Mediator mediator,AbonementsHttpClient abonementsHttpClient,
            ClientsHttpClient clientsHttpClient, TrainersHttpClient trainersHttpClient,
            BalanceWithdrawingsHttpClient balanceWithdrawingsHttpClient, TrainingsHttpClient trainingsHttpClient)
        {
            _mediator = mediator;
            _abonementsHttpClient = abonementsHttpClient;
            _clientsHttpClient = clientsHttpClient;
            _trainersHttpClient = trainersHttpClient;
            _balanceWithdrawingsHttpClient = balanceWithdrawingsHttpClient;
            _trainingsHttpClient = trainingsHttpClient;
        }

        #region Абонементы

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
                    await _trainersHttpClient.GetAllAsync())), RootLayerIdentifier);
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
                    await _trainersHttpClient.GetAllAsync(),true)), RootLayerIdentifier);
            //TODO Продумать режимы модульных окон
        }

        public async Task OpenAbonementDetailsDialogAsync(int abonementId)
        {
            await MaterialDesignThemes.Wpf.DialogHost.Show(
                new AbonementsDetailsDialog(new AbonementsDetailsDialogViewModel(_mediator,this,
                    _abonementsHttpClient, _balanceWithdrawingsHttpClient, DateTime.Now, abonementId)), RootLayerIdentifier);
        }

        public async Task<int?> OpenAbonementListForCreateUsageDialog(IEnumerable<AbonementForShow> abonements)
        {
            return (int?)await MaterialDesignThemes.Wpf.DialogHost.Show(
                 new AbonementUsageDialog(new AbonementUsageDialogViewModel(
                    abonements)), RootLayerIdentifier);
        }

        public async Task<BalanceWithdrawing?> OpenWithdrawingByAbonementDialog(DateTime currentDate, AbonementForShow abonement)
        {
            return (BalanceWithdrawing?)await MaterialDesignThemes.Wpf.DialogHost.Show(
                new WithdrawingByAbonementDialog(
                    new WithdrawingByAbonementDialogViewModel(currentDate, abonement)), SecondLayerIdentifier);
        }

        #endregion

        #region Тренировки
        public async Task<Training?> OpenTrainingCreateDialog(IEnumerable<Trainer> trainers,
            IEnumerable<Client> clients,
            IEnumerable<TrainingType> trainingTypes,
            IEnumerable<Horse> horses,DateTime dateTime)
        {
            return (Training?)await MaterialDesignThemes.Wpf.DialogHost.Show(
                 new TrainingDialog(new TrainingDialogViewModel(
                    trainers,clients,trainingTypes,horses, dateTime)), RootLayerIdentifier);
        }

        public async Task<Training?> OpenTrainingUpdateDialog(IEnumerable<Trainer> trainers,
            IEnumerable<Client> clients,
            IEnumerable<TrainingType> trainingTypes,
            IEnumerable<Horse> horses,Training training)
        {
            return (Training?)await MaterialDesignThemes.Wpf.DialogHost.Show(
                 new TrainingDialog(new TrainingDialogViewModel(
                    trainers, clients, trainingTypes, horses,training)), RootLayerIdentifier);
        }

        public async Task<Training?> OpenTrainingCopyDialog(IEnumerable<Trainer> trainers,
            IEnumerable<Client> clients,
            IEnumerable<TrainingType> trainingTypes,
            IEnumerable<Horse> horses, Training training)
        {
            training.TrainingId = 0;
            return (Training?)await MaterialDesignThemes.Wpf.DialogHost.Show(
                 new TrainingDialog(new TrainingDialogViewModel(
                    trainers, clients, trainingTypes, horses, training)), RootLayerIdentifier);
        }

        public async Task OpenTrainingDetailsDialogAsync(DateTime CurrentDate, int trainingId)
        {
            await MaterialDesignThemes.Wpf.DialogHost.Show(
                new TrainingsDetailsDialog(new TrainingsDetailsDialogViewModel(_mediator,
                    _trainingsHttpClient, _balanceWithdrawingsHttpClient, CurrentDate, trainingId)), RootLayerIdentifier);
        }
        #endregion

        #region Клиенты
        public async Task<Client?> OpenClientCreateDialog(IEnumerable<Trainer> trainers)
        {
            return (Client?)await MaterialDesignThemes.Wpf.DialogHost.Show(
                 new ClientDialog(new ClientDialogViewModel(
                    trainers)), RootLayerIdentifier);
        }

        public async Task<Client?> OpenClientUpdateDialog(IEnumerable<Trainer> trainers, Client client)
        {
            return (Client?)await MaterialDesignThemes.Wpf.DialogHost.Show(
                 new ClientDialog(new ClientDialogViewModel(
                    trainers,client)), RootLayerIdentifier);
        }

        public async Task<Client?> OpenClientShowDialog(IEnumerable<Trainer> trainers, Client client)
        {
            return (Client?)await MaterialDesignThemes.Wpf.DialogHost.Show(
                 new ClientDialog(new ClientDialogViewModel(
                    trainers, client,false)), RootLayerIdentifier);
        }

        #endregion

        #region Денежные транзакции

        public async Task<MoneyTransaction?> OpenMoneyTransactionCreateDialog(IEnumerable<MoneyAccount> moneyAccounts,
            IEnumerable<Trainer> trainers,
            DateTime CurrentDate)
        {
            return (MoneyTransaction?)await MaterialDesignThemes.Wpf.DialogHost.Show(
                new MoneyTransactionDialog(new MoneyTransactionDialogViewModel(
                moneyAccounts, trainers, CurrentDate)), RootLayerIdentifier);
        }

        public async Task<MoneyTransaction?> OpenMoneyTransactionUpdateDialog(IEnumerable<MoneyAccount> moneyAccounts,
            IEnumerable<Trainer> trainers,
            DateTime CurrentDate, MoneyTransaction transaction)
        {
            return (MoneyTransaction?)await MaterialDesignThemes.Wpf.DialogHost.Show(
                new MoneyTransactionDialog(new MoneyTransactionDialogViewModel(
                moneyAccounts, trainers, transaction)), RootLayerIdentifier);
        }

        #endregion
    }
}
