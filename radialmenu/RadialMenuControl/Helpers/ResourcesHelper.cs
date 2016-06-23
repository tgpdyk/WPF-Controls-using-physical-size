using System;
using System.Windows;

namespace RadialMenuControl.Helpers
{
    public sealed class ResourcesHelper
    {
        static ResourceDictionary _Resources = null;
        const string ResourceUriPath = "/RadialMenuControl;component/Resources/RadialMenu.xaml";

        static readonly ResourcesHelper _Instance = new ResourcesHelper();

        private ResourcesHelper()
        {
            _Resources = new ResourceDictionary()
            {
                Source = new Uri(ResourceUriPath,
                    UriKind.RelativeOrAbsolute)
            };
        }

        public static ResourcesHelper Instance { get { return _Instance; } }


        public ResourceDictionary Resource { get { return _Resources; } }
    }
}
