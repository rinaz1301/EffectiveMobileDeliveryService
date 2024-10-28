namespace EffectiveMobileDeliveryService.Models
{
    public class DeliveryOrder
    {
        public int OrderId { get; set; }
        public double Weight { get; set; }
        public string District { get; set; }
        public DateTime Time { get; set; }
    }
}
