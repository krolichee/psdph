using psdPH.Lets;
using psdPH.Lets.Core;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes.UI
{
    public class LetBarViewModel
    {
        public LetView LetView { get; set; }
        public bool IsChainable() { throw new NotImplementedException(); }
        public RelayCommand DropLinkOn;

        public LetBarViewModel(LetView letView)
        {
            LetView = letView;
        }
        [Obsolete]
        public LetBarViewModel()
        {
        }
    }

}
