// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace SpeechBasedGroceries.Parties.Fridgy.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class BaseFridge
    {
        /// <summary>
        /// Initializes a new instance of the BaseFridge class.
        /// </summary>
        public BaseFridge()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the BaseFridge class.
        /// </summary>
        public BaseFridge(string name = default(string), System.Guid id = default(System.Guid))
        {
            Name = name;
            Id = id;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public System.Guid Id { get; set; }

    }
}
