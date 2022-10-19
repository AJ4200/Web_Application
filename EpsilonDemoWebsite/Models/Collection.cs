using System.ComponentModel.DataAnnotations;


namespace EpsilonDemoWebsite.Models
{
    public class Collection
    {
        public int CollectionNumber { get; set; }
        public string Landfill { get; set; }
        public int Request { get; set; }
        public DateTime CollectionDate { get; set; }

        public virtual Location LandfillNavigation { get; set; }
        public virtual Request RequestNavigation { get; set; }


        public Collection()
        {

        }

        public Collection(int collectionNumber, string landfill, int request, DateTime collectionDate, Location landfillNavigation, Request requestNavigation)
        {
            CollectionNumber = collectionNumber;
            Landfill = landfill;
            Request = request;
            CollectionDate = collectionDate;
            LandfillNavigation = landfillNavigation;
            RequestNavigation = requestNavigation;

        }
    }


}
