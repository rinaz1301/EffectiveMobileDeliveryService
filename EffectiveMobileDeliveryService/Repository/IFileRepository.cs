using EffectiveMobileDeliveryService.Models;

namespace EffectiveMobileDeliveryService.Repository
{
    public interface IFileRepository
    {
        List<DeliveryOrder> GetDeliveryOrders();
        string GetJsonDeliveryOrders();
    }
}
