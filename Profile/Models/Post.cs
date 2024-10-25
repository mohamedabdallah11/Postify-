using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(50, ErrorMessage = "Category cannot be longer than 50 characters.")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Body is required.")]
        public string Body { get; set; }

        public int Likes { get; set; }

        [ForeignKey("user")]
        public int User_Id { get; set; }

        [Display(Name = "User")]
        public User? user { get; set; }
    }
}
