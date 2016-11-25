using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Data;

namespace Data.Migrations
{
    [DbContext(typeof(BloggingContext))]
    partial class BloggingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("Entity.Models.Blog", b =>
                {
                    b.Property<int>("BlogId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Url")
                        .IsRequired();

                    b.HasKey("BlogId");

                    b.ToTable("Blog");
                });

            modelBuilder.Entity("Entity.Models.Comment", b =>
                {
                    b.Property<long>("CommentID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Ctx")
                        .HasMaxLength(500);

                    b.Property<int>("PostID");

                    b.Property<string>("User")
                        .HasMaxLength(10);

                    b.Property<int?>("UserID");

                    b.HasKey("CommentID");

                    b.HasIndex("PostID");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("Entity.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BlogId");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("CreateOn");

                    b.Property<DateTime?>("ModifyOn");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("PostId");

                    b.HasIndex("BlogId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("Entity.Models.Tag", b =>
                {
                    b.Property<int>("TagID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Title")
                        .HasMaxLength(10);

                    b.HasKey("TagID");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("Entity.Models.Comment", b =>
                {
                    b.HasOne("Entity.Models.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Entity.Models.Post", b =>
                {
                    b.HasOne("Entity.Models.Blog", "Blog")
                        .WithMany("Post")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
