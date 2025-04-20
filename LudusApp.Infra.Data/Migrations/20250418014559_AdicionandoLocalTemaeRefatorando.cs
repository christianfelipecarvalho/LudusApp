using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LudusApp.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoLocalTemaeRefatorando : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Empresas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Empresas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCadastro",
                table: "Empresas",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Empresas",
                type: "bytea",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioCadastro",
                table: "Empresas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Locais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Cep = table.Column<string>(type: "text", nullable: false),
                    CidadeId = table.Column<int>(type: "integer", nullable: false),
                    Bairro = table.Column<string>(type: "text", nullable: false),
                    Rua = table.Column<string>(type: "text", nullable: false),
                    DiasFuncionamento = table.Column<int>(type: "integer", nullable: false),
                    Complemento = table.Column<string>(type: "text", nullable: true),
                    HorarioAbertura = table.Column<TimeSpan>(type: "interval", nullable: false),
                    HorarioFechamento = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Observacao = table.Column<string>(type: "text", nullable: true),
                    ValorHora = table.Column<double>(type: "double precision", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioCadastro = table.Column<long>(type: "bigint", nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioUltimaAlteracao = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locais_Cidades_CidadeId",
                        column: x => x.CidadeId,
                        principalTable: "Cidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Locais_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locais_CidadeId",
                table: "Locais",
                column: "CidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Locais_EmpresaId",
                table: "Locais",
                column: "EmpresaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locais");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "DataCadastro",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "UsuarioCadastro",
                table: "Empresas");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Empresas",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
