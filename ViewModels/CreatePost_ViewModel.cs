﻿using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class CreatePost_ViewModel
    {
        [Required(ErrorMessage = "Enter the text for the post")]
        [MaxLength(2500, ErrorMessage = "Max length of post is limited by 2,500")]
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? LinkedProjectId { get; set; }
        public int UserId { get; set; }
    }
}
