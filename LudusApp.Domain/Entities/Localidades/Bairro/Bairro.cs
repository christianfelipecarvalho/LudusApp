namespace LudusApp.Domain.Entities.Localidades;

public class Bairro
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int CidadeId { get; set; } // Chave estrangeira
    public Cidade.Cidade Cidade { get; set; }
}