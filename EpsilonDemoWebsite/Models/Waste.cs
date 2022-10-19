namespace EpsilonDemoWebsite.Models
{
    public class Waste
    {
        public int WasteNumber { get; set; }
        public string Name { get; set; }

        public Waste(int wasteNumber, string name)
        {
            WasteNumber = wasteNumber;
            Name = name;
        }

        public Waste()
        {
        }
    }
}
