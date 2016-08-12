namespace Nop.Plugin.Payments.Sisow.Models
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.sisow.nl/Sisow/REST")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://www.sisow.nl/Sisow/REST", IsNullable = false)]
    public partial class refundresponse
    {

        private refundresponseRefund refundField;

        private refundresponseSignature signatureField;

        private string versionField;

        /// <remarks/>
        public refundresponseRefund refund
        {
            get
            {
                return this.refundField;
            }
            set
            {
                this.refundField = value;
            }
        }

        /// <remarks/>
        public refundresponseSignature signature
        {
            get
            {
                return this.signatureField;
            }
            set
            {
                this.signatureField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.sisow.nl/Sisow/REST")]
    public partial class refundresponseRefund
    {

        private string refundidField;

        /// <remarks/>
        public string refundid
        {
            get
            {
                return this.refundidField;
            }
            set
            {
                this.refundidField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.sisow.nl/Sisow/REST")]
    public partial class refundresponseSignature
    {

        private string sha1Field;

        /// <remarks/>
        public string sha1
        {
            get
            {
                return this.sha1Field;
            }
            set
            {
                this.sha1Field = value;
            }
        }
    }


}
