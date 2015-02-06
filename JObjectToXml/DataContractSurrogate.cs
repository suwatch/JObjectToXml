using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace JObjectToXml
{
    [AttributeUsage(AttributeTargets.Class)]
    class DataContractSurrogateAttribute : Attribute
    {
        public DataContractSurrogateAttribute(Type dataContractType)
        {
            DataContractType = dataContractType;
        }

        public Type DataContractType { get; set; }
    }

    class DataContractSurrogate : IDataContractSurrogate
    {
        public Type GetDataContractType(Type type)
        {
            var attrib = type.GetCustomAttributes(typeof(DataContractSurrogateAttribute), false).OfType<DataContractSurrogateAttribute>().FirstOrDefault();
            if (attrib != null && attrib.DataContractType != null)
            {
                return attrib.DataContractType;
            }
            return type;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            return ExplicitCast(obj, targetType);
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            return ExplicitCast(obj, targetType);
        }

        private object ExplicitCast(object obj, Type targetType)
        {
            if (obj != null)
            {
                var type = obj.GetType();
                if (type != targetType)
                {
                    var convert = type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .FirstOrDefault(m => m.Name == "op_Explicit" && m.ReturnType == targetType);
                    if (convert != null)
                    {
                        return convert.Invoke(null, new[] { obj });
                    }
                }
            }
            return obj;
        }

        public object GetCustomDataToExport(Type clrType, Type dataContractType) { throw new NotImplementedException(); }
        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType) { throw new NotImplementedException(); }
        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes) { throw new NotImplementedException(); }
        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData) { throw new NotImplementedException(); }
        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit) { throw new NotImplementedException(); }
    }
}