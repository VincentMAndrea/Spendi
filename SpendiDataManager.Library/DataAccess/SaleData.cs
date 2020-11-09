using SpendiDataManager.Library.Internal.DataAccess;
using SpendiDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendiDataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo, string userId)
        {
            //Fill in available sale details to save to the database
            List<SaleDetailDBModel> saleDetails = new List<SaleDetailDBModel>();
            ProductData products = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate() / 100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                };

                //Get product information
                var productInfo = products.GetProductById(detail.ProductId);

                if (productInfo == null)
                {
                    throw new Exception($"The product { item.ProductId } could not be found in the database.");
                }

                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);

                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }

                saleDetails.Add(detail);
            }

            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = saleDetails.Sum(x => x.PurchasePrice),
                Tax = saleDetails.Sum(x => x.Tax),
                CashierId = userId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData("dbo.spSaleInsert", sale, "SpendiData");

            //Get Id from SaleModel
            sale.Id = sql.LoadData<int, dynamic>("spSaleLookup", new { CashierId = sale.CashierId, SaleDate = sale.SaleDate }, "SpendiData").FirstOrDefault();

            foreach (var item in saleDetails)
            {
                item.SaleId = sale.Id;
                sql.SaveData("dbo.spSaleDetailInsert", item, "SpendiData");
            }
        }
    }
}
