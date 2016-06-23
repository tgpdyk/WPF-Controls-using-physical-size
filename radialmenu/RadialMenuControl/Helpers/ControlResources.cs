using System;
using System.Windows;

namespace RadialMenuControl.Helpers
{
    public sealed class ControlResources
    {
        static ResourceDictionary _resources;
        const string ResourceUriPath = "/RadialMenuControl;component/Resources/RadialMenu.xaml";

        private ControlResources()
        {
            _resources = new ResourceDictionary()
            {
                Source = new Uri(ResourceUriPath,
                    UriKind.RelativeOrAbsolute)
            };
        }
        public static ControlResources Instance { get; } = new ControlResources();
        public ResourceDictionary Resource => _resources;
    }
}
