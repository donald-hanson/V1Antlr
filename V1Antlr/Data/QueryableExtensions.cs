using System.Linq;

namespace V1Antlr.Data
{
    public static class QueryableExtensions
    {
        public static QueryResult ApplyQuery(this IQueryable queryable, Query query)
        {
            var service = new QueryExecuteService();
            return service.Execute(queryable, query);
        }
    }
}
