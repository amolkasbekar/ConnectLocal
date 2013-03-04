using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

using RestSharp;
using ConnectSample.ViewModels;

namespace ConnectSample.Models
{
	public class Trip
	{
		public static List<TripDigestViewModel> GetTripDigest(User user)
		{
			var client = Utils.GetRestClient(user);

			var request = new RestRequest("/api/travel/trip/v1.1", Method.GET);
			request.AddParameter("startDate", "2012/01/01");
			request.AddParameter("endDate", "2013/12/31");

			var response = client.Execute(request);
			XmlDocument responseDoc = new XmlDocument();
			responseDoc.LoadXml(Utils.KillNamespace(response.Content));

			XmlNodeList tripListNodes = responseDoc.SelectNodes("/ItineraryInfoList/ItineraryInfo");
			List<TripDigestViewModel> tripDigestVMList = new List<TripDigestViewModel>();
			foreach (XmlNode tripNode in tripListNodes)
			{
				var tripDigestVM = new TripDigestViewModel() {
					TripName = tripNode["TripName"].InnerText,
					TripId = long.Parse(tripNode["TripId"].InnerText),
					id = tripNode["id"].InnerText,
					StartDateLocal = DateTime.Parse(tripNode["StartDateLocal"].InnerText),
					EndDateLocal = DateTime.Parse(tripNode["EndDateLocal"].InnerText),
					DateModifiedUtc = DateTime.Parse(tripNode["DateModifiedUtc"].InnerText)
				};
				tripDigestVMList.Add(tripDigestVM);
			}

			return tripDigestVMList;
		}

		public static TripViewModel GetTrip(User user, TripDigestViewModel t)
		{
			var client = Utils.GetRestClient(user);

			var request = new RestRequest("/api/travel/trip/v1.1/{id}", Method.GET);
			request.AddUrlSegment("id", t.TripId.ToStringNotNull());

			var response = client.Execute(request);
			XmlDocument responseDoc = new XmlDocument();
			responseDoc.LoadXml(Utils.KillNamespace(response.Content));

			TripViewModel model = BuildFromTrip(responseDoc);
			return model;
		}

		public static TripViewModel BuildFromTrip(XmlDocument trip)
		{
			TripViewModel tvm = new TripViewModel()
			{
				TripName = trip.SelectSingleNode("/Itinerary/TripName").InnerText,
				StartDateLocal = DateTime.Parse(trip.SelectSingleNode("/Itinerary/StartDateLocal").InnerText),
				EndDateLocal = DateTime.Parse(trip.SelectSingleNode("/Itinerary/EndDateLocal").InnerText),
			};


			XmlNodeList segmentList = trip.SelectNodes("/Itinerary/Bookings/*/*/*[self::Air or self::Rail or self::Hotel or self::Car]");
			foreach (XmlNode segment in segmentList)
			{
				switch (segment.Name)
				{
					case "Air":
						tvm.Destinations.Add(segment["EndCityCode"].InnerText);
						break;
					case "Rail":
						tvm.Destinations.Add(segment["EndCityCode"].InnerText);
						break;
					case "Hotel":
						tvm.Destinations.Add(segment["StartCityCode"].InnerText);
						break;
					case "Car":
						tvm.Destinations.Add(segment["StartCityCode"].InnerText);
						break;
				}

			}

			return tvm;
		}
	}
}