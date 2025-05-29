using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace psdPH.Utils
{
    public static class TopmostWindow
    {
        public static void CenterByTopmostOrScreen(this Window window)
        {
            var topWindow = Get();
            if (topWindow != null)
            {
                window.Owner = topWindow;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }

        public static Window Get()
        {
            try
            {
                return Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            }
            catch {
                return null; 
            }

        }
    }
}
