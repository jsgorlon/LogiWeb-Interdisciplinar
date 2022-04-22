namespace logiWeb.Models;

abstract class Pessoa
{
    protected int Id { get; set; }
    protected string Nome { get; set; }
    protected string TipoDocumento { get; set; }
    protected string Documento { get; set; }
    protected string Endereco { get; set; }
    protected string Telefone { get; set; }
}