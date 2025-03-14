using System;
using CsvHelper.Configuration.Attributes;

namespace TorontoDineSafeApp.Models
{
    public class DineSafeRecord
    {
        [Name("_id")]
        public string Id { get; set; }

        [Name("establishment_id")]
        public string EstablishmentId { get; set; }

        [Name("inspection_id")]
        public string InspectionId { get; set; }

        [Name("establishment_name")]
        public string EstablishmentName { get; set; }

        [Name("establishment_type")]
        public string EstablishmentType { get; set; }

        [Name("establishment_address")]
        public string EstablishmentAddress { get; set; }

        [Name("status")]
        public string EstablishmentStatus { get; set; }

        [Name("minimum_inspections_per_year")]
        public string MinInspectionsPerYear { get; set; }

        [Name("infraction_details")]
        public string InfractionDetails { get; set; }

        [Name("inspection_date")]
        public string InspectionDate { get; set; }

        [Name("severity")]
        public string Severity { get; set; }

        [Name("action")]
        public string Action { get; set; }

        [Name("outcome")]
        public string Outcome { get; set; }

        [Name("amount_fined")]
        public string AmountFined { get; set; }

        [Name("latitude")]
        public double Latitude { get; set; }

        [Name("longitude")]
        public double Longitude { get; set; }

        [Name("unique_id")]
        public string UniqueId { get; set; }
    }
}
