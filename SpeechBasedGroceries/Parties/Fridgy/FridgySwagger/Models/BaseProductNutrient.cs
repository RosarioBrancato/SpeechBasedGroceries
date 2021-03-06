// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace SpeechBasedGroceries.Parties.Fridgy.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class BaseProductNutrient
    {
        /// <summary>
        /// Initializes a new instance of the BaseProductNutrient class.
        /// </summary>
        public BaseProductNutrient()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the BaseProductNutrient class.
        /// </summary>
        /// <param name="nutrientbase">Possible values include:
        /// 'hundred_gramm', 'hundred_milliliter'</param>
        public BaseProductNutrient(double? carbs = default(double?), double? carbsSugar = default(double?), int? energyKcal = default(int?), double? fat = default(double?), double? fatSaturated = default(double?), double? fiber = default(double?), double? protein = default(double?), double? salt = default(double?), string nutrientbase = default(string))
        {
            Carbs = carbs;
            CarbsSugar = carbsSugar;
            EnergyKcal = energyKcal;
            Fat = fat;
            FatSaturated = fatSaturated;
            Fiber = fiber;
            Protein = protein;
            Salt = salt;
            Nutrientbase = nutrientbase;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "carbs")]
        public double? Carbs { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "carbs_sugar")]
        public double? CarbsSugar { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "energy_kcal")]
        public int? EnergyKcal { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "fat")]
        public double? Fat { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "fat_saturated")]
        public double? FatSaturated { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "fiber")]
        public double? Fiber { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "protein")]
        public double? Protein { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "salt")]
        public double? Salt { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'hundred_gramm',
        /// 'hundred_milliliter'
        /// </summary>
        [JsonProperty(PropertyName = "nutrientbase")]
        public string Nutrientbase { get; set; }

    }
}
