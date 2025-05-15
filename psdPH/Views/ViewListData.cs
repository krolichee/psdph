using psdPH.Logic.Compositions;
using psdPH.Views.SimpleView.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace psdPH.Views
{
    public interface Restorable
    {
        void Restore();
    }
    [Serializable]
    public abstract class ViewListData<T> where T:Restorable
    {
        public Blob RootBlob;
        public ObservableCollection<T> Variants = new ObservableCollection<T>();

        protected void Initialize(Blob root)
        {
            RootBlob = root;
        }
        public virtual void Remove(T item)
        {
            Variants.Remove(item);
        }
        public abstract void New();
        internal virtual void Restore()
        {
            RootBlob.Restore();
            foreach (var item in Variants)
            {
                item.Restore();
            }
        }
    }
}
