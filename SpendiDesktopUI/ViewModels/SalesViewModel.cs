using AutoMapper;
using Caliburn.Micro;
using SpendiDesktopUI.Library.Helpers;
using SpendiDesktopUI.Library.Models;
using SpendiDesktopUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendiDesktopUI.ViewModels
{
	public class SalesViewModel : Screen
	{
		IProductEndpoint _productEndpoint;
		IConfigHelper _configHelper;
		ISaleEndpoint _saleEndpoint;
		IMapper _mapper;

		public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, ISaleEndpoint saleEndpoint, IMapper mapper)
		{
			_productEndpoint = productEndpoint;
			_configHelper = configHelper;
			_saleEndpoint = saleEndpoint;
			_mapper = mapper;
		}


		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			await LoadProducts();
		}

		public async Task LoadProducts()
		{
			var productList = await _productEndpoint.GetAll();
			var products = _mapper.Map<List<ProductUIModel>>(productList);
			Products = new BindingList<ProductUIModel>(products);
		}

		private async Task ResetSalesViewModel()
		{
			Cart = new BindingList<CartItemUIModel>();
			await LoadProducts();

			NotifyOfPropertyChange(() => Subtotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckout);
		}

		private BindingList<ProductUIModel> _products;

		public BindingList<ProductUIModel> Products
		{
			get { return _products; }
			set
			{
				_products = value;
				NotifyOfPropertyChange(() => Products);
			}
		}

		private ProductUIModel _selectedProduct;

		public ProductUIModel SelectedProduct
		{
			get { return _selectedProduct; }
			set
			{
				_selectedProduct = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);
				NotifyOfPropertyChange(() => Description);
			}
		}

		private CartItemUIModel _selectedCartItem;

		public CartItemUIModel SelectedCartItem
		{
			get { return _selectedCartItem; }
			set
			{
				_selectedCartItem = value;
				NotifyOfPropertyChange(() => SelectedCartItem);
				NotifyOfPropertyChange(() => CanRemoveFromCart);
			}
		}


		private BindingList<CartItemUIModel> _cart = new BindingList<CartItemUIModel>();

		public BindingList<CartItemUIModel> Cart
		{
			get { return _cart; }
			set
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}

		private int _itemQuantity = 1;

		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set 
			{
				_itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		public string Subtotal
		{
			get
			{
				return CalculateSubTotal().ToString("C");
			}
		}

		private decimal CalculateSubTotal()
		{
			decimal subTotal = 0;

			foreach (var item in Cart)
			{
				subTotal += item.Product.RetailPrice * item.QuantityInCart;
			}
			return subTotal;
		}

		public string Description
		{
			get
			{
				if (SelectedProduct == null)
				{
					return "";
				}
				return SelectedProduct.Description;
			}
		}

		public string Tax
		{
			get
			{
				return CalculateTax().ToString("C");
			}
		}

		private decimal CalculateTax()
		{
			decimal taxAmount = 0;
			decimal taxRate = _configHelper.GetTaxRate() / 100;

			taxAmount = Cart.Where(x => x.Product.IsTaxable).Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

			return taxAmount;
		}

		public string Total
		{
			get
			{
				return (CalculateSubTotal() + CalculateTax()).ToString("C");
			}
		}

		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;

				if (SelectedCartItem != null)
				{
					output = true;
				}

				return output;
			}
		}

		public void RemoveFromCart()
		{
			SelectedCartItem.Product.QuantityInStock += 1;

			if (SelectedCartItem.QuantityInCart > 1)
			{
				SelectedCartItem.QuantityInCart -= 1;
			}
			else
			{
				Cart.Remove(SelectedCartItem);
			}
			NotifyOfPropertyChange(() => Subtotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckout);
			NotifyOfPropertyChange(() => CanAddToCart);
		}

		public bool CanAddToCart
		{
			get
			{
				bool output = false;

				if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
				{
					output = true;
				}
				

				return output;
			}
		}

		public void AddToCart()
		{
			CartItemUIModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
			if (existingItem != null)
			{
				existingItem.QuantityInCart += ItemQuantity;
			}
			else
			{
				CartItemUIModel item = new CartItemUIModel
				{
					Product = SelectedProduct,
					QuantityInCart = ItemQuantity
				};
				Cart.Add(item);
			}

			SelectedProduct.QuantityInStock -= ItemQuantity;
			ItemQuantity = 1;
			NotifyOfPropertyChange(() => Subtotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckout);
		}

		public bool CanCheckout
		{
			get
			{
				bool output = false;

				if (Cart.Count > 0)
				{
					output = true;
				}

				return output;
			}
		}

		public async Task Checkout()
		{
			SaleModel sale = new SaleModel();

			foreach (var item in Cart)
			{
				sale.SaleDetails.Add(new SaleDetailModel
				{
					ProductId = item.Product.Id,
					Quantity = item.QuantityInCart
				});
			}
			await _saleEndpoint.PostSale(sale);
			await ResetSalesViewModel();
		}

	}
}
