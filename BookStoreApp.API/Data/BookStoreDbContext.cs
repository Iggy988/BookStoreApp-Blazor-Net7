using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Data;

public partial class BookStoreDbContext : IdentityDbContext<ApiUser>
{
    public BookStoreDbContext()
    {
    }

    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //trebamo dodati da nema errora
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authors__3214EC07B98EDE1A");

            entity.Property(e => e.Bio).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC07E3A5B2A8");

            entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EA274CB554").IsUnique();

            entity.Property(e => e.Image).HasMaxLength(50);
            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .HasColumnName("ISBN");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Summary).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_Books_ToTable");
        });

        // seed roles in EF
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER",
                Id = "4485D0FC-B959-4718-9653-D10AF39F0E64"
            },
            new IdentityRole
            {
                Name = "Administartor",
                NormalizedName = "ADMINISTRATOR",
                Id = "BEAED54E-B72B-4EF6-BF58-8D9DC5CA51C5"
            });

        var hasher = new PasswordHasher<ApiUser>();

        modelBuilder.Entity<ApiUser>().HasData(
            new ApiUser
            {
                Id = "792352C0-90B0-43E9-93C0-7B73E22422C5",
                Email = "admin@bookstore.com",
                NormalizedEmail = "ADMIN@BOOKSTORE.COM",
                UserName = "admin@bookstore.com",
                NormalizedUserName = "ADMIN@BOOKSTORE.COM",
                FirstName = "System",
                LastName = "Admin",
                PasswordHash = hasher.HashPassword(null, "P@ssword1")
            },
            new ApiUser
            {
                Id = "06650E98-2BE4-4D25-9EFD-EBF1BEDC7863",
                Email = "user@bookstore.com",
                NormalizedEmail = "USER@BOOKSTORE.COM",
                UserName = "user@bookstore.com",
                NormalizedUserName = "user@BOOKSTORE.COM",
                FirstName = "System",
                LastName = "User",
                PasswordHash = hasher.HashPassword(null, "P@ssword1")
            });

        //spajamo IdentityRole sa ApiUser
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                // users id
                RoleId = "4485D0FC-B959-4718-9653-D10AF39F0E64",
                UserId = "06650E98-2BE4-4D25-9EFD-EBF1BEDC7863"
            },
            new IdentityUserRole<string>
            {
                // admin id
                RoleId = "BEAED54E-B72B-4EF6-BF58-8D9DC5CA51C5",
                UserId = "792352C0-90B0-43E9-93C0-7B73E22422C5"
            });
        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
