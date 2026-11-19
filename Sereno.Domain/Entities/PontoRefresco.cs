
namespace Sereno.Domain.Entities;

public class PontoRefresco : EntidadeBase
{
    public string Nome { get; private set; } = string.Empty;

    public string Endereco { get; private set; } = string.Empty;

    public double Latitude { get; private set; }

    public double Longitude { get; private set; }

    public bool TemAgua { get; private set; }

    public bool TemSombra { get; private set; }

    public ICollection<Totem> Totens { get; private set; }
        = new List<Totem>();

    private PontoRefresco() { }

    // ==================================================
    // CONSTRUTOR
    // ==================================================

    public PontoRefresco(
        string nome,
        string endereco,
        double latitude,
        double longitude,
        bool temAgua,
        bool temSombra)
    {
        Validar(
            nome,
            endereco,
            latitude,
            longitude,
            temAgua,
            temSombra);

        Nome = nome.Trim();
        Endereco = endereco.Trim();
        Latitude = latitude;
        Longitude = longitude;
        TemAgua = temAgua;
        TemSombra = temSombra;
    }

    // ==================================================
    // ATUALIZAR PONTO DE REFRESCO
    // ==================================================

    public void Atualizar(
        string nome,
        string endereco,
        double latitude,
        double longitude,
        bool temAgua,
        bool temSombra)
    {
        Validar(
            nome,
            endereco,
            latitude,
            longitude,
            temAgua,
            temSombra);

        Nome = nome.Trim();
        Endereco = endereco.Trim();
        Latitude = latitude;
        Longitude = longitude;
        TemAgua = temAgua;
        TemSombra = temSombra;
    }

    // ==================================================
    // VALIDACAO DAS REGRAS DE DOMINIO
    // ==================================================

    private static void Validar(
        string nome,
        string endereco,
        double latitude,
        double longitude,
        bool temAgua,
        bool temSombra)
    {
        if (string.IsNullOrWhiteSpace(nome) ||
            nome.Trim().Length > 150)
        {
            throw new ArgumentException(
                "O nome é obrigatório e deve ter até 150 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(endereco) ||
            endereco.Trim().Length > 250)
        {
            throw new ArgumentException(
                "O endereço é obrigatório e deve ter até 250 caracteres.");
        }

        if (!double.IsFinite(latitude) ||
            latitude < -90 ||
            latitude > 90)
        {
            throw new ArgumentOutOfRangeException(
                nameof(latitude),
                "Latitude inválida.");
        }

        if (!double.IsFinite(longitude) ||
            longitude < -180 ||
            longitude > 180)
        {
            throw new ArgumentOutOfRangeException(
                nameof(longitude),
                "Longitude inválida.");
        }

        if (!temAgua && !temSombra)
        {
            throw new ArgumentException(
                "O ponto deve oferecer água ou sombra.");
        }
    }
}
