using System.Runtime.Serialization;

namespace EpsilonDemoWebsite.Models
{
	[DataContract]
	public class Bubble
	{
		public Bubble(string label, DataPoint[] data)
		{
			this.Label = label;
			this.Data = data;
		}

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "label")]
		public string? Label = null;

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "data")]
		public DataPoint[]? Data = null;
	}

	[DataContract]
	public class DataPoint
    {
		public DataPoint(double x, double y, double r)
		{
			this.X = x;
			this.Y = y;
			this.R = r;
		}

		[DataMember(Name = "x")]
		public Nullable<double> X = null;

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "y")]
		public Nullable<double> Y = null;

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "r")]
		public Nullable<double> R = null;
	}
}
