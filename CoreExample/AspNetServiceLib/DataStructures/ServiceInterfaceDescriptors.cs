using AspNetServiceLib.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections;

namespace AspNetServiceLib.DataStructures
{
    [ServiceInterfaceData("ServiceInterfaceDescriptors")]
    public struct ServiceInterfaceDescriptors : IEnumerable<ServiceInterfaceDescriptors.Descriptor>
    {
        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public struct Descriptor
        {
            [JsonProperty(Required = Required.Always)]
            public string Name { get; set; }

            [JsonProperty(Required = Required.AllowNull)]
            public string Value { get; set; }
        }

        private readonly Descriptor[] descriptorsArray;
        private IEnumerable<Descriptor> DescriptorsSequence => descriptorsArray ?? new Descriptor[0];

        [JsonIgnore]
        public IEnumerable<string> DescriptorNames => DescriptorsSequence.Select(d => d.Name);

        [JsonIgnore]
        public IEnumerable<string> DescriptorValues => DescriptorsSequence.Select(d => d.Value);

        [JsonIgnore]
        public IEnumerable<(string, string)> Descriptors => DescriptorsSequence.Select(d => (d.Name, d.Value));

        /// <summary>
        /// Returns descriptors which is not addressing a specific service interface implementation.
        /// Mainly this means that this is a descriptors structure without descriptors.
        /// </summary>
        [JsonIgnore]
        public static ServiceInterfaceDescriptors Unaddressed => default;

        /// <summary>
        /// Indicates that these descriptors are not addressing a specific service interface implementation.
        /// </summary>
        [JsonIgnore]
        public bool IsUnaddressed => !DescriptorsSequence.Any();

        /// <summary>
        /// Indicates that these descriptors specify a service interface, i.e. are not using filtering patterns.
        /// Unaddressed descriptors are always specific.
        /// </summary>
        [JsonIgnore]
        public bool IsSpecific => !DescriptorValues.Any(v => v == null);

        public ServiceInterfaceDescriptors(params (string Name, string Value)[] descriptorPairs) : this()
        {
            descriptorsArray = descriptorPairs.Select(d => new Descriptor() { Name = d.Name, Value = d.Value }).ToArray();
        }

        public ServiceInterfaceDescriptors(JToken descriptorJson) : this()
        {
            var array = descriptorJson as JArray;
            if (array == null)
            {
                throw new ArgumentException($"JSON descriptors needs to be an array.");
            }

            descriptorsArray = array.Select(d => new Descriptor() { Name = d.Value<string>("name"), Value = d.Value<string>("value") }).ToArray();
        }

        [JsonConstructor]
        private ServiceInterfaceDescriptors(IEnumerable<Descriptor> descriptors) : this()
        {
            descriptorsArray = descriptors.ToArray();
        }

        public JToken ToJson()
        {
            return JToken.FromObject(this);
        }

        public override string ToString()
        {
            if (IsUnaddressed)
            {
                return "(Unaddressed)";
            }

            string quote(string s) => s != null ? $"\"{s}\"" : null;
            string descValuePrint(string v) => quote(v) ?? "*";
            return $"({string.Join(" / ", DescriptorsSequence.Select(d => $"{d.Name}: {descValuePrint(d.Value)}"))})";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ServiceInterfaceDescriptors))
            {
                return false;
            }

            var other = (ServiceInterfaceDescriptors)obj;
            return DescriptorsSequence.SequenceEqual(other.DescriptorsSequence);
        }

        public override int GetHashCode()
        {
            var hashCode = -173912503;
            unchecked
            {
                hashCode = hashCode * -1521134295;
                foreach (var d in DescriptorsSequence)
                {
                    hashCode = hashCode * 23 + d.GetHashCode();
                }
            }
            return hashCode;
        }

        public IEnumerator<Descriptor> GetEnumerator()
        {
            var enumerable = descriptorsArray as IEnumerable<Descriptor>;
            return enumerable?.GetEnumerator() ?? Enumerable.Empty<Descriptor>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<Descriptor>).GetEnumerator();
        }
    }
}
