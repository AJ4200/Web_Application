namespace EpsilonDemoWebsite.Models
{
    public class Truck
    {


        public string TruckId { get; set; }
        public string NumberPlate { get; set; }
        public string? Bin { get; set; }
        public string Driver { get; set; }
        public bool Active { get; set; }

        public Truck(string truckid, string numberplate, string? bin, string driver, bool active)
        {
            TruckId = truckid;
            NumberPlate = numberplate;
            Bin = bin;
            Driver = driver;
            Active = active;

        }

        public Truck()
        {

        }
    }
}
