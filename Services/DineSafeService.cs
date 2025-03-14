using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using TorontoDineSafeApp.Models;

namespace TorontoDineSafeApp.Services
{
    public class DineSafeService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://ckan0.cf.opendata.inter.prod-toronto.ca";

        public DineSafeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DineSafeRecord>> GetDineSafeDataAsync()
        {
            try
            {
                // Step 1: Get package metadata
                var packageUrl = $"{BaseUrl}/api/3/action/package_show";
                var response = await _httpClient.GetAsync($"{packageUrl}?id=dinesafe");
                response.EnsureSuccessStatusCode();
                
                var packageData = await response.Content.ReadFromJsonAsync<ApiResponse>();
                var resources = packageData?.Result?.Resources;
                
                if (resources == null || resources.Count == 0)
                {
                    throw new Exception("No resources found in the DineSafe package");
                }
                
                // Step 2: Find the resource with DineSafe data
                string resourceId = null;
                foreach (var resource in resources)
                {
                    if (resource.DatastoreActive && resource.Name.Contains("DineSafe", StringComparison.OrdinalIgnoreCase))
                    {
                        resourceId = resource.Id;
                        break;
                    }
                }
                
                if (string.IsNullOrEmpty(resourceId))
                {
                    throw new Exception("Could not find DineSafe resource in the package");
                }
                
                // Step 3: Get data from the datastore
                var datastoreUrl = $"{BaseUrl}/api/3/action/datastore_search";
                var datastoreResponse = await _httpClient.GetAsync($"{datastoreUrl}?id={resourceId}&limit=10000");
                datastoreResponse.EnsureSuccessStatusCode();
                
                var datastoreData = await datastoreResponse.Content.ReadFromJsonAsync<ApiResponse>();
                var records = datastoreData?.Result?.Records;
                
                if (records == null || records.Count == 0)
                {
                    throw new Exception("No records found in the DineSafe datastore");
                }
                
                // Step 4: Convert API records to DineSafeRecord objects
                var dineSafeRecords = new List<DineSafeRecord>();
                
                foreach (var record in records)
                {
                    // Only add records with valid coordinates
                    if (TryParseCoordinate(record.GetValueOrDefault("latitude"), out double latitude) &&
                        TryParseCoordinate(record.GetValueOrDefault("longitude"), out double longitude) &&
                        latitude != 0 && longitude != 0)
                    {
                        var dineSafeRecord = new DineSafeRecord
                        {
                            Id = record.GetValueOrDefault("_id"),
                            EstablishmentId = record.GetValueOrDefault("establishment_id"),
                            InspectionId = record.GetValueOrDefault("inspection_id"),
                            EstablishmentName = record.GetValueOrDefault("establishment_name"),
                            EstablishmentType = record.GetValueOrDefault("establishment_type"),
                            EstablishmentAddress = record.GetValueOrDefault("establishment_address"),
                            EstablishmentStatus = record.GetValueOrDefault("status"),
                            MinInspectionsPerYear = record.GetValueOrDefault("minimum_inspections_per_year"),
                            InfractionDetails = record.GetValueOrDefault("infraction_details"),
                            InspectionDate = record.GetValueOrDefault("inspection_date"),
                            Severity = record.GetValueOrDefault("severity"),
                            Action = record.GetValueOrDefault("action"),
                            Outcome = record.GetValueOrDefault("outcome"),
                            AmountFined = record.GetValueOrDefault("amount_fined"),
                            Latitude = latitude,
                            Longitude = longitude,
                            UniqueId = record.GetValueOrDefault("unique_id")
                        };
                        
                        dineSafeRecords.Add(dineSafeRecord);
                    }
                }
                
                return dineSafeRecords;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching DineSafe data: {ex.Message}");
                return new List<DineSafeRecord>();
            }
        }
        
        private bool TryParseCoordinate(string value, out double result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = 0;
                return false;
            }
            
            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
        }
    }
    
    // API response classes
    public class ApiResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        
        [JsonPropertyName("result")]
        public ApiResult Result { get; set; }
    }
    
    public class ApiResult
    {
        [JsonPropertyName("resources")]
        public List<Resource> Resources { get; set; }
        
        [JsonPropertyName("records")]
        public List<Dictionary<string, string>> Records { get; set; }
    }
    
    public class Resource
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("datastore_active")]
        public bool DatastoreActive { get; set; }
    }
    
    // Extension method to safely get values from dictionary
    public static class DictionaryExtensions
    {
        public static string GetValueOrDefault(this Dictionary<string, string> dict, string key)
        {
            return dict.TryGetValue(key, out string value) ? value : string.Empty;
        }
    }
}
