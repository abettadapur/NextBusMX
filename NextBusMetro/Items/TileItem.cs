using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace NextBusMetro.Items
{
    public class TileItem : IItem
    {
        
        private ObservableCollection<TilePiece> _Items;
        public ObservableCollection<TilePiece> Items { get { return _Items; } set { _Items = value; } }
        public SolidColorBrush Color { get; set; }
        public object AssignedObject { get; set; }
        public TileItem()
        {
            _Items = new ObservableCollection<TilePiece>();
        }

    }
   
   
    public class TilePiece
    {
        public string Title{get;set;}
        public string Subtitle{get;set;}
        public string Details{get;set;}
    }
}
