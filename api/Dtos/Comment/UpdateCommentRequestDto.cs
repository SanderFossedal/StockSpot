using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters long")]        
        [MaxLength(280, ErrorMessage = "Title cannot be more than 280 characters long")]
        public string Title { get; set; } = string.Empty;

        
        [Required]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters long")]        
        [MaxLength(280, ErrorMessage = "Title cannot be more than 280 characters long")]
        public string Content { get; set; } = string.Empty;
    }
}