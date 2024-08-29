using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;

namespace api.Dtos.Stock
{
    //DTOs are used to shape the data that is sent to the client
    //Usefull when you want to hide some properties from the client, like passwords
    public class StockDto
    {
         public int Id { get; set; }

        
        public string Symbol { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty; 

        public decimal Purchase { get; set; }

        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = string.Empty;

        public long MarketCap { get; set; }

        
        public List<CommentDto> Comments { get; set; }
    }
}