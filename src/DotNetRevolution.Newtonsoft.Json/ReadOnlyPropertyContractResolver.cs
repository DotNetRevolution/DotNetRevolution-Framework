using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DotNetRevolution.Json
{
    public class ReadOnlyPropertyContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            Contract.Assume(type != null);

            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       .Select(p =>
                           {                               
                               var prop = base.CreateProperty(p, memberSerialization);

                               prop.Writable = p.SetMethod == null ? false : true;
                               prop.Readable = true;

                               return prop;
                           })
                       .ToList();
        }
    }
}