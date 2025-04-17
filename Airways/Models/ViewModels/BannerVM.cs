namespace Airways.ViewModels
{
    public class BannerVM
    {
        public bool IsLoggedIn { get; set; }
        public bool HasPreferredCity { get; set; }
        public string PreferredCityName { get; set; }
        public decimal FlightPrice { get; set; }
    }
}