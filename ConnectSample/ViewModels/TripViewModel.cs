using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Configuration;

using RestSharp;
using YelpSharp;
using YelpSharp.Data;
using YelpSharp.Data.Options;

namespace ConnectSample.ViewModels
{
	public class TripViewModel
	{
		public string TripName;
		public string Description;
		public DateTime StartDateLocal;
		public DateTime EndDateLocal;

		public List<string> Destinations = new List<string>();
		public Dictionary<string, object> DestinatationResults = new Dictionary<string,object> ();

		public void LoadDestinationData()
		{
			foreach (string destination in Destinations)
			{ 
				var options = new Options()
				{
					AccessToken = ConfigurationManager.AppSettings["YelpToken"],
					AccessTokenSecret = ConfigurationManager.AppSettings["YelpTokenSecret"],
					ConsumerKey = ConfigurationManager.AppSettings["YelpConsumerKey"],
					ConsumerSecret = ConfigurationManager.AppSettings["YelpConsumerSecret"]
				};

				Yelp yelp = new Yelp(options);
				//SearchOptions searchOptions = new SearchOptions();
				//searchOptions.LocationOptions = new LocationOptions() { location = destination };
				//searchOptions.GeneralOptions = new GeneralOptions() { term = "restaurants", limit = 25 };
				//var results = yelp.Search(searchOptions).Result; 
				
				var results = yelp.Search("restaurants", destination).Result;
				DestinatationResults.Add(destination, results);
			}
		}

	}
}