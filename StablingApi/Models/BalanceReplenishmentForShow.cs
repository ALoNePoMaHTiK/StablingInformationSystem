﻿namespace StablingApi.Models
{
    public class BalanceReplenishmentForShow
    {
        public Guid BalanceReplenishmentId { get; set; }
        public string TrainerName { get; set; }
        public string ClientName { get; set; }
        public double Amount { get; set; }
        public DateTime ReplenishmentDate { get; set; }
    }
}
