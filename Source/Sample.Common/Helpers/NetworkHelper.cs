
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Sample.Common.Entities;

namespace Sample.Common.Helpers
{
	public static class NetworkHelper
    {
	    public static bool IsNetworkAvailable()
	    {
	        return CrossConnectivity.Current.IsConnected;
	    }

	    public static async Task<SocialAccountInfo> GetFacebookUserInfoByToken(string accessToken)
	    {
	        var client = new WebClient();

	        try
	        {
	            using (client)
	            {
	                var requestUrl = String.Format(Constant.FACEBOOK_ACCESS_WORKLINK, accessToken, Constant.FACEBOOK_ACCESS_SCOPE);
	                var requestUri = new Uri(requestUrl);
	                var response = await client.DownloadDataTaskAsync(requestUri);
	                var responseString = Encoding.Default.GetString(response);

	                var obj = JObject.Parse(responseString);

	                var id = obj["id"].ToString().Replace("\"", "");
	                var email = obj["email"].ToString().Replace("\"", "");
	                var firstName = obj["first_name"].ToString().Replace("\"", "");
	                var lastName = obj["last_name"].ToString().Replace("\"", "");
	                var avatarUrl = obj["picture"]["data"]["url"].ToString().Replace("\"", "");
	                return new SocialAccountInfo
	                {
	                    Id = id,
	                    Email = email,
	                    Name = String.Format("{0} {1}", firstName, lastName),
	                    AvatarUrl = avatarUrl
	                };
	            }
	        }
	        catch (Exception)
	        {
	            return null;
	        }
	        finally
	        {
	            if (client.IsBusy)
	            {
	                client.CancelAsync();
	                client.Dispose();
	            }
	        }
	    }

	    public static async Task<SocialAccountInfo> GetGooglePlusUserInfoByToken(string accessToken)
	    {
	        var client = new WebClient();

	        try
	        {
	            using (client)
	            {
	                var requestUrl = String.Format(Constant.GOOGLEPLUS_ACCESS_WORKLINK, accessToken);
	                var requestUri = new Uri(requestUrl);
	                var response = await client.DownloadDataTaskAsync(requestUri);
	                var responseString = Encoding.Default.GetString(response);

	                var obj = JObject.Parse(responseString);

	                var id = obj["id"].ToString().Replace("\"", "");
	                var email = obj["email"].ToString().Replace("\"", "");
	                var fullName = obj["name"].ToString().Replace("\"", "");
	                var avatarUrl = obj["picture"].ToString().Replace("\"", "");
	                return new SocialAccountInfo
	                {
	                    Id = id,
	                    Email = email,
	                    Name = fullName,
	                    AvatarUrl = avatarUrl
	                };
	            }
	        }
	        catch (Exception)
	        {
	            return null;
	        }
	        finally
	        {
	            if (client.IsBusy)
	            {
	                client.CancelAsync();
	                client.Dispose();
	            }
	        }
	    }
    }
}
