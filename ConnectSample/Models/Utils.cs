using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text.RegularExpressions;

using RestSharp;

namespace ConnectSample.Models
{
	public static class Utils
	{
		/// <summary>
		/// Returns empty string "" if the object is null.
		/// </summary>
		/// <param name="object">Object to convert to a string</param>
		/// <returns>The string version of the object, or an empty string if the object is null</returns>
		public static string ToStringNotNull(this object @object)
		{
			return (@object == null || @object == DBNull.Value) ? "" : @object.ToString();
		}

		/// <summary>
		/// Returns empty string "" if the object is null.
		/// </summary>
		/// <param name="object">Object to convert to a string</param>
		/// <param name="format">A format string to use when converting the object to a string</param>
		/// <returns>The string version of the object, or an empty string if the object is null</returns>
		public static string ToStringNotNull<T>(this T? @object, string format) where T : struct, IFormattable
		{
			return (@object == null) ? "" : ((T)@object).ToString(format, null);
		}

		/// <summary>
		/// Returns empty string "" if the object is null.
		/// </summary>
		/// <param name="object">Object to convert to a string</param>
		/// <param name="format">A format string to use when converting the object to a string</param>
		/// <param name="formatProvider">The format provider object, which may be null to use the default</param>
		/// <returns>The string version of the object, or an empty string if the object is null</returns>
		public static string ToStringNotNull<T>(this T? @object, string format, IFormatProvider formatProvider) where T : struct, IFormattable
		{
			return (@object == null) ? "" : ((T)@object).ToString(format, formatProvider);
		}

		/// <summary>
		/// Converts object value returned from a database to N? (Nullable) replacing DBNull with null 
		/// </summary>
		/// <param name="inp">Input</param>
		/// <returns>Nullable value of specified type</returns>
		public static TN? GetNullable<TN>(object inp) where TN : struct
		{
			if (inp == DBNull.Value || inp == null)
			{
				return null;
			}
			else
			{
				return (TN?)System.Convert.ChangeType(inp, typeof(TN));
			}
		}

		public static RestClient GetRestClient(User user)
		{
			var client = new RestClient(ConfigurationManager.AppSettings["ConcurConnectBaseUrl"]);
			client.ClearHandlers();
			client.AddHandler(@"application/xml", new RestSharp.Deserializers.XmlDeserializer());

			if (user.OAuthToken != null)
				client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(user.OAuthToken.Token);
			else
				client.Authenticator = new HttpBasicAuthenticator(user.ConcurUserName, user.ConcurPassword);


			return client;
		}

		public static string KillNamespace(string xmlString)
		{

			Regex re = new System.Text.RegularExpressions.Regex("\\sxmlns(:[A-Za-z0-9]+)?=\"[^\"]*\"", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

			string newXmlString = re.Replace(xmlString, "");
			re = new System.Text.RegularExpressions.Regex("\\sxsi(:[A-Za-z0-9]+)?=\"[^\"]*\"", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			newXmlString = re.Replace(newXmlString, "");
			return newXmlString;

		}
	
	}
}