using System.Text.Json.Serialization;

namespace LudusApp.Application.Dtos.Localidades;

public class EstadoDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("sigla")]
    public string Sigla { get; set; }

    [JsonPropertyName("nome")]
    public string Nome { get; set; }

}