using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Data;
using Sitecore.ContentSearch.Linq;

namespace DevsWeb.Features.Search.Searches
{
    public class CoordinateSearch
    {
        public static IEnumerable<CoordinateSearchResult> Search(Coordinate point)
        {
            var index = ContentSearchManager.GetIndex("devsweb_master");
            var query = index.CreateSearchContext().GetQueryable<CoordinateSearchResult>();
            var results = query.OrderByDistance(x => x.Locations, point);

            return results.ToList();
        }
    }
}
