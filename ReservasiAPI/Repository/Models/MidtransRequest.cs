namespace ReservasiAPI.Models
{
    public class MidtransRequest
    {
        public string OrderId { get; set; }
        public int Amount { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
    }
}
