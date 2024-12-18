﻿namespace StablingApi.Models
{
    public class BalanceReplenishment
    {
        public Guid BalanceReplenishmentId { get; set; }
        public int ClientId { get; set; }
        public int TrainerId { get; set; }
        public DateTime ReplenishmentDate { get; set; }
        public double Amount { get; set; }
    }
}