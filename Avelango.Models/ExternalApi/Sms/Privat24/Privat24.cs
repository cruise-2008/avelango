namespace Avelango.Models.ExternalApi.Sms.Privat24
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class request
    {

        private requestMerchant merchantField;

        private requestData dataField;

        private string versionField;

        private bool versionFieldSpecified;

        /// <remarks/>
        public requestMerchant merchant
        {
            get { return this.merchantField; }
            set { this.merchantField = value; }
        }

        /// <remarks/>
        public requestData data
        {
            get { return this.dataField; }
            set { this.dataField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get { return this.versionField; }
            set { this.versionField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool versionSpecified
        {
            get { return this.versionFieldSpecified; }
            set { this.versionFieldSpecified = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class requestMerchant
    {

        private int idField;

        private string signatureField;

        /// <remarks/>
        public int id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }

        /// <remarks/>
        public string signature
        {
            get { return this.signatureField; }
            set { this.signatureField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class requestData
    {

        private string operField;

        private sbyte waitField;

        private sbyte testField;

        private requestDataPayment paymentField;

        /// <remarks/>
        public string oper
        {
            get { return this.operField; }
            set { this.operField = value; }
        }

        /// <remarks/>
        public sbyte wait
        {
            get { return this.waitField; }
            set { this.waitField = value; }
        }

        /// <remarks/>
        public sbyte test
        {
            get { return this.testField; }
            set { this.testField = value; }
        }

        /// <remarks/>
        public requestDataPayment payment
        {
            get { return this.paymentField; }
            set { this.paymentField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class requestDataPayment
    {

        private requestDataPaymentProp[] propField;

        private string idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("prop")]
        public requestDataPaymentProp[] prop
        {
            get { return this.propField; }
            set { this.propField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class requestDataPaymentProp
    {

        private string nameField;

        private string valueField;

        private string valueField1;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get { return this.valueField; }
            set { this.valueField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get { return this.valueField1; }
            set { this.valueField1 = value; }
        }
    }
}
