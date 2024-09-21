using CSharpFinalProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSharpFinalProject.EntityConfigurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.HasMany(u => u.BuyHistory).WithOne(sh => sh.User).HasForeignKey(u => u.UserId);
            builder.Ignore(nameof(User.Basket));
            builder.Ignore(nameof(User.TotalBasketCost));
        }
    }
}
