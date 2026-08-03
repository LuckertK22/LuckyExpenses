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
            builder.Property(c => c.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            builder.Property(c => c.Icon).HasColumnName("icon").HasMaxLength(10);
            builder.Property(c => c.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp");
            builder.Property(c => c.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp");
        }
    }
}