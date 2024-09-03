using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("Comments")]
    public class Comment
    {

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime createdOn { get; set; } = DateTime.Now;

        public int? StockId { get; set; }
        //navigation property () .net core will automatically create a foreign key in the database

        public Stock? Stock { get; set; }
    }
}