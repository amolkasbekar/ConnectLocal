using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;


namespace ConnectSample.ViewModels
{
	public class TripDigestViewModel
	{
		public string id;
		public long TripId;
		public string TripName;
		public string Description;
		public DateTime StartDateLocal;
		public DateTime EndDateLocal;
		public DateTime DateModifiedUtc;
	}
}