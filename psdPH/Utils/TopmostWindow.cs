using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace psdPH.Utils
{
    public static class TopmostWindow
    {
        public static Window Get()
        {
            return Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
        }
    }
}
