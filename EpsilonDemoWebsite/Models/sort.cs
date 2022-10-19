using System.Runtime.Serialization;

namespace EpsilonDemoWebsite.Models
{
    [DataContract]
    public class sort
    {
        [DataMember(Name = "label")]
        public string? datelabel = null;
        [DataMember(Name = "label")]
        public int sortType = 0;

        public sort(string datelabel, int sortType)
        {
            this.datelabel = datelabel;
            this.sortType = sortType;


        }
    }
}
