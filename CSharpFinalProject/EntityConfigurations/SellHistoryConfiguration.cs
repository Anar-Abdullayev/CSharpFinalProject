using CSharpFinalProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSharpFinalProject.EntityConfigurations
{
    internal class SellHistoryConfiguration : IEntityTypeConfiguration<SellHistory>
    {
        public void Configure(EntityTypeBuilder<SellHistory> builder)
        {
            builder.HasKey(sh => sh.HistoryId);
            builder.HasOne(sh=>sh.User).WithMany(u=>u.BuyHistory).HasForeignKey(sh=>sh.UserId);
            builder.HasOne(sh => sh.Product).WithMany(p => p.SellHistory).HasForeignKey(p=>p.ProductId);
        }
    }
}
