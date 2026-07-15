namespace ECommerceAPI.Domain;

public enum OrderStatus {
    Unpaid = 1,
    Paid = 2,
    Shipping = 3,
    Completed = 4,
    Cancelled = 5
}