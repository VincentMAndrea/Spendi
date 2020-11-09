using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendiDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
        { //TODO Move to API
            string rate = ConfigurationManager.AppSettings["taxRate"];

            bool isValid = Decimal.TryParse(rate, out decimal output);

            if (!isValid)
            {
                throw new ConfigurationErrorsException("The tax rate is not configured properly.");
            }

            return output;
        }
    }
}
