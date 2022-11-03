using System.Text.Json;
using System.Text.Json.Serialization;
using NameOrigin.Code1stModelGen;

namespace NameOrigin;

internal class JsonReadr
{
  internal static void ReadArray()
  {
    string[] ary = {
      @"{
    ""name"": ""maziar"",
    ""country_of_origin"": [
        {
            ""country_name"": ""Iran"",
            ""country"": ""IR"",
            ""probability"": 0.86,
            ""continental_region"": ""Asia"",
            ""statistical_region"": ""Southern Asia""
        },
        {
            ""country_name"": ""Canada"",
            ""country"": ""CA"",
            ""probability"": 0.03,
            ""continental_region"": ""Americas"",
            ""statistical_region"": ""Northern America""
        },
        {
            ""country_name"": ""United Kingdom"",
            ""country"": ""GB"",
            ""probability"": 0.02,
            ""continental_region"": ""Europe"",
            ""statistical_region"": ""Northern Europe""
        },
        {
            ""country_name"": ""United States"",
            ""country"": ""US"",
            ""probability"": 0.02,
            ""continental_region"": ""Americas"",
            ""statistical_region"": ""Northern America""
        }
    ],
    ""name_sanitized"": ""Maziar"",
    ""gender"": ""male"",
    ""samples"": 548,
    ""accuracy"": 99,
    ""country_of_origin_map_url"": ""https:\/\/gender-api.com\/en\/map\/26206162\/6b000a9d951f35bc"",
    ""credits_used"": 2,
    ""duration"": ""24ms""
}",
      @"{
    ""name"": ""jaspal"",
    ""country_of_origin"": [
        {
            ""country_name"": ""India"",
            ""country"": ""IN"",
            ""probability"": 0.68,
            ""continental_region"": ""Asia"",
            ""statistical_region"": ""Southern Asia""
        },
        {
            ""country_name"": ""Kuwait"",
            ""country"": ""KW"",
            ""probability"": 0.18,
            ""continental_region"": ""Asia"",
            ""statistical_region"": ""Western Asia""
        },
        {
            ""country_name"": ""Canada"",
            ""country"": ""CA"",
            ""probability"": 0.13,
            ""continental_region"": ""Americas"",
            ""statistical_region"": ""Northern America""
        },
        {
            ""country_name"": ""United Kingdom"",
            ""country"": ""GB"",
            ""probability"": 0.13,
            ""continental_region"": ""Europe"",
            ""statistical_region"": ""Northern Europe""
        },
        {
            ""country_name"": ""Cyprus"",
            ""country"": ""CY"",
            ""probability"": 0.12,
            ""continental_region"": ""Asia"",
            ""statistical_region"": ""Western Asia""
        },
        {
            ""country_name"": ""New Zealand"",
            ""country"": ""NZ"",
            ""probability"": 0.11,
            ""continental_region"": ""Oceania"",
            ""statistical_region"": ""AustraliaandNew Zealand""
        },
        {
            ""country_name"": ""Oman"",
            ""country"": ""OM"",
            ""probability"": 0.11,
            ""continental_region"": ""Asia"",
            ""statistical_region"": ""Western Asia""
        },
        {
            ""country_name"": ""United Arab Emirates"",
            ""country"": ""AE"",
            ""probability"": 0.07,
            ""continental_region"": ""Asia"",
            ""statistical_region"": ""Western Asia""
        },
        {
            ""country_name"": ""Australia"",
            ""country"": ""AU"",
            ""probability"": 0.06,
            ""continental_region"": ""Oceania"",
            ""statistical_region"": ""AustraliaandNew Zealand""
        },
        {
            ""country_name"": ""Singapore"",
            ""country"": ""SG"",
            ""probability"": 0.05,
            ""continental_region"": ""Asia"",
            ""statistical_region"": ""South-eastern Asia""
        },
        {
            ""country_name"": ""Thailand"",
            ""country"": ""TH"",
            ""probability"": 0.04,
            ""continental_region"": ""Asia"",
            ""statistical_region"": ""South-eastern Asia""
        },
        {
            ""country_name"": ""Austria"",
            ""country"": ""AT"",
            ""probability"": 0.03,
            ""continental_region"": ""Europe"",
            ""statistical_region"": ""Western Europe""
        },
        {
            ""country_name"": ""Malaysia"",
            ""country"": ""MY"",
            ""probability"": 0.03,
            ""continental_region"": ""Asia"",
            ""statistical_region"": ""South-eastern Asia""
        },
        {
            ""country_name"": ""United States"",
            ""country"": ""US"",
            ""probability"": 0.02,
            ""continental_region"": ""Americas"",
            ""statistical_region"": ""Northern America""
        }
    ],
    ""name_sanitized"": ""Jaspal"",
    ""gender"": ""male"",
    ""samples"": 483,
    ""accuracy"": 96,
    ""country_of_origin_map_url"": ""https:\/\/gender-api.com\/en\/map\/26206190\/387999678b6fc033"",
    ""credits_used"": 4,
    ""duration"": ""27ms""
}",
      @"{
    ""name"": ""chartabian"",
    ""country_of_origin"": [],
    ""gender"": ""unknown"",
    ""samples"": 0,
    ""accuracy"": 0,
    ""duration"": ""17ms""
}",
      @"{
    ""name"": ""bhavik"",
    ""country_of_origin"": [
        {
            ""country_name"": ""India"",
            ""country"": ""IN"",
            ""probability"": 0.92,
            ""continental_region"": ""Asia"",
            ""statistical_region"": ""Southern Asia""
        },
        {
            ""country_name"": ""Nepal"",
            ""country"": ""NP"",
            ""probability"": 0.11,
            ""continental_region"": ""Asia"",
            ""statistical_region"": ""Southern Asia""
        },
        {
            ""country_name"": ""Tanzania"",
            ""country"": ""TZ"",
            ""probability"": 0.07,
            ""continental_region"": ""Africa"",
            ""statistical_region"": ""Eastern Africa""
        },
        {
            ""country_name"": ""New Zealand"",
            ""country"": ""NZ"",
            ""probability"": 0.06,
            ""continental_region"": ""Oceania"",
            ""statistical_region"": ""AustraliaandNew Zealand""
        },
        {
            ""country_name"": ""Australia"",
            ""country"": ""AU"",
            ""probability"": 0.06,
            ""continental_region"": ""Oceania"",
            ""statistical_region"": ""AustraliaandNew Zealand""
        },
        {
            ""country_name"": ""United Arab Emirates"",
            ""country"": ""AE"",
            ""probability"": 0.05,
            ""continental_region"": ""Asia"",
            ""statistical_region"": ""Western Asia""
        },
        {
            ""country_name"": ""United Kingdom"",
            ""country"": ""GB"",
            ""probability"": 0.05,
            ""continental_region"": ""Europe"",
            ""statistical_region"": ""Northern Europe""
        },
        {
            ""country_name"": ""Canada"",
            ""country"": ""CA"",
            ""probability"": 0.04,
            ""continental_region"": ""Americas"",
            ""statistical_region"": ""Northern America""
        },
        {
            ""country_name"": ""Norway"",
            ""country"": ""NO"",
            ""probability"": 0.03,
            ""continental_region"": ""Europe"",
            ""statistical_region"": ""Northern Europe""
        },
        {
            ""country_name"": ""Kenya"",
            ""country"": ""KE"",
            ""probability"": 0.03,
            ""continental_region"": ""Africa"",
            ""statistical_region"": ""Eastern Africa""
        },
        {
            ""country_name"": ""United States"",
            ""country"": ""US"",
            ""probability"": 0.02,
            ""continental_region"": ""Americas"",
            ""statistical_region"": ""Northern America""
        }
    ],
    ""name_sanitized"": ""Bhavik"",
    ""gender"": ""male"",
    ""samples"": 1122,
    ""accuracy"": 100,
    ""country_of_origin_map_url"": ""https:\/\/gender-api.com\/en\/map\/26206249\/3788b2ae338c78fd"",
    ""credits_used"": 4,
    ""duration"": ""23ms""
}",
      @"{
    ""name"": ""yogesh"",
    ""country_of_origin"": [
        {
            ""country_name"": ""India"",
            ""country"": ""IN"",
            ""probability"": 0.67,
            ""continental_region"": ""Asia"",
            ""statistical_region"": ""Southern Asia""
        },
        {
            ""country_name"": ""Nepal"",
            ""country"": ""NP"",
            ""probability"": 0.1,
            ""continental_region"": ""Asia"",
            ""statistical_region"": ""Southern Asia""
        },
        {
            ""country_name"": ""Mauritius"",
            ""country"": ""MU"",
            ""probability"": 0.09,
            ""continental_region"": ""Africa"",
            ""statistical_region"": ""Eastern Africa""
        },
        {
            ""country_name"": ""Fiji"",
            ""country"": ""FJ"",
            ""probability"": 0.03,
            ""continental_region"": ""Oceania"",
            ""statistical_region"": ""Melanesia""
        },
        {
            ""country_name"": ""New Zealand"",
            ""country"": ""NZ"",
            ""probability"": 0.02,
            ""continental_region"": ""Oceania"",
            ""statistical_region"": ""AustraliaandNew Zealand""
        },
        {
            ""country_name"": ""United Arab Emirates"",
            ""country"": ""AE"",
            ""probability"": 0.02,
            ""continental_region"": ""Asia"",
            ""statistical_region"": ""Western Asia""
        }
    ],
    ""name_sanitized"": ""Yogesh"",
    ""gender"": ""male"",
    ""samples"": 11009,
    ""accuracy"": 100,
    ""country_of_origin_map_url"": ""https:\/\/gender-api.com\/en\/map\/26613995\/87efbc5fee2f53d5"",
    ""credits_used"": 3,
    ""duration"": ""33ms""
}" };

    ary.ToArray().ToList().ForEach(r => Read(r));
  }
  internal static void Read(string jsonString)
  {
    var rootobj = System.Text.Json.JsonSerializer.Deserialize<FirstnameRootObject>(jsonString);

    WriteLine($"\\>>> {rootobj?.name,32}  {rootobj?.country_of_origin?.Count(),4}     {rootobj?.country_of_origin.FirstOrDefault()?.country_name}");
  }
}