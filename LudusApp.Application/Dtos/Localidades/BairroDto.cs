using System.Text.Json.Serialization;

namespace LudusApp.Application.Dtos.Localidades;

public class BairroDto
{
    [JsonPropertyName("id")]

    public int Id { get; set; }
    [JsonPropertyName("nome")]

    public string Nome { get; set; }
}