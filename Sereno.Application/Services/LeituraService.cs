
using Sereno.Application.DTOs;
using Sereno.Application.Interfaces;
using Sereno.Domain.Entities;
using Sereno.Domain.Enums;

namespace Sereno.Application.Services;

public class LeituraService
{
    private readonly ILeituraRepository _repository;

    public LeituraService(ILeituraRepository repository)
    {
        _repository = repository;
    }

    public async Task<LeituraRespostaDto> CriarAsync(
        CriarLeituraDto dto,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.TotemCodigo))
            throw new ArgumentException(
                "O código do totem é obrigatório.");

        if (dto.Temperatura < -50 ||
            dto.Temperatura > 70 ||
            double.IsNaN(dto.Temperatura) ||
            double.IsInfinity(dto.Temperatura))
        {
            throw new ArgumentException(
                "Temperatura fora da faixa permitida.");
        }

        if (dto.Umidade < 0 ||
            dto.Umidade > 100 ||
            double.IsNaN(dto.Umidade) ||
            double.IsInfinity(dto.Umidade))
        {
            throw new ArgumentException(
                "Umidade deve estar entre 0 e 100.");
        }

        var codigo = dto.TotemCodigo.Trim();

        var totem = await _repository.BuscarTotemPorCodigoAsync(
            codigo,
            cancellationToken);

        if (totem is null)
            throw new KeyNotFoundException(
                "Totem não encontrado.");

        if (!totem.Ativo)
            throw new InvalidOperationException(
                "O totem está desativado.");

        // Calcular o indice de calor.
        double indice = CalcularIndiceCalor(
            dto.Temperatura,
            dto.Umidade);

        // Classificar o nivel de risco.
        NivelRisco nivel = ClassificarRisco(indice);

        // Criar a leitura.
        var leitura = new LeituraSensor(
            totem.Id,
            dto.Temperatura,
            dto.Umidade,
            indice,
            nivel,
            DateTime.UtcNow);

        // Gerar alerta quando o nivel for Perigo.
        Alerta? alerta = null;

        if (nivel == NivelRisco.Perigo)
        {
            string mensagem =
                "Calor perigoso detectado! " +
                "Procure sombra, reduza a exposicao ao sol " +
                "e mantenha-se hidratado.";

            alerta = new Alerta(
                leitura.Id,
                codigo,
                nivel,
                mensagem);
        }

        // Salvar a leitura e o alerta na mesma operacao.
        var salva = await _repository.AdicionarAsync(
            leitura,
            alerta,
            cancellationToken);

        return ConverterParaDto(salva, codigo);
    }

    public async Task<LeituraRespostaDto?> BuscarUltimaAsync(
        string codigo,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new ArgumentException(
                "O código do totem é obrigatório.");

        codigo = codigo.Trim();

        var leitura = await _repository.BuscarUltimaAsync(
            codigo,
            cancellationToken);

        if (leitura is null)
            return null;

        return ConverterParaDto(leitura, codigo);
    }

    public static double CalcularIndiceCalor(
        double temperaturaCelsius,
        double umidade)
    {
        if (temperaturaCelsius < -50 ||
            temperaturaCelsius > 70 ||
            double.IsNaN(temperaturaCelsius) ||
            double.IsInfinity(temperaturaCelsius))
        {
            throw new ArgumentOutOfRangeException(
                nameof(temperaturaCelsius));
        }

        if (umidade < 0 ||
            umidade > 100 ||
            double.IsNaN(umidade) ||
            double.IsInfinity(umidade))
        {
            throw new ArgumentOutOfRangeException(
                nameof(umidade));
        }

        double t = temperaturaCelsius * 9.0 / 5.0 + 32.0;
        double r = umidade;

        // Aproximacao inicial do indice de calor (NOAA).
        double hi = 0.5 * (
            t + 61.0 +
            (t - 68.0) * 1.2 +
            r * 0.094);

        if ((hi + t) / 2.0 < 80.0)
        {
            return Math.Round(
                (hi - 32.0) * 5.0 / 9.0, 2);
        }

        // Equacao de regressao de Rothfusz.
        hi =
            -42.379 +
            2.04901523 * t +
            10.14333127 * r -
            0.22475541 * t * r -
            0.00683783 * t * t -
            0.05481717 * r * r +
            0.00122874 * t * t * r +
            0.00085282 * t * r * r -
            0.00000199 * t * t * r * r;

        // Ajuste para baixa umidade.
        if (r < 13 && t >= 80 && t <= 112)
        {
            hi -= ((13 - r) / 4.0) *
                  Math.Sqrt(
                      (17 - Math.Abs(t - 95.0)) / 17.0);
        }

        // Ajuste para alta umidade.
        if (r > 85 && t >= 80 && t <= 87)
        {
            hi += ((r - 85) / 10.0) *
                  ((87 - t) / 5.0);
        }

        return Math.Round(
            (hi - 32.0) * 5.0 / 9.0, 2);
    }

    public static NivelRisco ClassificarRisco(
        double indiceCalor)
    {
        if (indiceCalor >= 41)
            return NivelRisco.Perigo;

        if (indiceCalor >= 32)
            return NivelRisco.Atencao;

        return NivelRisco.Normal;
    }

    private static LeituraRespostaDto ConverterParaDto(
        LeituraSensor leitura,
        string codigoTotem)
    {
        return new LeituraRespostaDto
        {
            Id = leitura.Id,
            TotemCodigo = codigoTotem,
            Temperatura = leitura.Temperatura,
            Umidade = leitura.Umidade,
            IndiceCalor = leitura.IndiceCalor,
            Nivel = leitura.Nivel.ToString(),
            ColetadaEm = leitura.ColetadaEm
        };
    }
}
