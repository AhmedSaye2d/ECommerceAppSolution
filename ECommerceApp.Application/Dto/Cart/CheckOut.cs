namespace ECommerceApp.Application.Dto.Cart
{
    public class CheckOut
    {
        public required Guid PaymentMethodId { get; set; }  
        public required IEnumerable<ProcessCart> Carts { get; set; }
    }
}
