using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TestApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    EmpresaId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CNPJ = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: false),
                    NomeFantasia = table.Column<string>(type: "text", nullable: true),
                    CEP = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.EmpresaId);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedores",
                columns: table => new
                {
                    FornecedorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    CEP = table.Column<string>(type: "text", nullable: true),
                    TipoFornecedor = table.Column<string>(type: "text", nullable: false),
                    CNPJ = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: true),
                    CPF = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    RG = table.Column<string>(type: "text", nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.FornecedorId);
                });

            migrationBuilder.CreateTable(
                name: "EmpresaFornecedor",
                columns: table => new
                {
                    EmpresasEmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FornecedoresFornecedorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpresaFornecedor", x => new { x.EmpresasEmpresaId, x.FornecedoresFornecedorId });
                    table.ForeignKey(
                        name: "FK_EmpresaFornecedor_Empresas_EmpresasEmpresaId",
                        column: x => x.EmpresasEmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "EmpresaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpresaFornecedor_Fornecedores_FornecedoresFornecedorId",
                        column: x => x.FornecedoresFornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "FornecedorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmpresaFornecedor_FornecedoresFornecedorId",
                table: "EmpresaFornecedor",
                column: "FornecedoresFornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_CNPJ",
                table: "Empresas",
                column: "CNPJ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedores_CNPJ",
                table: "Fornecedores",
                column: "CNPJ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedores_CPF",
                table: "Fornecedores",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedores_RG",
                table: "Fornecedores",
                column: "RG",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpresaFornecedor");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "Fornecedores");
        }
    }
}
