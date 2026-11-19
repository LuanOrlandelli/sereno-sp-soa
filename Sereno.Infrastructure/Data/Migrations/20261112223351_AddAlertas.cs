using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sereno.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAlertas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LeituraSensorId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotemCodigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Origem = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Nivel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Mensagem = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alertas_Leituras_LeituraSensorId",
                        column: x => x.LeituraSensorId,
                        principalTable: "Leituras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_Ativo_CriadoEm",
                table: "Alertas",
                columns: new[] { "Ativo", "CriadoEm" });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_LeituraSensorId",
                table: "Alertas",
                column: "LeituraSensorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_TotemCodigo",
                table: "Alertas",
                column: "TotemCodigo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas");
        }
    }
}
