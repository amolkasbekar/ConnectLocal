using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Configuration;
using System.Xml.Serialization;

using RestSharp;

namespace ConnectSample.Models
{
	[XmlType("Access_Token")]
	public class AuthorizationToken
	{
		private static string OAUTH_URI = ConfigurationManager.AppSettings["ConcurConnectBaseUrl"] + @"/net2/oauth2/accesstoken.ashx";

		[XmlElement("Instance_Url")]
		public string InstanceUrl { get; set; }

		[XmlElement("Token")]
		public string Token { get; set; }

		[XmlElement("Expiration_date")]
		public DateTime? ExpirationDate { get; set; }

		[XmlElement("Refresh_Token")]
		public string RefreshToken { get; set; }

		public static AuthorizationToken GetAuthorizationToken(User user)
		{
			var client = Utils.GetRestClient(user);

			var request = new RestRequest("net2/oauth2/accesstoken.ashx", Method.GET);
			request.AddHeader("X-ConsumerKey", ConfigurationManager.AppSettings["ConcurConsumerKey"]);

			var response = client.Execute<AuthorizationToken>(request);
			if (response.Data == null || response.Data.Token == null) throw new ApplicationException(response.StatusDescription);

			AuthorizationToken authToken = response.Data;
			return authToken;
		}

	}
}