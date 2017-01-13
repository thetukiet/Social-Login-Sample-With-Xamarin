using System;
using System.Net.Mail;

namespace Sample.Common.Helpers
{
    public class ValidationHelper
    {
        public static bool IsValidUri(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.Absolute);
        }


        public static bool IsValidEmailaddress(string emailAddress)
        {
            try
            {
                var email = new MailAddress(emailAddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
