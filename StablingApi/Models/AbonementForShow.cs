﻿namespace StablingApi.Models
{
    public class AbonementForShow
    {
        public int AbonementId { get; set; }
        public int AbonementTypeId { get; set; }
        public string TypeName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int TrainerId { get; set; }
        public string TrainerName { get; set; }
        public int MaxUses { get; set; }
        public int UsesCount { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPaid { get; set; }
        public DateTime OpenDateTime { get; set; }
        public double AbonementFunds { get; set; }
        public string AbonementLabel { get; set; }
    }
}
