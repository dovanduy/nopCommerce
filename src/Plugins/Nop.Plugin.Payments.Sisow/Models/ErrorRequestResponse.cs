using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nop.Plugin.Payments.Sisow.Models
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.sisow.nl/Sisow/REST")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://www.sisow.nl/Sisow/REST", IsNullable = false)]
    public partial class errorresponse
    {
        /// <remarks/>
        public errorresponseError error { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.sisow.nl/Sisow/REST")]
    public partial class errorresponseError
    {
        /// <remarks/>
        public string errorcode { get; set; }

        /// <remarks/>
        public string errormessage { get; set; }
    }

}
