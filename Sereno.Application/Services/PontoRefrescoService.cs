
using Sereno.Application.DTOs;
using Sereno.Application.Interfaces;
using Sereno.Domain.Entities;

namespace Sereno.Application.Services;

public class PontoRefrescoService
{
    private readonly IPontoRefrescoRepository _repository;

    public PontoRefrescoService(
        IPontoRefrescoRepository repository)
    {
        _repository = repository;
    }

    // ==================================================
    // CREATE - CADASTRAR
    // ==================================================

    public async Task<PontoRefrescoRespostaDto> CriarAsync(
        CriarPontoRefrescoDto dto,
        CancellationToken cancellationToken = default)
    {
        ValidarDados(
            dto.Nome,
            dto.Endereco,
            dto.Latitude,
            dto.Longitude,
            dto.TemAgua,
            dto.TemSombra);

        var ponto = new PontoRefresco(
            dto.Nome,
            dto.Endereco,
            dto.Latitude,
            dto.Longitude,
            dto.TemAgua,
            dto.TemSombra);

        var salvo = await _repository.AdicionarAsync(
            ponto,
            cancellationToken);

        return ConverterParaDto(salvo);
    }

    // ==================================================
    // READ - LISTAR
    // ==================================================

    public async Task<List<PontoRefrescoRespostaDto>> ListarAsync(
        bool? temAgua = null,
        bool? temSombra = null,
        CancellationToken cancellationToken = default)
    {
        var pontos = await _repository.ListarAsync(
            temAgua,
            temSombra,
            cancellationToken);

        return pontos
            .Select(ConverterParaDto)
            .ToList();
    }

    // ==================================================
    // READ - BUSCAR POR ID
    // ==================================================

    public async Task<PontoRefrescoRespostaDto?> BuscarPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidarId(id);

        var ponto = await _repository.BuscarPorIdAsync(
            id,
            cancellationToken);

