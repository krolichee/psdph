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
        public static void SetOwnerWithTopmost(this Window window)
        {
            var topWindow = Get();
            if (topWindow != null)
                window.Owner = topWindow;
        }
        public static bool IsWindowBlockedByDialog(Window window)
        {
            // Если Dispatcher окна не обрабатывает новые задачи (заблокирован ShowDialog)
            return !window.Dispatcher.CheckAccess() ||
                   window.Dispatcher.HasShutdownStarted ||
                   window.Dispatcher.Invoke(() => false, System.Windows.Threading.DispatcherPriority.Send);
        }
        static Window[] NotTop()
        {
            try
            {
                return Application.Current.Windows.OfType<Window>().Where(IsWindowBlockedByDialog).ToArray();
            }
            catch
            {
                return new Window[0];
            }
        }
        public static void HideNotTop()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (IsWindowBlockedByDialog(window))
                    {
                        window.Hide();
                    }
                }
            });
            //foreach (var w in NotTop())
            //{
            //    w.Hide();
            //}
            //Get()?.Show();
        }
    }
}
