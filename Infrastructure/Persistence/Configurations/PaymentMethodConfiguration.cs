using LuckyExpenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuckyExpenses.Infrastructure.Persistence.Configurations
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.ToTable("payment_methods");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(p => p.UserId).HasColumnName("user_id").IsRequired();
            builder.Property(p => p.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            builder.Property(p => p.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp");
            builder.Property(p => p.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp");
            builder.HasIndex(p => p.UserId);
        }
    }
}