using NextBusMetro.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NextBusMetro
{
    public class VariableSizedTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NormalTemplate { get; set; }
        public DataTemplate DoubleTemplate { get; set; }
        public DataTemplate TileTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if(this.NormalTemplate==null || this.DoubleTemplate==null)
                return base.SelectTemplateCore(item, container);
            if (item is NormalItem)
                return NormalTemplate;
            if (item is SpecialItem)
                return DoubleTemplate;
            if (item is TileItem)
                return TileTemplate;
            return base.SelectTemplateCore(item, container);
        }
    }
}
