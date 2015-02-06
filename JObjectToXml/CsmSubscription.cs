using System;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JObjectToXml
{
    [DataContract]
    [DataContractSurrogate(typeof(CsmSubscriptionSurrogate))]
    public class CsmSubscription
    {
        [DataMember]
        public string SubscriptionId { get; set; }

        [DataMember]
        public DateTime RegistrationDate { get; set; }

        [DataMember]
        public JObject Properties { get; set; }

        public static explicit operator CsmSubscriptionSurrogate(CsmSubscription src)
        {
            return new CsmSubscriptionSurrogate { Inner = src };
        }
    }

    // This class is for XML serialization logging purpose
    [DataContract(Name = "CsmSubscription")]
    public class CsmSubscriptionSurrogate
    {
        private CsmSubscription _inner;

        public CsmSubscription Inner
        { 
            get { return _inner ?? (_inner = new CsmSubscription()); } 
            set { _inner = value; } 
        }

        [DataMember]
        public string SubscriptionId 
        { 
            get { return Inner.SubscriptionId; } 
            set { Inner.SubscriptionId = value; } 
        }

        [DataMember]
        public DateTime RegistrationDate 
        { 
            get { return Inner.RegistrationDate; } 
            set { Inner.RegistrationDate = value; } 
        }

        [DataMember]
        public virtual XElement Properties 
        { 
            get
            {
                return Inner.Properties == null ? null : JsonConvert.DeserializeXNode(Inner.Properties.ToString(Newtonsoft.Json.Formatting.None), "envelope").Root;
            }
            set
            {
                Inner.Properties = value == null ? null : JObject.Parse(JsonConvert.SerializeXNode(value, Newtonsoft.Json.Formatting.None, omitRootObject: true));
            }
        }

        public static explicit operator CsmSubscription(CsmSubscriptionSurrogate src)
        {
            return src.Inner;
        }
    }
}
