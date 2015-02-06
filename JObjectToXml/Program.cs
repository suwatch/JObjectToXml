using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace JObjectToXml
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var serializer = new DataContractSerializer(typeof(CsmSubscription), null,
                    Int32.MaxValue, false, true, new DataContractSurrogate());

                var data = new CsmSubscription 
                { 
                    SubscriptionId = Guid.NewGuid().ToString(), 
                    RegistrationDate = DateTime.UtcNow, 
                    Properties = new JObject() 
                };

                data.Properties["str"] = "mystring";
                data.Properties["int"] = 2;
                data.Properties["bool"] = true;

                var obj = new JObject();
                obj["objstr"] = "mystring";
                obj["objint"] = 2;
                obj["objbool"] = true;

                data.Properties["obj"] = obj;

                // Serialize to xml
                var mem = new MemoryStream();
                using (var writer = XmlWriter.Create(mem))
                {
                    serializer.WriteObject(writer, data);
                }

                var dataStr = Encoding.UTF8.GetString(mem.ToArray());
                Console.WriteLine(dataStr);

                // Deserialize to object
                CsmSubscription copy;
                mem.Position = 0;
                using (var reader = XmlReader.Create(mem))
                {
                    copy = (CsmSubscription)serializer.ReadObject(reader);
                }

                // round tripping check
                mem = new MemoryStream();
                using (var writer = XmlWriter.Create(mem))
                {
                    serializer.WriteObject(writer, copy);
                }

                var copyStr = Encoding.UTF8.GetString(mem.ToArray());
                Console.WriteLine(copyStr);

                Console.WriteLine("roundtrip = " + (copyStr == dataStr));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
