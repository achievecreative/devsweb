using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Data;
using Sitecore.ContentSearch.SearchTypes;

namespace DevsWeb.Features.Search.Searches
{
    public class CoordinateSearchResult : SearchResultItem
    {
        [IndexField("locations")]
        public List<Coordinate> Locations { get; set; }
    }
}
