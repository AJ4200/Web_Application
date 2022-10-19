namespace EpsilonDemoWebsite.Models
{
    public partial class RecycleRequestView
    {
        public Guid Id { get; set; }
        public DateTime RequestDate { get; set; }
        public string Bin { get; set; }
        public string Waste { get; set; }
        public string GardenSite { get; set; }
    }
}
