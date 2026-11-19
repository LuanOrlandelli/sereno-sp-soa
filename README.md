# Sereno SP — SOA e Web Services

**Global Solution CitizenTech 2026 · FIAP**  
**Disciplina:** Arquitetura Orientada a Serviços (SOA) e Web Services

## Integrantes

> **Preencher antes da entrega:** nome completo e RM de todos os integrantes.

| Integrante | RM |
|---|---|
| Nome completo | RM |

## Sobre o projeto

O **Sereno SP** é uma solução CitizenTech voltada ao monitoramento de condições de calor urbano e ao apoio à população na identificação de situações de risco. Seu backend recebe leituras de sensores/totens, calcula e classifica indicadores de calor, registra alertas, mantém pontos de refresco e armazena previsões de calor.

Esta entrega reúne a API REST, o modelo de domínio, a persistência em PostgreSQL, as migrações do Entity Framework Core, a documentação OpenAPI/Swagger e os testes automatizados.

## Tecnologias

- C# e **.NET 10 / ASP.NET Core Web API**
- **PostgreSQL** e **Entity Framework Core** (provedor Npgsql)
- **OpenAPI** e **Swagger UI**
- **xUnit** para testes automatizados
- Git e GitHub

## Organização da solução

```text
Sereno.Api.slnx
Sereno.Api/              # Controllers, configuração e endpoints
Sereno.Application/      # Serviços de aplicação, DTOs e interfaces
Sereno.Domain/           # Entidades e enumerações
Sereno.Infrastructure/   # DbContext, repositórios e migrações EF Core
Sereno.Tests/            # Testes automatizados xUnit
evidencias/soa/           # Evidências de execução
sereno-migrations.sql    # Script SQL de migrações (complementar)
```

A separação em camadas permite manter as regras de domínio isoladas da API e da persistência. O `Program.cs` registra os repositórios e serviços por injeção de dependência.

## Funcionalidades e endpoints

| Método | Rota | Finalidade |
|---|---|---|
| GET | `/health` | Verificar disponibilidade da API e conexão com o banco |
| POST | `/api/totens` | Cadastrar totem |
| GET | `/api/totens` | Listar totens |
| GET | `/api/totens/{id}` | Consultar totem |
| PUT | `/api/totens/{id}` | Atualizar totem |
| DELETE | `/api/totens/{id}` | Excluir totem |
| POST | `/api/leituras` | Registrar leitura de sensor |
| GET | `/api/leituras/ultima?totemCodigo=STN-01` | Consultar última leitura de um totem |
| GET | `/api/alertas` | Listar alertas |
| POST | `/api/pontos` | Cadastrar ponto de refresco |
| GET | `/api/pontos` | Consultar pontos de refresco |
| GET | `/api/pontos/proximos?lat=-23.505&lon=-46.625&limite=3` | Encontrar pontos mais próximos |
| GET | `/api/pontos/{id}` | Consultar ponto |
| PUT | `/api/pontos/{id}` | Atualizar ponto |
| DELETE | `/api/pontos/{id}` | Excluir ponto |
| POST | `/api/previsoes` | Registrar previsão de calor |
| GET | `/api/previsoes/amanha` | Consultar previsão do dia seguinte |
| GET | `/api/previsoes/data/2026-11-14` | Consultar previsão por data |

> A documentação OpenAPI fornece os parâmetros e formatos de entrada e saída dos endpoints; alguns endpoints exigem dados previamente cadastrados.

## Requisitos para executar localmente

1. **SDK do .NET 10** instalado.
2. **PostgreSQL** em execução.
3. Uma base de dados para a aplicação e um usuário PostgreSQL com as permissões necessárias para as migrações.
4. Visual Studio ou terminal com a CLI `dotnet`.

### 1. Obter o código e restaurar dependências

```powershell
git clone https://github.com/LuanOrlandelli/sereno-sp-soa.git
cd sereno-sp-soa
dotnet restore Sereno.Api.slnx
```

### 2. Configurar a conexão com o banco

O projeto lê a string de conexão chamada `DefaultConnection`. **Não coloque credenciais no repositório.** Para desenvolvimento local, use User Secrets no projeto web:

```powershell
cd Sereno.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=sereno_sp;Username=SEU_USUARIO;Password=SUA_SENHA"
cd ..
```

Substitua `SEU_USUARIO` e `SUA_SENHA` pelos dados locais do seu PostgreSQL. O banco `sereno_sp` deverá existir e o usuário deverá ter as permissões adequadas.

### 3. Aplicar as migrações

No Visual Studio, abra **Ferramentas → Gerenciador de Pacotes NuGet → Console do Gerenciador de Pacotes** e execute:

```powershell
Update-Database -Project Sereno.Infrastructure -StartupProject Sereno.Api
```

As migrações versionadas estão em `Sereno.Infrastructure/Data/Migrations/`:

- `InitialCreate`
- `AddAlertas`
- `AddPrevisoes`

### 4. Executar a API

Na raiz da solução:

```powershell
dotnet run --project Sereno.Api --launch-profile https
```

A porta exata pode variar de acordo com o perfil de execução. No ambiente de desenvolvimento utilizado no projeto, a API foi testada em `https://localhost:7169`.

- **Swagger UI:** `https://localhost:7169/swagger`
- **OpenAPI JSON:** `https://localhost:7169/openapi/v1.json`
- **Health check:** `https://localhost:7169/health`

O Swagger e a documentação OpenAPI estão habilitados no ambiente `Development`.

## Testes automatizados

Na raiz da solução:

```powershell
dotnet test Sereno.Api.slnx
```

Na execução documentada para o desenvolvimento, **49 testes xUnit foram aprovados, com 0 falhas**. O comando acima permite reproduzir a execução no ambiente local.

## Critérios de avaliação de SOA

| Critério | Implementação / evidência |
|---|---|
| Estruturação do domínio (3,0) | `Sereno.Domain/Entities/`, `Sereno.Domain/Enums/` e serviços em `Sereno.Application/` |
| Conexão com banco (3,0) | `Sereno.Infrastructure/Data/SerenoDbContext.cs`, repositórios e registro do Npgsql no `Program.cs` |
| Migrações (1,5) | `Sereno.Infrastructure/Data/Migrations/` e `sereno-migrations.sql` |
| Documentação automática (1,5) | `/swagger` e `/openapi/v1.json` em ambiente de desenvolvimento |
| Testes automatizados (1,0) | `Sereno.Tests/` e evidência da execução xUnit |

## Evidências

Arquivos disponíveis em `evidencias/soa/`:

| Arquivo | O que demonstra |
|---|---|
| `01-swagger-endpoints.png` | Documentação automática dos endpoints |
| `02-health-200.png` | API e conexão com o banco respondendo |
| `03-totens-200.png` | Consulta REST aos totens |
| `04-compilacao.png` | Compilação bem-sucedida |
| `05-testes-xunit.png` | Execução dos testes automatizados |
| `06-compilacao-com-testes.png` | Compilação da solução com projeto de testes |

## Observações de segurança e ambiente

- Não publique senhas, `secrets.json`, arquivos `.env` reais, pastas `bin/`, `obj/`, `.vs/` ou artefatos de publicação.
- O ambiente de desenvolvimento local é demonstrativo. Para exposição pública, são necessários controles adicionais, como autenticação, TLS, validação e políticas de acesso.
- A infraestrutura Linux e os scripts de Operating Systems fazem parte de **outra entrega acadêmica**, em repositório separado.
