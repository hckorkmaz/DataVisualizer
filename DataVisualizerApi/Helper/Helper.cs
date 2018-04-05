using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DataVisualizerApi.Helper
{
    public static class Helper
    {
        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                for(int i = 0; i < table.Rows.Count; i++)
                {
                    var row = table.Rows[i];
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static DataTable GetNullFilledDataTableForXML(DataTable dtSource)
        {
            // Create a target table with same structure as source and fields as strings
            // We can change the column datatype as long as there is no data loaded
            DataTable dtTarget = dtSource.Clone();
            foreach (DataColumn col in dtTarget.Columns)
                col.DataType = typeof(string);

            // Start importing the source into target by ItemArray copying which 
            // is found to be reasonably fast for null operations. VS 2015 is reporting
            // 500-525 milliseconds for loading 100,000 records x 10 columns 
            // after null conversion in every cell 
            // The speed may be usable in many circumstances.
            // Machine config: i5 2nd Gen, 8 GB RAM, Windows 7 64bit, VS 2015 Update 1
            int colCountInTarget = dtTarget.Columns.Count;
            foreach (DataRow sourceRow in dtSource.Rows)
            {
                // Get a new row loaded with data from source row
                DataRow targetRow = dtTarget.NewRow();
                targetRow.ItemArray = sourceRow.ItemArray;

                // Update DBNull.Values to empty string in the new (target) row
                // We can safely assign empty string since the target table columns
                // are all of string type
                for (int ctr = 0; ctr < colCountInTarget; ctr++)
                    if (targetRow[ctr] == DBNull.Value)
                        targetRow[ctr] = String.Empty;

                // Now add the null filled row to target datatable
                dtTarget.Rows.Add(targetRow);
            }

            // Return the target datatable
            return dtTarget;
        }

        public static string GetModelStateErrors(this ModelStateDictionary modelState)
        {
            IEnumerable<KeyValuePair<string, string[]>> errors = modelState.IsValid
                ? null
                : modelState
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray())
                    .Where(m => m.Value.Any());

            string output = "{";

            if (errors != null)
            {
                foreach (KeyValuePair<string, string[]> kvp in errors)
                {
                    output += "\"" + kvp.Key.Replace(".", "") + "\":\"" + String.Join(", ", kvp.Value) + "\",";
                }
            }
            output = output.TrimEnd(',');
            output += "}";
            return output;
        }
    }
}