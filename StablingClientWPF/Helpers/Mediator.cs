using StablingApiClient;

namespace StablingClientWPF.Helpers
{
    public class Mediator
    {
        //Обновление суточных данных при обновлении даты
        public event Action<DateTime> OnDayOperationsDateUpdated;
        public void UpdateDayOperationsDate(DateTime date)
            => OnDayOperationsDateUpdated?.Invoke(date);

        public event Action<int> GetClientInfo;
        public void ShowClientInfo(int clientId)
            => GetClientInfo?.Invoke(clientId);

        //Обновление суммы списаний по тренировке
        public event Action<int> OnTrainingFundsUpdated;
        public void UpdateTrainingFunds(int trainingId)
            => OnTrainingFundsUpdated?.Invoke(trainingId);

        //Обновление суммы списаний по абонементу
        public event Action<int> OnAbonementFundsUpdated;
        public void UpdateAbonementFunds(int trainingId)
            => OnAbonementFundsUpdated?.Invoke(trainingId);
        

        public event Action<BalanceReplenishment> OnReplenishmentByTrainingCreateNotified;
        public void NotifyReplenishmentByTrainingCreate(BalanceReplenishment replenishment)
            => OnReplenishmentByTrainingCreateNotified?.Invoke(replenishment);

        /// <summary>
        /// Уведомление о том, что необходимо создать абонемент на основании данных о клиенте
        /// </summary>
        public event Action<int> OnCreateAbonementByClient;
        public void CreateAbonementByClient(int clientId)
            => OnCreateAbonementByClient?.Invoke(clientId);

        public event Action<Abonement> OnAbonementByClientCreated;
        public void AbonementByClientCreated(Abonement createdAbonement)
            => OnAbonementByClientCreated?.Invoke(createdAbonement);

        public event Action<Training> NeedToCreateTrainingWithdrawing;
    }
}