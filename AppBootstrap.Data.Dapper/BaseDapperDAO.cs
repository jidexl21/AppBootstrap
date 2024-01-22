using AppBootstrap.Core;
using Dapper;
using OF.Infrastructure.Data;

namespace AppBootstrap.Data.Dapper
{
    public abstract class BaseDapperDAO<T> : IBaseDAO<T> where T : BaseEntity
    {
        protected string tableName; //Set table name from descendant
        protected readonly IDataContext dataContext;
        protected string[] IgnoreFields;
        protected Dialect SQLDialect;
        protected string Last_Insert_ID { get => last_insert_id(); }
        protected char ColOpenDelimiter { get => col_open(); }
        protected char ColCloseDelimiter { get => col_close(); }


        protected string _(string toDelimit)
        {
            return $"{ColOpenDelimiter}{toDelimit}{ColCloseDelimiter}";
        }
        public BaseDapperDAO(IDataContext dataContext) //string parameters not compatible with simple injector
        {
            this.tableName = string.Empty;
            this.dataContext = dataContext;
            this.IgnoreFields = new string[] { };
            this.SQLDialect = dataContext.SqlDialect;
        }

        private string last_insert_id()
        {
            string str = "SCOPE_IDENTITY()";
            switch (SQLDialect)
            {
                case Dialect.MSSQL:

                    break;
                case Dialect.MySql:
                    str = "LAST_INSERT_ID()";
                    break;
                default:
                    //str = "LAST_INSERT_ID()";
                    break;
            }
            return str;
        }
        private char col_open()
        {
            char str = '[';
            switch (SQLDialect)
            {
                case Dialect.MSSQL:

                    break;
                case Dialect.MySql:
                    str = '`';
                    break;
                default:
                    //str = "LAST_INSERT_ID()";
                    break;
            }
            return str;
        }
        private char col_close()
        {
            char str = ']';
            switch (SQLDialect)
            {
                case Dialect.MSSQL:

                    break;
                case Dialect.MySql:
                    str = '`';
                    break;
                default:

                    break;
            }
            return str;
        }

        public async Task<bool> Exists(long Id)
        {
            string sql = $"SELECT  count(0) cnt FROM {this.tableName} where ID = @ID";
            var connection = this.dataContext.Connection;
            var result = await connection.QueryFirstOrDefaultAsync<int>(sql, new { ID = Id }, this.dataContext.Transaction);
            return (result > 0);
        }

        public virtual async Task<(int, List<T>)> FilterPagedAsync(Dictionary<string, (string, string)> keyValuePairs, int pageNum, int maxSize)
        {

            string conditions = string.Empty;
            var queryParameters = keyValuePairs.ToList();


            if (queryParameters.Count > 0)
                conditions = " WHERE " + string.Join(" AND ", queryParameters.Select(x => $" {x.Key} {x.Value.Item1} " + ((x.Value.Item1 == "=") ? $"'{x.Value.Item2}'" : $"'%{x.Value.Item2}%'")));


            string sql = $"SELECT * FROM {this.tableName}" +
                $"{conditions} " +
                $"ORDER BY Id desc " +
                $"OFFSET {(pageNum - 1) * maxSize} ROWS FETCH NEXT {maxSize} ROWS ONLY; " +
                $"SELECT COUNT(0) from {this.tableName} {conditions}";



            var connection = this.dataContext.Connection;
            var response = await connection.QueryMultipleAsync(sql, new { }, dataContext.Transaction);



            var result = response.Read<T>().ToList();
            int total = response.Read<int>().First();
            return (total, result);
        }

        public virtual async Task<T> RetrieveByIdAsync(long Id)
        {
            string sql = $"SELECT * FROM {this.tableName} where ID = @ID";
            var connection = this.dataContext.Connection;
            var result = await connection.QueryFirstOrDefaultAsync<T>(sql, new { ID = Id }, this.dataContext.Transaction);
            if (result != null) await PopluateInnerObjects(result);
            return result;
        }

        internal virtual async Task<bool> PopluateInnerObjects(T t)
        {
            return await Task.FromResult(true);
        }

