using System;
using System.Collections.Generic;

namespace LearnAspNetCoreMVC.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int DisplayOrder { get; set; }

    public int CompanyId { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public DateTime? DeleteDate { get; set; }

    public bool IsDelete { get; set; }

    public virtual Company Company { get; set; } = null!;
}
