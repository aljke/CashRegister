using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CashRegister.AppDbContext;

namespace CashRegister.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CashRegister.Entities.CheckData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("UtcTime");

                    b.HasKey("Id");

                    b.ToTable("Checks");
                });

            modelBuilder.Entity("CashRegister.Entities.CheckProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<int>("CheckId");

                    b.Property<decimal>("Price");

                    b.Property<int>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("CheckId");

                    b.HasIndex("ProductId");

                    b.ToTable("CheckProducts");
                });

            modelBuilder.Entity("CashRegister.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Caption");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("CashRegister.Entities.CheckProduct", b =>
                {
                    b.HasOne("CashRegister.Entities.CheckData", "Check")
                        .WithMany("CheckProducts")
                        .HasForeignKey("CheckId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CashRegister.Entities.Product", "Product")
                        .WithMany("ThisCheckProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
