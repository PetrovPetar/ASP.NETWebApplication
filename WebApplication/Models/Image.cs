using System.ComponentModel.DataAnnotations;


namespace WebApplication.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}