        return ponto is null
            ? null
            : ConverterParaDto(ponto);
    }

    // ==================================================
    // UPDATE - ATUALIZAR
    // ==================================================

    public async Task<PontoRefrescoRespostaDto> AtualizarAsync(
        Guid id,
        AtualizarPontoRefrescoDto dto,
        CancellationToken cancellationToken = default)
    {
        ValidarId(id);

        ValidarDados(
            dto.Nome,
            dto.Endereco,
            dto.Latitude,
            dto.Longitude,
            dto.TemAgua,
            dto.TemSombra);

        var ponto = await _repository.BuscarPorIdAsync(
            id,
            cancellationToken);

        if (ponto is null)
        {
            throw new KeyNotFoundException(
                "Ponto de refresco não encontrado.");
        }

        ponto.Atualizar(
            dto.Nome,
            dto.Endereco,
            dto.Latitude,
            dto.Longitude,
            dto.TemAgua,
            dto.TemSombra);

        var atualizado = await _repository.AtualizarAsync(
            ponto,
            cancellationToken);

        return ConverterParaDto(atualizado);
    }

    // ==================================================
    // DELETE - EXCLUIR
    // ==================================================

    public async Task ExcluirAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidarId(id);

        var ponto = await _repository.BuscarPorIdAsync(
            id,
            cancellationToken);

        if (ponto is null)
        {
            throw new KeyNotFoundException(
                "Ponto de refresco não encontrado.");
        }

        bool possuiTotens = await _repository.PossuiTotensAsync(
            id,
            cancellationToken);

        if (possuiTotens)
        {
            throw new InvalidOperationException(
                "Este ponto possui totens vinculados. " +
                "Remova os vínculos antes de excluí-lo.");
        }

        await _repository.ExcluirAsync(
            ponto,
            cancellationToken);
    }

    // ==================================================
    // BUSCAR PONTOS MAIS PROXIMOS
    // ==================================================

    public async Task<List<PontoRefrescoRespostaDto>> BuscarProximosAsync(
        double latitude,
        double longitude,
        int limite = 3,
        CancellationToken cancellationToken = default)
    {
        ValidarCoordenadas(latitude, longitude);

        if (limite < 1 || limite > 20)
        {
            throw new ArgumentException(
                "O limite deve estar entre 1 e 20.");
        }

        var pontos = await _repository.ListarAsync(
            cancellationToken: cancellationToken);

        var resultado = pontos
            .Where(p => p.TemAgua || p.TemSombra)
            .Select(p =>
            {
                var dto = ConverterParaDto(p);

                dto.DistanciaKm = CalcularDistanciaKm(
                    latitude,
                    longitude,
                    p.Latitude,
                    p.Longitude);

                return dto;
            })
            .OrderBy(p => p.DistanciaKm)
            .ThenBy(p => p.Nome)
            .Take(limite)
            .ToList();

        return resultado;
    }

    // ==================================================
    // VALIDAR ID
    // ==================================================

    private static void ValidarId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Identificador do ponto inválido.");
        }
    }

    // ==================================================
    // VALIDAR DADOS
    // ==================================================

    private static void ValidarDados(
        string nome,
        string endereco,
        double latitude,
        double longitude,
        bool temAgua,
        bool temSombra)
    {
        if (string.IsNullOrWhiteSpace(nome) ||
            nome.Trim().Length < 3 ||
            nome.Trim().Length > 150)
        {
            throw new ArgumentException(
                "O nome deve ter entre 3 e 150 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(endereco) ||
            endereco.Trim().Length < 5 ||
            endereco.Trim().Length > 250)
        {
            throw new ArgumentException(
                "O endereço deve ter entre 5 e 250 caracteres.");
        }

        ValidarCoordenadas(latitude, longitude);

        if (!temAgua && !temSombra)
        {
            throw new ArgumentException(
                "O ponto deve oferecer água ou sombra.");
        }
    }

    // ==================================================
    // VALIDAR COORDENADAS
    // ==================================================

    private static void ValidarCoordenadas(
        double latitude,
        double longitude)
    {
        if (!double.IsFinite(latitude) ||
            latitude < -90 ||
            latitude > 90)
        {
            throw new ArgumentException(
                "Latitude inválida.");
        }

        if (!double.IsFinite(longitude) ||
            longitude < -180 ||
            longitude > 180)
        {
            throw new ArgumentException(
                "Longitude inválida.");
        }
    }

    // ==================================================
    // CALCULO DE DISTANCIA - HAVERSINE
    // ==================================================

    public static double CalcularDistanciaKm(
        double latitudeOrigem,
        double longitudeOrigem,
        double latitudeDestino,
        double longitudeDestino)
    {
        ValidarCoordenadas(
            latitudeOrigem,
            longitudeOrigem);

        ValidarCoordenadas(
            latitudeDestino,
            longitudeDestino);

        const double raioTerraKm = 6371.0;

        double lat1 = GrausParaRadianos(latitudeOrigem);
        double lon1 = GrausParaRadianos(longitudeOrigem);

        double lat2 = GrausParaRadianos(latitudeDestino);
        double lon2 = GrausParaRadianos(longitudeDestino);

        double diferencaLatitude = lat2 - lat1;
        double diferencaLongitude = lon2 - lon1;

        double a =
            Math.Pow(Math.Sin(diferencaLatitude / 2), 2) +
            Math.Cos(lat1) *
            Math.Cos(lat2) *
            Math.Pow(Math.Sin(diferencaLongitude / 2), 2);

        a = Math.Clamp(a, 0.0, 1.0);

        double c = 2 * Math.Atan2(
            Math.Sqrt(a),
            Math.Sqrt(1 - a));

        double distancia = raioTerraKm * c;

        return Math.Round(distancia, 3);
    }

    private static double GrausParaRadianos(
        double graus)
    {
        return graus * Math.PI / 180.0;
    }

    // ==================================================
    // CONVERTER ENTIDADE PARA DTO
    // ==================================================

    private static PontoRefrescoRespostaDto ConverterParaDto(
        PontoRefresco ponto)
    {
        return new PontoRefrescoRespostaDto
        {
            Id = ponto.Id,
            Nome = ponto.Nome,
            Endereco = ponto.Endereco,
            Latitude = ponto.Latitude,
            Longitude = ponto.Longitude,
            TemAgua = ponto.TemAgua,
            TemSombra = ponto.TemSombra,
            DistanciaKm = null
        };
    }
}
