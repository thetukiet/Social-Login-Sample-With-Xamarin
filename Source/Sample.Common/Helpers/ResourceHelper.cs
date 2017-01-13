using Xamarin.Forms;
using Sample.Common.Enums;

namespace Sample.Common.Helpers
{
    public class ResourceHelper
    {
        public static ImageSource GetEmbeddedImageSource(string imageName)
        {
            var resourcePath = GetResourceFullPath(ResourceType.Image, imageName);
            return ImageSource.FromResource(resourcePath);
        }


        public static ImageSource GetEmbeddedIconSource(string iconName)
        {
            var resourcePath = GetResourceFullPath(ResourceType.Icon, iconName);
            return ImageSource.FromResource(resourcePath);
        }



        public static string GetResourceFullPath(ResourceType resourceType, string resourceName)
        {
            string[] resourceFolders = { "Icons", "Images", "Gifs"};
            var currentNameSpace = GetResourceNameSpace();
            var folderName = resourceFolders[(int)resourceType];

            return string.Format("{0}.{1}.{2}", currentNameSpace, folderName, resourceName);
        }

        private static string GetResourceNameSpace()
        {
            var platform = Device.OS;
            switch (platform)
            {
                case TargetPlatform.iOS:
                    return "Sample.iOS";
                case TargetPlatform.Android:
                    return "Sample.Droid";
            }

            return string.Empty;
        }
    }
}
