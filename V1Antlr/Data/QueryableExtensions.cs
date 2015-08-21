using System.Collections.Generic;
using System.Linq;

namespace V1Antlr.Data
{
    public static class QueryableExtensions
    {
        public static QueryResult ApplyQuery(this IQueryable queryable, Query query)
        {
            return new QueryResult(query, Enumerable.Empty<Asset>(), 0);
        }
    }

    public class QueryResult
    {
        public readonly Query Query;
        public readonly IEnumerable<Asset> Assets;
        public readonly ulong TotalAvailable;

        public QueryResult(Query query, IEnumerable<Asset> assets, ulong totalAvailable)
        {
            Query = query;
            Assets = assets;
            TotalAvailable = totalAvailable;
        }
    }

    public class Asset
    {
        
    }
}
