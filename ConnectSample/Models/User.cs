using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Xml;

using RestSharp;

namespace ConnectSample.Models
{
	public class User
	{ 
		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Display(Name = "Concur User Name")]
		public string ConcurUserName { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Concur Password")]
		public string ConcurPassword { get; set; }

		private AuthorizationToken _OAuthToken { get; set; }

		public AuthorizationToken OAuthToken { get { return _OAuthToken; } }

		public void LinkToConcur()
		{
			_OAuthToken = AuthorizationToken.GetAuthorizationToken(this);
		}

		public void SubscribeToChanges(string type)
		{
			var client = Utils.GetRestClient(this);

			var subscriptionUri = string.Format("/api/user/v1.0/subscribe?type={0}", type);
			var request = new RestRequest(subscriptionUri, Method.GET);
			var response = client.Execute(request);
			XmlDocument responseDoc = new XmlDocument();
			responseDoc.LoadXml(Utils.KillNamespace(response.Content));
		}

		public bool Load()
		{
			var connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
			using (var conn = new SqlConnection(connString))
			{
				conn.Open();
				using (var command = new SqlCommand("CONNECT_GetUser", conn))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@userName", UserName);

					using (var dr = command.ExecuteReader())
					{
						if (dr.RecordsAffected == 0)
							return false;

						while (dr.Read())
						{
							_OAuthToken = new AuthorizationToken() { 
								Token = dr["OAuth_Token"].ToStringNotNull(),
								ExpirationDate	= Utils.GetNullable<DateTime>(dr["Oauth_Expiration_date"]),
								InstanceUrl = dr["Instance_Url"].ToStringNotNull(),
								RefreshToken = dr["OAuth_Refresh_Token"].ToStringNotNull()
							};
						}
					}
				}
			}
			return true;
		}

		public void Save()
		{ 
			var connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
			using (var conn = new SqlConnection(connString))
			{
				conn.Open();
				using (var command = new SqlCommand("CONNECT_SaveUser", conn))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@userName", UserName);
					command.Parameters.AddWithValue("@userPassword", Password);
					if (this.OAuthToken != null)
					{
						command.Parameters.AddWithValue("@token", this.OAuthToken.Token);
						command.Parameters.AddWithValue("@expiration_date", this.OAuthToken.ExpirationDate);
						command.Parameters.AddWithValue("@instanceUrl", this.OAuthToken.InstanceUrl);
						command.Parameters.AddWithValue("@refreshToken", this.OAuthToken.RefreshToken);
					}
					command.ExecuteNonQuery();
				}
			}
		}


	}
}