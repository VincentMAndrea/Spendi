using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendiDesktopUI.Models
{
    public class CartItemUIModel : INotifyPropertyChanged
    {
        private int _quantityInCart;

        public int QuantityInCart
        {
            get
            {
                return _quantityInCart;
            }
            set
            {
                _quantityInCart = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QuantityInCart)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayInfo)));
            }
        }

        public ProductUIModel Product { get; set; }
        public string DisplayInfo
        {
            get
            {
                return $"{Product.ProductName} ({QuantityInCart})";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
