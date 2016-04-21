using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DotNetRevolution.Core.Serialization;

namespace DotNetRevolution.Json
{
    public class JsonSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public JsonSerializer()
            : this(new JsonSerializerSettings
            {
                // this setting is required to save the type in the Json string, use this setting when deserializing as well
                TypeNameHandling = TypeNameHandling.Auto,

                // allow for non public constructors to be used when deserializing
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,

                // use custom contract resolver to serialize/deserialize public get/private set properties
                ContractResolver = new ReadOnlyPropertyContractResolver(),

                // save references to protect against stack overflow exceptions with self referencing property loops
                PreserveReferencesHandling = PreserveReferencesHandling.All,

                // indent children for readability
                Formatting = Formatting.Indented
            })
        {
        }

        public JsonSerializer(JsonSerializerSettings serializerSettings)
        {
            Contract.Requires(serializerSettings != null);

            _serializerSettings = serializerSettings;
        }
        
        public string Serialize(object item)
        {
            var returnValue = JsonConvert.SerializeObject(item, _serializerSettings);
            Contract.Assume(returnValue != null);

            return returnValue;
        }

        public Stream Serialize(object item, Encoding encoding)
        {
            // get item as a string
            var messageAsString = item as string;

            if (messageAsString == null)
            {
                // item is not a string, serialize as json
                var serializedObject = Serialize(item);
                Contract.Assume(!string.IsNullOrWhiteSpace(serializedObject));
                
                return new MemoryStream(encoding.GetBytes(serializedObject));
            }

            try
            {
                // validate item is json string
                JToken.Parse(messageAsString);

                // return item as memory stream
                return new MemoryStream(encoding.GetBytes(messageAsString));
            }
            catch (JsonReaderException)
            {
                // item is not valid json, serialize as json
                return new MemoryStream(encoding.GetBytes(Serialize(item)));
            }
        }

        public object Deserialize(Type type, string data)
        {
            // deserialize and return
            var returnValue = JsonConvert.DeserializeObject(data, type, _serializerSettings);
            Contract.Assume(returnValue != null);

            return returnValue;
        }
    }
}