        public virtual async Task<List<T>> RetrieveAllAsync(string UserCode)
        {
            string sql = $"SELECT * FROM {this.tableName} where InstitutionCode = @InstitutionCode";

            var connection = this.dataContext.Connection;
            List<T> result = (List<T>)await connection.QueryAsync<T>(sql, new { UserCode }, this.dataContext.Transaction);
            await result.ForEachAsync(i => PopluateInnerObjects(i));
            return result;
        }

        public virtual async Task<List<T>> RetrieveAllAsync(long InstitutionCode)
        {
            string sql = $"SELECT * FROM {this.tableName} where InstitutionCode = @InstitutionCode";

            var connection = this.dataContext.Connection;
            List<T> result = (List<T>)await connection.QueryAsync<T>(sql, new { InstitutionCode = InstitutionCode }, this.dataContext.Transaction);
            await result.ForEachAsync(i => PopluateInnerObjects(i));
            return result;
        }

        public virtual async Task<List<T>> RetrieveAllAsync(long InstitutionCode, int pageNumber, int pageSize)
        {
            string sql = $"SELECT * FROM {this.tableName} where InstitutionCode = @InstitutionCode " +
                $"ORDER BY ID" +
                $" OFFSET {(pageNumber - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            var connection = this.dataContext.Connection;
            List<T> result = (List<T>)await connection.QueryAsync<T>(sql, new { InstitutionCode = InstitutionCode }, this.dataContext.Transaction);
            await result.ForEachAsync(i => PopluateInnerObjects(i));
            return result;
        }

        public virtual async Task<List<T>> RetrieveAllAsync(int pageNumber, int pageSize)
        {
            string sql = $"SELECT * FROM {this.tableName} " +
                $" ORDER BY ID" +
                $" OFFSET {(pageNumber - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            var connection = this.dataContext.Connection;
            List<T> result = (List<T>)await connection.QueryAsync<T>(sql, new { }, this.dataContext.Transaction);
            await result.ForEachAsync(i => PopluateInnerObjects(i));
            return result;
        }

        public async Task<T> RetrieveByIdAsync(long InstitutionCode, long Id)
        {
            string sql = $"SELECT * FROM {this.tableName} where InstitutionCode = @InstitutionCode and Id = @ID";

            var connection = this.dataContext.Connection;
            var result = await connection.QueryFirstOrDefaultAsync<T>(sql, new { InstitutionCode = InstitutionCode, ID = Id }, this.dataContext.Transaction);
            if (result != null) await PopluateInnerObjects(result);
            return result;
        }

        public async Task<List<T>> RetrieveByIdsAsync(IEnumerable<long> Ids)
        {
            string sql = $"SELECT * FROM {this.tableName} where ID in = '{string.Join(",", Ids)}'";

            var connection = this.dataContext.Connection;
            var result = await connection.QueryAsync<T>(sql, new { }, this.dataContext.Transaction);
            //if (result != null) await PopluateInnerObjects(result);
            return result.ToList();
        }

        public async Task<T> DisableAsync(T item)
        {
            string sql = $"Update {this.tableName}  SET {ColOpenDelimiter}Status{ColCloseDelimiter} = 'Inactive' WHERE ID = @ID";
            var connection = this.dataContext.Connection;
            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                item.Id
            }, this.dataContext.Transaction);

