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
                PreserveReferencesHandling = PreserveReferencesHandling.All
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
            if (item is string)
            {
                var messageAsString = item as string;

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

            // item is not a string, serialize as json
            var serializedObject = Serialize(item);
            Contract.Assume(string.IsNullOrWhiteSpace(serializedObject) == false);

            return new MemoryStream(encoding.GetBytes(serializedObject));
        }

        public object Deserialize(Type type, string data)
        {
            // deserialize and return
            var returnValue = JsonConvert.DeserializeObject(data, type, _serializerSettings);
            Contract.Assume(returnValue != null);

            return returnValue;
        }

        public object Deserialize(Type type, Stream data, Encoding encoding)
        {
            Contract.Assume(type != null);
            Contract.Assume(data != null);
            Contract.Assume(encoding != null);

            var bytes = new byte[data.Length];
            data.Read(bytes, 0, bytes.Length);

            var dataString = encoding.GetString(bytes);
            Contract.Assume(string.IsNullOrWhiteSpace(dataString) == false);
            
            return Deserialize(type, dataString);
        }

        public object Deserialize(Type type, byte[] data, Encoding encoding)
        {
            Contract.Assume(type != null);
            Contract.Assume(data != null);
            Contract.Assume(encoding != null);

            var dataString = encoding.GetString(data);
            Contract.Assume(string.IsNullOrWhiteSpace(dataString) == false);

            return Deserialize(type, dataString);
        }
    }
}