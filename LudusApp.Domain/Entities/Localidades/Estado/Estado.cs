namespace LudusApp.Domain.Entities.Localidades.Estado;
using LudusApp.Domain.Entities.Localidades.Cidade;

public class Estado
{
    public int Id { get; set; } // ID único do estado
    public string Nome { get; set; }
    public string Sigla { get; set; } // Sigla do estado, como "SP"
    public ICollection<Cidade> Cidades { get; set; } // Relacionamento com cidades

}