using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Models
{
    [PrimaryKey("User_Id","Role_Id")]   
    public class UserRole
    {
        [ForeignKey("user")]
        public int User_Id { get; set; }
        [ForeignKey("role")]
        public int Role_Id { get; set; }
        public User user { get; set; }
        public Role role { get; set; }
    }
}
