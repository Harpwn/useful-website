using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsefulDatabase.Model.Images
{
    [Table("Images")]
    public class Image : BaseEntity
    {
        [Required]
        [MaxLength(5000000)]
        public virtual byte[] File { get; set; }

    }
}
