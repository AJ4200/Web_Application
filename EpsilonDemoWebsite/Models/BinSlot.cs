namespace EpsilonDemoWebsite.Models
{
    public class BinSlot
    {

        public int SlotNumber { get; set; }
        public string Bin { get; set; }
        public string GardenSite { get; set; }



        public BinSlot(int slotNumber, string bin, string gardenSite)
        {
            SlotNumber = slotNumber;
            Bin = bin;
            GardenSite = gardenSite;
        }
        public BinSlot()
        {

        }
    }
}

