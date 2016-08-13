using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Nop.Core.Domain.Shipping
{
    /// <summary>
    /// Represents a shipping option
    /// </summary>
    public partial class ShippingOption
    {
         /// <summary>
        /// Minimal Order Amount
        /// </summary>
        public virtual decimal MinimalOrderAmount { get; set; }

        /// <summary>
        /// Reason Disabled </summary>
        public virtual string ReasonDisabled { get; set; }
        
        /// <summary>
        /// Disabled or not
        /// </summary>
        public virtual bool Disabled { get; set; }

    }

}
