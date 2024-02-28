namespace BarterExchange.Data.Classes
{
    public class OrderValue
    {
        public int OrderId { get; set; }

        public int Value { get; set; }

        public OrderValue(int orderId, int value)
        {
            OrderId = orderId;
            Value = value;
        }

    }
}
