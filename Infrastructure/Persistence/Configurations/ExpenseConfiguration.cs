using LuckyExpenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuckyExpenses.Infrastructure.Persistence.Configurations
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("expenses");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            builder.Property(e => e.CategoryId).HasColumnName("category_id").IsRequired();
            builder.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
            builder.Property(e => e.Title).HasColumnName("title").IsRequired().HasMaxLength(150);
            builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);
            builder.Property(e => e.Amount).HasColumnName("amount").HasPrecision(18, 2).IsRequired();
            builder.Property(e => e.ExpenseDate).HasColumnName("expense_date").IsRequired();
            builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp");
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => e.ExpenseDate);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PaymentMethod)
                .WithMany()
                .HasForeignKey(e => e.PaymentMethodId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}