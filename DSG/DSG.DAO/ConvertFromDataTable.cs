using DSG.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.DAO
{
    public class ConvertFromDataTable
    {
        public List<DominionExpansion> ConvertExpansionFromDataTable(DataTable dataTable)
        {
            List<DominionExpansion> expansions = new List<DominionExpansion>();

            int tableCount = dataTable.Rows.Count;

            for(int i = 0; i < tableCount; i++)
            {
                expansions.Add(new DominionExpansion
                {
                    Name = (string)dataTable.Rows[i]["Name"]
                });
            }

            return expansions;
        }
    }
}
