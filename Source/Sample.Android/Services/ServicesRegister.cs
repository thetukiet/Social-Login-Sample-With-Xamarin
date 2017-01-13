using System;
using Sample.Common.Helpers;
using XLabs.Ioc;

namespace Sample.Droid.Services
{
    public class ServicesRegister
    {
        public static void Register()
        {
            try
            {
                var container = new SimpleContainer();
                container.Register<ICommonHelper, DroidHelper>();
                Resolver.SetResolver(container.GetResolver());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }
    }
}