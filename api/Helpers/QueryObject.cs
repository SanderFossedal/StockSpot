using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{

/// <summary>
/// Represents a query object used to filter or search stocks. 
/// The class contains optional properties, such as 'Symbol' and 'CompanyName',
/// that can be set to specify search criteria for stocks.
/// </summary>
    public class QueryObject
    {
        public string? Symbol { get; set; } = null;

        public string? CompanyName { get; set; } = null;

        public string? SortBy { get; set; } = null;

        public bool IsDecending { get; set; } = false;


        //Pagination, used to restrict the number of stocks returned to frontend
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}