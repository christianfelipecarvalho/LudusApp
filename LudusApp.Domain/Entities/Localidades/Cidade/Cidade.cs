namespace LudusApp.Domain.Entities.Localidades.Cidade;
using LudusApp.Domain.Entities.Local;

public class Cidade
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int EstadoId { get; set; }
    public ICollection<Local> Locais { get; set; }
}