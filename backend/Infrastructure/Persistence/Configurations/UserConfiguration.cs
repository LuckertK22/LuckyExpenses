using LuckyExpenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuckyExpenses.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(u => u.FirstName).HasColumnName("first_name").IsRequired().HasMaxLength(100);
            builder.Property(u => u.LastName).HasColumnName("last_name").IsRequired().HasMaxLength(100);
            builder.Property(u => u.Email).HasColumnName("email").IsRequired().HasMaxLength(255);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.PasswordHash).HasColumnName("password_hash").IsRequired();
            builder.Property(u => u.Role).HasColumnName("role").IsRequired();
            builder.Property(u => u.State).HasColumnName("state").IsRequired();
            builder.Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp");
            builder.Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp");
        }
    }
}