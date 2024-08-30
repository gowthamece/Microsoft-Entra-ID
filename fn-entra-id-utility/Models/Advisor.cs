using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace fn_entra_id_utility.Models
{
    public class Advisor
    {
        [JsonPropertyName("value")]
        public List<Value> Values { get; set; }
    }

    public class Value
    {
        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Properties
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("impact")]
        public string Impact { get; set; }

        [JsonPropertyName("impactedField")]
        public string ImpactedField { get; set; }

        [JsonPropertyName("impactedValue")]
        public string ImpactedValue { get; set; }

        [JsonPropertyName("lastUpdated")]
        public DateTime LastUpdated { get; set; }

        [JsonPropertyName("recommendationTypeId")]
        public string RecommendationTypeId { get; set; }

        [JsonPropertyName("shortDescription")]
        public ShortDescription ShortDescription { get; set; }

        [JsonPropertyName("extendedProperties")]
        public ExtendedProperties ExtendedProperties { get; set; }

        [JsonPropertyName("resourceMetadata")]
        public ResourceMetadata ResourceMetadata { get; set; }
    }

    public class ShortDescription
    {
        [JsonPropertyName("problem")]
        public string Problem { get; set; }

        [JsonPropertyName("solution")]
        public string Solution { get; set; }
    }

    public class ExtendedProperties
    {
        [JsonPropertyName("recommendationControl")]
        public string RecommendationControl { get; set; }

        [JsonPropertyName("maturityLevel")]
        public string MaturityLevel { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("recommendedActionLearnMore")]
        public string RecommendedActionLearnMore { get; set; }

        [JsonPropertyName("releaseNotes")]
        public string ReleaseNotes { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }
    }

    public class ResourceMetadata
    {
        [JsonPropertyName("resourceId")]
        public string ResourceId { get; set; }
    }
}
