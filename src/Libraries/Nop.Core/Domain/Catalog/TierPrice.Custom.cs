using System;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a tier price
    /// </summary>
    public partial class TierPrice
    {
        /// <summary>
        /// Gets or sets the tier category.
        /// </summary>
        /// <value>
        /// The tier category.
        /// </value>
        public int? TierPriceCategoryId { get; set; }


        public virtual TierPriceCategory TierPriceCategory { get; set; }
    }

    /// <summary>
    /// Represents a tier proce category
    /// this way is possible to calculate tierprices over different products
    /// but belong to a category.
    /// </summary>
    /// <seealso cref="Nop.Core.BaseEntity" />
    public partial class TierPriceCategory : BaseEntity
    {
        public string TierPriceCategoryDesc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of order note creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
    }
}
