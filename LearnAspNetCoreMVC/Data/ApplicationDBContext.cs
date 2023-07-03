using System;
using System.Collections.Generic;
using LearnAspNetCoreMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnAspNetCoreMVC.Data;

public partial class ApplicationDBContext : DbContext
{
    public ApplicationDBContext()
    {
    }

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.CompanyId, "IX_Products_CompanyID");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

            entity.HasOne(d => d.Company).WithMany(p => p.Products).HasForeignKey(d => d.CompanyId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
