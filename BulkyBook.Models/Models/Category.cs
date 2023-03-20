using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class Category
    {

        /*
         * get {
             return this.name; }

           set {
             this.name = value;
               }
         */

        /* prop -> duble tab will create property
         */
        /*
         Set extra config on the columns as if the ID is unique and the Name is required 
         */
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public DateTime CreatedDateTimr { get; set; } = DateTime.Now;
        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage ="Display Order Value Must Be Between 1 - 100")]
        public int DisplayOrder { get; set; }

    }
}
