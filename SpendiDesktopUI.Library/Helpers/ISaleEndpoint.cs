using SpendiDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace SpendiDesktopUI.Library.Helpers
{
    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel sale);
    }
}