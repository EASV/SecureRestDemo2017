using System;
using System.Collections.Generic;
using System.Text;
using CustomerAppBLL.BusinessObjects;
using CustomerAppDAL;
using CustomerAppBLL.Converters;
using System.Linq;
using CustomerAppDAL.Entities;

namespace CustomerAppBLL.Services
{
    class OrderService : IOrderService
    {
        IConverter<Order, OrderBO> _conv;
        private IDALFacade _facade;
        public OrderService(IDALFacade facade, 
                            IConverter<Order, OrderBO> conv = null)
        {
            _facade = facade;
            _conv = conv ?? new OrderConverter();
        }

        public OrderBO Create(OrderBO order)
        {
            using (var uow = _facade.UnitOfWork)
            {
                var orderEntity = uow.OrderRepository.Create(_conv.Convert(order));
                uow.Complete();
                return _conv.Convert(orderEntity);
            }
        }

        public OrderBO Delete(int Id)
        {
            using (var uow = _facade.UnitOfWork)
            {
                var orderEntity = uow.OrderRepository.Delete(Id);
                uow.Complete();
                return _conv.Convert(orderEntity);
            }
        }

        public OrderBO Get(int Id)
        {
            using (var uow = _facade.UnitOfWork)
            {
                var orderEntity = uow.OrderRepository.Get(Id);
                orderEntity.Customer = uow.CustomerRepository.Get(orderEntity.CustomerId);
                return _conv.Convert(orderEntity);
            }
        }

        public List<OrderBO> GetAll()
        {
            using (var uow = _facade.UnitOfWork)
            {
                return uow.OrderRepository.GetAll().Select(_conv.Convert).ToList();
            }
        }

        public OrderBO Update(OrderBO order)
        {
            using (var uow = _facade.UnitOfWork)
            {
                var orderEntity = uow.OrderRepository.Get(order.Id);
                if(orderEntity == null)
                {
                    throw new InvalidOperationException("Order not found");
                }
                orderEntity.DeliveryDate = order.DeliveryDate;
                orderEntity.OrderDate = order.OrderDate;
                orderEntity.CustomerId = order.CustomerId;
                uow.Complete();
                //BLL choice
                orderEntity.Customer = uow.CustomerRepository.Get(orderEntity.CustomerId);
                return _conv.Convert(orderEntity);
            }
        }
    }
}
