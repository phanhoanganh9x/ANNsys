using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ.Models
{
    public class PaginationMetadataModel
    {
        public int totalCount { get; set; } = 0;
        public int pageSize { get; set; } = 10;
        public int currentPage { get; set; } = 1;
        public int totalPages { get; set; } = 0;
    }
}