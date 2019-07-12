using System;

namespace Sample.Domain.Domain.Order
{
    public class Order : IAggregateRoot
    {
        private Order()
        {}

        public Order(string orderCode, OrderStatus status, OrderDetails orderDetails)
        {
            OrderCode = orderCode;
            Status = status;
            OrderDetails = orderDetails;
            IsDeleted = false;
        }

        public void SetOrderDetails(OrderDetails orderDetails)
        {
            OrderDetails = orderDetails;
            ModifiedDate = DateTime.UtcNow;
        }

        public void Delete()
        {
            IsDeleted = true;
        }

        public int Id { get; set; }
        public string OrderCode { get; private set; }
        public OrderStatus Status { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime ModifiedDate { get; private set; }
        public OrderDetails OrderDetails { get; private set; }
    }
}
