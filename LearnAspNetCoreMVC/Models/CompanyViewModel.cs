using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LearnAspNetCoreMVC.Models
{
    public class CompanyViewModel
    {
        public IEnumerable<Company> Companys { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
