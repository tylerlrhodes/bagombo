using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Bagombo.EFCore;

namespace Bagombo.Data.Migrations
{
    [DbContext(typeof(BlogDbContext))]
    [Migration("20170722224607_AuthorProfile1")]
    partial class AuthorProfile1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Bagombo.Models.Author", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("Biography");

                    b.Property<string>("Blurb");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("ImageLink");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Author");
                });

            modelBuilder.Entity("Bagombo.Models.BlogPost", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AuthorId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<bool>("Public");

                    b.Property<DateTime>("PublishOn");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("BlogPost");
                });

            modelBuilder.Entity("Bagombo.Models.BlogPostCategory", b =>
                {
                    b.Property<long>("BlogPostId");

                    b.Property<long>("CategoryId");

                    b.HasKey("BlogPostId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("BlogPostCategory");
                });

            modelBuilder.Entity("Bagombo.Models.BlogPostTopic", b =>
                {
                    b.Property<long>("TopicId");

                    b.Property<long>("BlogPostId");

                    b.HasKey("TopicId", "BlogPostId");

                    b.HasIndex("BlogPostId");

                    b.ToTable("BlogPostTopic");
                });

            modelBuilder.Entity("Bagombo.Models.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Bagombo.Models.Topic", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<bool>("ShowOnHomePage");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Topic");
                });

            modelBuilder.Entity("Bagombo.Models.BlogPost", b =>
                {
                    b.HasOne("Bagombo.Models.Author", "Author")
                        .WithMany("BlogPosts")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Bagombo.Models.BlogPostCategory", b =>
                {
                    b.HasOne("Bagombo.Models.BlogPost", "BlogPost")
                        .WithMany("BlogPostCategory")
                        .HasForeignKey("BlogPostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Bagombo.Models.Category", "Category")
                        .WithMany("BlogPosts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Bagombo.Models.BlogPostTopic", b =>
                {
                    b.HasOne("Bagombo.Models.BlogPost", "BlogPost")
                        .WithMany("BlogPostTopic")
                        .HasForeignKey("BlogPostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Bagombo.Models.Topic", "Topic")
                        .WithMany("BlogPosts")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
