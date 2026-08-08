using LuckyExpenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuckyExpenses.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(c => c.Code).HasColumnName("code").IsRequired().HasMaxLength(50);
            builder.HasIndex(c => c.Code).IsUnique();
            builder.Property(c => c.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            builder.HasIndex(c => c.Name).IsUnique();
            builder.Property(c => c.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp");
            builder.Property(c => c.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp");
        }
    }
}