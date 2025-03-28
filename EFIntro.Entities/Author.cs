using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFIntro.Entities
{
    [Table("Authors")]
    [Index(nameof(Author.FirstName),nameof(Author.LastName),Name ="IX_Authors_FirstName_LastName",IsUnique =true)]
    public class Author 
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="The field {0} is required")]
        [StringLength(50,ErrorMessage="The field {0} must be between {2} and {1} characteres",MinimumLength =3)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(50, ErrorMessage = "The field {0} must be between {2} and {1} characteres", MinimumLength = 3)]
        public string LastName { get; set; } = null!;
        public override string ToString()
        {
            return $"{LastName.ToUpper()}, {FirstName}";
        }
    }
}
