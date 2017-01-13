using System;
using Sample.Common.Helpers;
using XLabs.Ioc;

namespace Sample.iOS.Services
{
    public class ServicesRegister
    {
        public static void Register()
        {
            try
            {
                var container = new SimpleContainer();
                container.Register<ICommonHelper, iOsHelper>();
                Resolver.SetResolver(container.GetResolver());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}