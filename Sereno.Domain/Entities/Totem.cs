
namespace Sereno.Domain.Entities;

public class Totem : EntidadeBase
{
    public string Codigo { get; private set; } = string.Empty;

    public bool Ativo { get; private set; } = true;

    public Guid? PontoRefrescoId { get; private set; }

    public PontoRefresco? PontoRefresco { get; private set; }

    public ICollection<LeituraSensor> Leituras { get; private set; }
        = new List<LeituraSensor>();

    private Totem() { }

    public Totem(
        string codigo,
        Guid? pontoRefrescoId = null)
    {
        ValidarCodigo(codigo);

        Codigo = codigo.Trim();
        PontoRefrescoId = pontoRefrescoId;
    }

    // ==================================================
    // ATUALIZAR DADOS DO TOTEM
    // ==================================================

    public void Atualizar(
        string codigo,
        Guid? pontoRefrescoId,
        bool ativo)
    {
        ValidarCodigo(codigo);

        Codigo = codigo.Trim();
        PontoRefrescoId = pontoRefrescoId;
        Ativo = ativo;
    }

    // ==================================================
    // ATIVAR E DESATIVAR
    // ==================================================

    public void Desativar()
    {
        Ativo = false;
    }

    public void Ativar()
    {
        Ativo = true;
    }

    // ==================================================
    // VALIDACAO
    // ==================================================

    private static void ValidarCodigo(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
        {
            throw new ArgumentException(
                "O código do totem é obrigatório.");
        }

        if (codigo.Trim().Length > 50)
        {
            throw new ArgumentException(
                "O código deve ter no máximo 50 caracteres.");
        }
    }
}
