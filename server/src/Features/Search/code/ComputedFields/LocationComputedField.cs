/*
 * For testing purpose
 * Assume there is a Location item which include a list of lan & long.
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Data;

namespace DevsWeb.Features.Search.ComputedFields
{
    public class LocationComputedField : MultipleCoordinatesField
    {
        protected override List<Coordinate> GetCoordinates(SitecoreIndexableItem indexableItem)
        {
            var item = indexableItem?.Item;
            if (item == null || item.TemplateName != "Page")
            {
                return null;
                ;
            }

            var locationItem = item.Axes.GetDescendants().FirstOrDefault(x => x.TemplateName == "Location");
            if (locationItem == null)
            {
                return null;
            }

            return locationItem["latitudeAndlongitude"]?.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x =>
                {
                    var array = x.Split(',');

                    return new Coordinate(double.Parse(array[0]), double.Parse(array[1]));

                }).ToList();
        }
    }
}
