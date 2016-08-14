using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class TierPriceCategoryMap : NopEntityTypeConfiguration<TierPriceCategory>
    {
        public TierPriceCategoryMap()
        {
            this.ToTable("TierPriceCategory");
            this.HasKey(tp => tp.Id);
            this.Property(u => u.TierPriceCategoryDesc).IsRequired().HasMaxLength(400);
        }
    }
}