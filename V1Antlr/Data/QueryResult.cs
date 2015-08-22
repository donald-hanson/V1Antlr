using System.Collections.Generic;

namespace V1Antlr.Data
{
    public class QueryResult
    {
        public readonly Query Query;
        public readonly IEnumerable<Asset> Assets;
        public readonly long TotalAvailable;

        public QueryResult(Query query, IEnumerable<Asset> assets, long totalAvailable)
        {
            Query = query;
            Assets = assets;
            TotalAvailable = totalAvailable;
        }
    }
}