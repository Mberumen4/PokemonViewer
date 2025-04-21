using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokemonViewer
{
    public partial class Form1 : Form
    {
        private const string PokeApiUrl = "https://pokeapi.co/api/v2/pokemon-species/";

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnGetPokemon_Click(object sender, EventArgs e)
        {
            string pokemonName = txtPokemonName.Text.ToLower();
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                MessageBox.Show("Please enter a Pokémon name.");
                return;
            }

            try
            {
                var pokemonData = await GetPokemonData(pokemonName);
                if (pokemonData != null)
                {
                    lblName.Text = $"Name: {pokemonData.Name}";
                    lblHappiness.Text = $"Happiness: {pokemonData.BaseHappiness}";
                    lblCaptureRate.Text = $"Capture Rate: {pokemonData.CaptureRate}";
                    lblHabitat.Text = $"Habitat: {pokemonData.HabitatName}";
                    lblGrowthRate.Text = $"Growth Rate: {pokemonData.GrowthRateName}";
                    lblFlavorText.Text = $"Flavor Text: {pokemonData.FlavorText}";
                    lblEggGroup.Text = $"Egg Group: {pokemonData.EggGroup}";
                }
                else
                {
                    MessageBox.Show("Pokémon not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task<PokemonData> GetPokemonData(string pokemonName)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(PokeApiUrl + pokemonName);
                    var pokemon = JsonConvert.DeserializeObject<PokemonData>(response);
                    return pokemon;
                }
                catch
                {
                    return null;
                }
            }
        }

        public class PokemonData
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("base_happiness")]
            public int BaseHappiness { get; set; }

            [JsonProperty("capture_rate")]
            public int CaptureRate { get; set; }

            [JsonProperty("habitat")]
            public Habitat Habitat { get; set; }

            [JsonProperty("growth_rate")]
            public GrowthRate GrowthRate { get; set; }

            [JsonProperty("flavor_text_entries")]
            public List<FlavorTextEntry> FlavorTextEntries { get; set; }

            [JsonProperty("egg_groups")]
            public List<EggGroup> EggGroups { get; set; }

            public string FlavorText =>
                FlavorTextEntries?.FirstOrDefault(f => f.Language.Name == "en")?.FlavorText?
                .Replace("\n", " ").Replace("\f", " ");

            public string EggGroup =>
                EggGroups != null ? string.Join(", ", EggGroups.Select(e => e.Name)) : "";

            public string HabitatName => Habitat?.Name ?? "Unknown";

            public string GrowthRateName => GrowthRate?.Name ?? "Unknown";
        }

        public class Habitat { [JsonProperty("name")] public string Name { get; set; } }

        public class GrowthRate { [JsonProperty("name")] public string Name { get; set; } }

        public class FlavorTextEntry
        {
            [JsonProperty("flavor_text")]
            public string FlavorText { get; set; }

            [JsonProperty("language")]
            public Language Language { get; set; }
        }

        public class Language { [JsonProperty("name")] public string Name { get; set; } }

        public class EggGroup { [JsonProperty("name")] public string Name { get; set; } }
    }
}
