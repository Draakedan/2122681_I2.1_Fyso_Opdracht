using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DomainModels.Models;

namespace InfrastructureAPIHandler.Data
{
    public class APIReader
    {
        private static readonly string urlStart = "https://localhost:44307";
        private static readonly HttpClient client = new();
        public static async Task<List<Treatment>> ProcessAllBehandelingen()
        {
            var streamTask = client.GetStreamAsync($"{urlStart}/Behandeling");

            List<Treatment> behandelingen = await JsonSerializer.DeserializeAsync<List<Treatment>>(await streamTask);
            return behandelingen;
        }

        public static async Task<List<Treatment>> ProcessSomeBehandelingen(string codeStart)
        {
            var streamTask = client.GetStreamAsync($"{urlStart}/Behandeling/some/{codeStart}");

            var behandelingen = await JsonSerializer.DeserializeAsync<List<Treatment>>(await streamTask);
            return behandelingen;
        }

        public static async Task<Treatment> ProcessOneBehandeling(string code)
        {
            var streamTask = client.GetStreamAsync($"{urlStart}/Behandeling/{code}");

            var behandelingen = await JsonSerializer.DeserializeAsync<Treatment>(await streamTask);
            return behandelingen;
        }
    }
}
