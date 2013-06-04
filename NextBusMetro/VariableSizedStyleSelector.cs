using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NextBusMetro
{
    public class VariableSizedStyleSelector : StyleSelector
    {
        public Style NormalStyle { get; set; }
        public Style DoubleHeightStyle { get; set; }

        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            if(this.NormalStyle==null||this.DoubleHeightStyle==null)
                return base.SelectStyleCore(item, container);

            if (item is NormalItem)
                return NormalStyle;
            if (item is SpecialItem)
                return DoubleHeightStyle;

            return base.SelectStyleCore(item, container);
        }
    }
}
