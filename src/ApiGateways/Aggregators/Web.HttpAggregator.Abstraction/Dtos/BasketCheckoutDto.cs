﻿namespace Web.HttpAggregator.Abstraction.Dtos
{
    public class BasketCheckoutDto
    {
        public string? RequestId { get; set; }
        public string? UserId { get; set; }
        public string? UserEmail { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? CardNumber { get; set; }
        public string? CardHolderName { get; set; }
        public DateTime CardExpiration { get; set; }
        public string? CardSecurityCode { get; set; }
    }
}