            return item;
        }

        public async Task<T> EnableAsync(T item)
        {
            string sql = $"Update {this.tableName}  SET {ColOpenDelimiter}Status{ColCloseDelimiter} = 'Active' WHERE ID = @ID";
            var connection = this.dataContext.Connection;
            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                item.Id
            }, this.dataContext.Transaction);

            return item;
        }

        public async Task DeleteAsync(List<long> ids)
        {
            string sql = $"Delete from {this.tableName}  where ID in ({string.Join(",", ids)})";
            var connection = this.dataContext.Connection;
            await connection.ExecuteAsync(sql, new { }, this.dataContext.Transaction);
        }

        public virtual async Task<List<T>> RetrieveAllAsync()
        {
            CheckType();
            string sql = $"SELECT * FROM {this.tableName} ";

            var connection = this.dataContext.Connection;
            List<T> result = (List<T>)await connection.QueryAsync<T>(sql, new { }, this.dataContext.Transaction);
            await result.ForEachAsync(i => PopluateInnerObjects(i));
            return result;
        }

        public virtual async Task<T> SaveAsync(T item)
        {
            CheckType();
            var fields = item.GetType().GetProperties()
                .Where(x => x.GetValue(item) != null)
                .Where(x => x.Name.ToLower() != "id")
                .Where(x => !this.IgnoreFields.Contains(x.Name))
                .Where(x => !x.GetValue(item).IsCollection())
                .ToDictionary(x => x.Name, x => x.GetValueX(item));
            var connection = this.dataContext.Connection;
            string names = string.Join(",", fields.Keys.Select(x => $"{ColOpenDelimiter}{x}{ColCloseDelimiter}"));
            string placeholders = string.Join(",", fields.Keys.Select(x => $"@{x}"));
            string sql = $@"INSERT INTO {this.tableName} ({names}) VALUES ({placeholders});
                SELECT {Last_Insert_ID}";
            List<long> result = (List<long>)await connection.QueryAsync<long>(sql, fields, this.dataContext.Transaction);
            item.Id = result.FirstOrDefault();
            return item;
        }


        public virtual async Task<IEnumerable<T>> SaveAsync(IEnumerable<T> item)
        {
            var tasks = item.Select(x => SaveAsync(x));
            var result = await Task.WhenAll(tasks);
            return result.ToList();
        }

        public virtual async Task<T> UpdateAsync(T item)
        {
            return await UpdateAsync(item, new string[] { });
        }

        public virtual async Task<T> UpdateAsync(T item, IEnumerable<string> skip)
        {
            CheckType();
            string[] ignore = IgnoreFields.Concat(skip).Select(x => x.ToLower()).ToArray();
            var args = new DynamicParameters(item);
            var connection = this.dataContext.Connection;
            var fields = item.GetType().GetProperties()
                    .Where(x => x.GetValue(item) != null)
                    .Where(x => x.Name.ToLower() != "id")
                    .Where(x => !ignore.Contains(x.Name.ToLower()))
                    .Where(x => !x.GetValue(item).IsCollection())
                    .ToDictionary(x => x.Name, x => x.GetValueX(item));
            var flds = fields.Select(x => $"{_(x.Key)} = '{x.Value}'");
            string sql = $@"UPDATE {this.tableName}  SET {string.Join(",", flds)} WHERE ID = {item.Id}";
            int result = await connection.ExecuteAsync(sql, args, this.dataContext.Transaction);
            return (item.Id > 0) ? item : null;
        }
        public virtual async Task<T> RemoveAsync(T item)
        {
            string sql = $"Delete from {this.tableName}  where ID = @ID ";
            var connection = this.dataContext.Connection;
            int removeCount = await connection.ExecuteAsync(sql, new { item.Id }, this.dataContext.Transaction);
            return (removeCount > 0) ? item : null;
        }

        public async Task<int> RemoveAsync(IEnumerable<long> ids)
        {
            string sql = $"Delete from {this.tableName}  where ID in ( {string.Join(",", ids)} )";
            var connection = this.dataContext.Connection;
            return await connection.ExecuteAsync(sql, new { }, this.dataContext.Transaction);
        }
        public List<T> FindWithPaging(Dictionary<string, object> propertyValuePairs, int pageIndex, int maxSize, out int total)
        {
            throw new NotImplementedException();
        }

        public Task<(List<T>, int)> FindWithPagingAsync(Dictionary<string, object> propertyValuePairs, int pageNum, int maxSize)
        {
            throw new NotImplementedException();
        }


        private void CheckType()
        {

            if (string.IsNullOrEmpty(this.tableName)) { throw new TypeInitializationException(this.GetType().FullName, new Exception("tableName not set in constructor")); }
        }
    }

}