using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBusMetro
{
    public class TileGroup
    {
        public string Title { get; set; }
        public ObservableCollection<IItem> Items { get { return _Items; } set { _Items = value; } }
        private ObservableCollection<IItem> _Items;

        public TileGroup()
        {
            _Items = new ObservableCollection<IItem>();
        }
    }
}
