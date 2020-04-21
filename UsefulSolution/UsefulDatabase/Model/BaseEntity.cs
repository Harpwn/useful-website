using System;
using System.ComponentModel.DataAnnotations;

namespace UsefulDatabase.Model
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            CreatedDate = DateTime.UtcNow;
            LastModified = DateTime.UtcNow;
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastModified { get; set; }
    }
}
