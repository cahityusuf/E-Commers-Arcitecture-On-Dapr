﻿using Order.Application.Actors;

namespace Order.Application.Models;

public class OrderDto
{
    public Guid Id { get; private set; }
    public int OrderNumber { get; private set; }
    public DateTime OrderDate { get; private set; }
    public string OrderStatus { get; set; }
    public string Description { get; set; }
    public AddressDto Address { get; private set; }
    public string BuyerId { get; private set; }
    public string BuyerEmail { get; private set; }
    public List<OrderItemDto>? OrderItems { get; set; }

    public OrderDto()
    {
        Id = Guid.Empty;
        OrderStatus = string.Empty;
        Description = string.Empty;
        Address = new AddressDto();
        BuyerId = string.Empty;
        BuyerEmail = string.Empty;
        OrderItems = new();
    }

    public OrderDto(Guid orderId, OrderState state)
    {
        Id = orderId;
        OrderDate = state.OrderDate;
        OrderStatus = state.OrderStatus.Name;
        BuyerId = state.BuyerId;
        BuyerEmail = state.BuyerEmail;
        Description = state.Description;
        Address = new AddressDto(
            state.Address.Street,
            state.Address.City,
            state.Address.State,
            state.Address.Country);
        OrderItems = state.OrderItems
            .Select(itemState => new OrderItemDto(itemState))
            .ToList();
    }

    public decimal GetTotal() => OrderItems.Sum(o => o.Units * o.UnitPrice);
}
