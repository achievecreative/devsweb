using System.Collections.Generic;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.ContentSearch.Data;

namespace DevsWeb.Features.Search.ComputedFields
{
    public abstract class MultipleCoordinatesField : IComputedIndexField
    {
        private const string ReturnTypeValue = "coordinateCollection";

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = indexable as SitecoreIndexableItem;
            if (indexableItem == null)
            {
                return null;
            }

            return GetCoordinates(indexableItem);
        }

        public string FieldName { get; set; }

        /// <summary>
        /// Return type must be coordinateCollection
        /// </summary>
        public string ReturnType
        {
            get => ReturnTypeValue;
            set { }
        }

        protected abstract List<Coordinate> GetCoordinates(SitecoreIndexableItem indexableItem);
    }
}
