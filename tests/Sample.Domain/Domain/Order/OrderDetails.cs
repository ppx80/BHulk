using System.Collections.Generic;

namespace Sample.Domain.Domain.Order
{
    public class OrderDetails : ValueObject
    {
        private OrderDetails()
        {}

        public OrderDetails(decimal? totalAmount, Address billingAddress, Address shippingAddress)
        {
            TotalAmount = totalAmount;
            BillingAddress = billingAddress;
            ShippingAddress = shippingAddress;
        }

        public decimal? TotalAmount { get; private set; }
        public Address BillingAddress { get; private set; }
        public Address ShippingAddress { get; private set; }

        public static OrderDetails Empty() => new OrderDetails(0, Address.Empty(), Address.Empty());

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TotalAmount;
            yield return BillingAddress;
            yield return ShippingAddress;
        }
    }
}
