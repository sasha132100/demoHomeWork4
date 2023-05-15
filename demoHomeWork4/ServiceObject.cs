using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace demoHomeWork4
{
    class ServiceObject
    {
        public string PanelColor { get; set; }
        public string Name { get; set; }
        public SchoolService Service { get; set; }
        public string DiscountText { get; set; }
        public string PriceText { get; set; }
        public Visibility OldPriceVisibility { get; set; }
        public Visibility ButtonsVisibility { get; set; }
        public string MainImage { get; set; }
    }
}
