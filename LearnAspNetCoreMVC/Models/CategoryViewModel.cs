using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LearnAspNetCoreMVC.Models
{
    public class CategoryViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public string Name { get; set; }
        [Range(1, 100, ErrorMessage = "Display Order must be between 1 and 100 only!!")]
        public int? DisplayOrder { get; set; }
    }
}
