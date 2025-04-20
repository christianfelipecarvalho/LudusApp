using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LudusApp.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoTemaEntidadesBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Verifica se a coluna já existe antes de tentar alterá-la
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 
                        FROM information_schema.columns 
                        WHERE table_name = 'AspNetUsers' 
                          AND column_name = 'UsuarioCadastro'
                    ) THEN
                        ALTER TABLE ""AspNetUsers"" ADD ""UsuarioCadastro"" text;
                    ELSE
                        ALTER TABLE ""AspNetUsers"" ALTER COLUMN ""UsuarioCadastro"" DROP NOT NULL;
                    END IF;
                END $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No método Down, revertendo a alteração para "NOT NULL"
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1 
                        FROM information_schema.columns 
                        WHERE table_name = 'AspNetUsers' 
                          AND column_name = 'UsuarioCadastro'
                    ) THEN
                        ALTER TABLE ""AspNetUsers"" ALTER COLUMN ""UsuarioCadastro"" SET NOT NULL;
                    END IF;
                END $$;
            ");
        }
    }
}