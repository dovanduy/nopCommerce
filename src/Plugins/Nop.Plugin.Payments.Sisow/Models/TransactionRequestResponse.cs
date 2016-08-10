using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nop.Plugin.Payments.Sisow.Models
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.sisow.nl/Sisow/REST")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://www.sisow.nl/Sisow/REST", IsNullable = false)]
    public partial class transactionrequest
    {
        /// <remarks/>
        public transactionrequestTransaction transaction { get; set; }

        /// <remarks/>
        public transactionrequestSignature signature { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.sisow.nl/Sisow/REST")]
    public partial class transactionrequestTransaction
    {
        /// <remarks/>
        public string issuerurl { get; set; }

        /// <remarks/>
        public string trxid { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.sisow.nl/Sisow/REST")]
    public partial class transactionrequestSignature
    {
        /// <remarks/>
        public string sha1 { get; set; }
    }


}
