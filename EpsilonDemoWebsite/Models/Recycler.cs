namespace EpsilonDemoWebsite.Models
{
    public class Recycler
    {
        public string RecyclerId { get; set; }
        public string Name { get; set; }
        public bool IndustrialWaste { get; set; }
        public bool CardboardandPaper { get; set; }
        public bool Plastic { get; set; }
        public bool Glass { get; set; }
        public bool GardenWaste { get; set; }
        public bool GeneralWaste { get; set; }

        public bool Active { get; set; }
        public Recycler(string RecyclerId, string Name, bool IndustrialWaste, bool CardboardandPaper, bool Plastic, bool Glass, bool GardenWaste, bool GeneralWaste,bool active)
        {
            this.RecyclerId = RecyclerId;
            this.Name = Name;
            this.IndustrialWaste = IndustrialWaste;
            this.CardboardandPaper = CardboardandPaper;
            this.Plastic = Plastic;
            this.Glass = Glass;
            this.GardenWaste = GardenWaste;
            this.GeneralWaste = GeneralWaste;
            Active = active;

        }
    }
}
