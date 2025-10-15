using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.DataAccess.CommonModels
{
    public static class Extensions
    {
        public static List<Dictionary<string, object>> ToListOfDictionary(this DataTable dt)
        {
            var result = dt.AsEnumerable().Select(
            // ...then iterate through the columns...
            row => dt.Columns.Cast<DataColumn>().ToDictionary(
            // ...and find the key value pairs for the dictionary
            column => column.ColumnName,    // Key
        column => row[column]  // Value
    )).ToList();
            return result;

        }
    }
}
