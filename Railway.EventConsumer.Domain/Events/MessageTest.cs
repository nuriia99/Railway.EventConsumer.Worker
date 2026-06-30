namespace Railway.EventConsumer.Domain.Events
{
    public record MessageTest
    {
        public Guid MessageId { get; set; }
        public decimal Amount { get; set; }
    }
}
