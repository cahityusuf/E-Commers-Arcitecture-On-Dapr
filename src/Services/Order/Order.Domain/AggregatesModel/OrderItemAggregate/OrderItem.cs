namespace Order.Domain.AggregatesModel.OrderItemAggregate;

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid? OrderId { get; set; }
    public OrderAggregate.Order Order { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal? UnitPrice { get; set; }
    public int? Units { get; set; }
    public string? PictureFileName { get; set; }

}
