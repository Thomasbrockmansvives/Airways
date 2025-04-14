namespace Airways.ViewModels
{
    public class CartVM
    {
        public List<CartItemVM> Items { get; set; } = new List<CartItemVM>();
        public decimal TotalCartPrice => Items.Sum(i => i.TotalPrice);
    }
}