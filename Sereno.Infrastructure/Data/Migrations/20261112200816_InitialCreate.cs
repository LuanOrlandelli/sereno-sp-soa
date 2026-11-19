using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sereno.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PontosRefresco",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Endereco = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    TemAgua = table.Column<bool>(type: "boolean", nullable: false),
                    TemSombra = table.Column<bool>(type: "boolean", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PontosRefresco", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Totens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    PontoRefrescoId = table.Column<Guid>(type: "uuid", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Totens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Totens_PontosRefresco_PontoRefrescoId",
                        column: x => x.PontoRefrescoId,
                        principalTable: "PontosRefresco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Leituras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TotemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Temperatura = table.Column<double>(type: "double precision", nullable: false),
                    Umidade = table.Column<double>(type: "double precision", nullable: false),
                    IndiceCalor = table.Column<double>(type: "double precision", nullable: false),
                    Nivel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ColetadaEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leituras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leituras_Totens_TotemId",
                        column: x => x.TotemId,
                        principalTable: "Totens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leituras_ColetadaEm",
                table: "Leituras",
                column: "ColetadaEm");

            migrationBuilder.CreateIndex(
                name: "IX_Leituras_TotemId",
                table: "Leituras",
                column: "TotemId");

            migrationBuilder.CreateIndex(
                name: "IX_Totens_Codigo",
                table: "Totens",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Totens_PontoRefrescoId",
                table: "Totens",
                column: "PontoRefrescoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leituras");

            migrationBuilder.DropTable(
                name: "Totens");

            migrationBuilder.DropTable(
                name: "PontosRefresco");
        }
    }
}
