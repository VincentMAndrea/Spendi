using SpendiDataManager.Library.Internal.DataAccess;
using SpendiDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendiDataManager.Library.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string Id)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var p = new { Id };

            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "SpendiData");

            return output;
        }
    }
}
