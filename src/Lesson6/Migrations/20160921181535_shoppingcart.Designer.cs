using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Lesson6.Models;

namespace Lesson6.Migrations
{
    [DbContext(typeof(WebshopRepository))]
    [Migration("20160921181535_shoppingcart")]
    partial class shoppingcart
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Lesson6.Models.Cart", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CartId");

                    b.Property<int>("Count");

                    b.Property<DateTime>("DateCreated");

                    b.Property<int>("ProductId");

                    b.HasKey("ItemId");

                    b.HasIndex("ProductId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("Lesson6.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<DateTime>("OrderDate");

                    b.Property<string>("Phone");

                    b.Property<string>("PostalCode");

                    b.Property<decimal>("Total");

                    b.Property<string>("Username");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Lesson6.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("OrderId");

                    b.Property<int>("ProductId");

                    b.Property<decimal>("UnitPrice");

                    b.Property<int>("count");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("Lesson6.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Price");

                    b.Property<int>("ProductCategoryId");

                    b.HasKey("ProductId");

                    b.HasIndex("ProductCategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Lesson6.Models.ProductCategory", b =>
                {
                    b.Property<int>("ProductCategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ProductCategoryName")
                        .IsRequired();

                    b.HasKey("ProductCategoryId");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("Lesson6.Models.ProductTranslation", b =>
                {
                    b.Property<int>("ProductId");

                    b.Property<string>("Language");

                    b.Property<string>("ProductDescription")
                        .IsRequired();

                    b.Property<string>("ProductName")
                        .IsRequired();

                    b.HasKey("ProductId", "Language");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductTranslations");
                });

            modelBuilder.Entity("Lesson6.Models.Cart", b =>
                {
                    b.HasOne("Lesson6.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Lesson6.Models.OrderDetail", b =>
                {
                    b.HasOne("Lesson6.Models.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Lesson6.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Lesson6.Models.Product", b =>
                {
                    b.HasOne("Lesson6.Models.ProductCategory", "ProductCategory")
                        .WithMany("Products")
                        .HasForeignKey("ProductCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Lesson6.Models.ProductTranslation", b =>
                {
                    b.HasOne("Lesson6.Models.Product")
                        .WithMany("Translations")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
