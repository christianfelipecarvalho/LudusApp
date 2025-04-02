public class BuscaCepDto
{
    public string Cep { get; set; }
    public string Logradouro { get; set; }
    public string Complemento { get; set; }
    public string Bairro { get; set; }
    public int CidadeId { get; set; }
    public string CidadeNome { get; set; }
    public int EstadoId { get; set; }
    public string EstadoNome { get; set; }
}