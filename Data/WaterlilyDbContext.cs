using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WaterlilyLabs.Models.Data;

namespace WaterlilyLabs.Data;

public partial class WaterlilyDbContext : DbContext
{
    public WaterlilyDbContext()
    {
    }

    public WaterlilyDbContext(DbContextOptions<WaterlilyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<PublicHoliday> PublicHolidays { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=WaterlilyDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC075B8CC195");
        });

        modelBuilder.Entity<PublicHoliday>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PublicHo__3214EC072DCC0DB9");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
