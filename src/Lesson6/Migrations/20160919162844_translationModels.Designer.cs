using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Lesson6.Models;

namespace Lesson6.Migrations
{
    [DbContext(typeof(WebshopRepository))]
    [Migration("20160919162844_translationModels")]
    partial class translationModels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
