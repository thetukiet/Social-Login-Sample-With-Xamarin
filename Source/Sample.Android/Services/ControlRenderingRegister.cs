using System;
using Sample.Droid.Renderers;

namespace Sample.Droid.Services
{
    public class ControlRenderingRegister
    {
        public static void Register()
        {
            try
            {
                CircleImageRenderer.Init();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }
    }
}