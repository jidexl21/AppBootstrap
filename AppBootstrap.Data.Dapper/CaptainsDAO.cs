using AppBootstrap.Core;
using Dapper;
using OF.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBootstrap.Data.Dapper
{
    public class CaptainsDAO : BaseDapperDAO<Captain>, ICaptainsDAO
    {
        public CaptainsDAO(IDataContext dataContext) : base(dataContext)
        {
            tableName = "Captains";
        }

        public async Task<Captain> GetLastCaptain()
        {
            string sql = $"SELECT * FROM {this.tableName} ORDER BY ID DESC LIMIT 1";
            var connection = this.dataContext.Connection;
            var result = await connection.QueryFirstOrDefaultAsync<Captain>(sql, new { }, this.dataContext.Transaction);
            return result;
        }

        public async Task<List<Captain>> GetElectorate(long locationId)
        {
            string storedProcedureName = $"get_captains";

            var connection = this.dataContext.Connection;
            var result = await connection.QueryAsync<Captain>(storedProcedureName, new { eid = locationId }, this.dataContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);

            return result.AsList();
        }

        
    }
}
