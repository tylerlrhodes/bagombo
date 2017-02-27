using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using blog.Data;

namespace blog.Migrations
{
    [DbContext(typeof(BlogContext))]
    partial class BlogContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("blog.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAlternateKey("FirstName", "LastName");

                    b.ToTable("Author");
                });

            modelBuilder.Entity("blog.Models.BlogPost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AuthorId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("BlogPost");
                });

            modelBuilder.Entity("blog.Models.BlogPostCategory", b =>
                {
                    b.Property<int>("BlogPostId");

                    b.Property<int>("CategoryId");

                    b.HasKey("BlogPostId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("BlogPostCategory");
                });

            modelBuilder.Entity("blog.Models.BlogPostFeature", b =>
                {
                    b.Property<int>("FeatureId");

                    b.Property<int>("BlogPostId");

                    b.Property<int>("Order");

                    b.HasKey("FeatureId", "BlogPostId");

                    b.HasIndex("BlogPostId");

                    b.ToTable("BlogPostFeature");
                });

            modelBuilder.Entity("blog.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("blog.Models.Feature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Feature");
                });

            modelBuilder.Entity("blog.Models.BlogPost", b =>
                {
                    b.HasOne("blog.Models.Author", "Author")
                        .WithMany("BlogPosts")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("blog.Models.BlogPostCategory", b =>
                {
                    b.HasOne("blog.Models.BlogPost", "BlogPost")
                        .WithMany("Categories")
                        .HasForeignKey("BlogPostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("blog.Models.Category", "Category")
                        .WithMany("BlogPosts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("blog.Models.BlogPostFeature", b =>
                {
                    b.HasOne("blog.Models.BlogPost", "BlogPost")
                        .WithMany("Features")
                        .HasForeignKey("BlogPostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("blog.Models.Feature", "Feature")
                        .WithMany("BlogPosts")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
