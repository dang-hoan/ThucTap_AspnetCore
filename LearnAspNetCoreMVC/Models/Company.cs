using System;
using System.Collections.Generic;

namespace LearnAspNetCoreMVC.Models;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public DateTime? DeleteDate { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
