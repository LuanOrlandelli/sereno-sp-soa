using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sereno.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPrevisoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrevisoesCalor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DataReferencia = table.Column<DateOnly>(type: "date", nullable: false),
                    Probabilidade = table.Column<double>(type: "double precision", nullable: false),
                    Nivel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    VersaoModelo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Mensagem = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrevisoesCalor", x => x.Id);
                    table.CheckConstraint("CK_PrevisoesCalor_Probabilidade", "\"Probabilidade\" >= 0 AND \"Probabilidade\" <= 1");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrevisoesCalor_DataReferencia",
                table: "PrevisoesCalor",
                column: "DataReferencia",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrevisoesCalor");
        }
    }
}
