using SpendiDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendiDesktopUI.Library.Helpers
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}