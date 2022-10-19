using System.Runtime.Serialization;

namespace EpsilonDemoWebsite.Models
{
    [DataContract]
    public class timeCartianModel
    {

        [DataMember(Name = "x")]
        public string? x = null;

        
        [DataMember(Name = "y")]
        public int? y = null;

        [DataMember(Name = "T")]
        public string? T = null;
        public timeCartianModel(string? x, int? y,string T)
        {
            this.x = x;
            this.y = y;
            this.T = T;
        }


    }

}
