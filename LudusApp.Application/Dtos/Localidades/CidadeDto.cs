using System.Text.Json.Serialization;

namespace LudusApp.Application.Dtos.Localidades;

public class CidadeDto
{
    [JsonPropertyName("id")]

    public int Id { get; set; } // ID único da cidade
    [JsonPropertyName("nome")]

    public string Nome { get; set; } // Nome da cidade
    public int EstadoId { get; set; }
    public string EstadoNome { get; set; }
}