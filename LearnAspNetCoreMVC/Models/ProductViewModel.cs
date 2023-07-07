using System.ComponentModel.DataAnnotations;

namespace LearnAspNetCoreMVC.Models
{
    public class ProductViewModel
    {
        public IEnumerable<ProductView> Products { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; }
        [Range(1, 100, ErrorMessage = "Display order must be between 1 and 100 only!!")]
        public int? DisplayOrder { get; set; }
        public int? CompanyId { get; set; }
    }
}
