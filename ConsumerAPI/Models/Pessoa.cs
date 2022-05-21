using Newtonsoft.Json;

namespace ConsumerAPI.Models
{
    class Pessoa
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        
        [JsonProperty("Nome")]
        public string Nome { get; set; }
    }
}
