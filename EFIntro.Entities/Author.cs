using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFIntro.Entities
{
    public class Author 
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
        public ICollection<Book>? Books { get; set; }
        public override string ToString()
        {
            return $"{LastName.ToUpper()}, {FirstName}";
        }
    }
}
