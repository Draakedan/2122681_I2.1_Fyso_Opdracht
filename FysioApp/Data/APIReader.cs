using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FysioAppUX.Models;

namespace FysioAppUX.Data
{
    public class APIReader
    {
        private static readonly string urlStart = "https://localhost:44307";
        private static readonly HttpClient client = new();
        public static async Task<List<Behandeling>> ProcessAllBehandelingen()
        {
            var streamTask = client.GetStreamAsync($"{urlStart}/Behandeling");

            var behandelingen = await JsonSerializer.DeserializeAsync<List<Behandeling>>(await streamTask);
            return behandelingen;
        }

        public static async Task<List<Behandeling>> ProcessSomeBehandelingen(string codeStart)
        {
            var streamTask = client.GetStreamAsync($"{urlStart}/Behandeling/some/{codeStart}");

            var behandelingen = await JsonSerializer.DeserializeAsync<List<Behandeling>>(await streamTask);
            return behandelingen;
        }

        public static async Task<Behandeling> ProcessOneBehandeling(string code)
        {
            var streamTask = client.GetStreamAsync($"{urlStart}/Behandeling/{code}");

            var behandelingen = await JsonSerializer.DeserializeAsync<Behandeling>(await streamTask);
            return behandelingen;
        }
    }
}
