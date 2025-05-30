﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "AspNetRoles" (
    "Id" text NOT NULL,
    "Name" character varying(256),
    "NormalizedName" character varying(256),
    "ConcurrencyStamp" text,
    CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetUsers" (
    "Id" text NOT NULL,
    "Nome" text NOT NULL,
    "DataNascimento" timestamp with time zone NOT NULL,
    "Cpf" text,
    "Cep" text,
    "Endereco" text,
    "Bairro" text,
    "Cidade" text,
    "Telefone" text,
    "Numero" text,
    "UserName" character varying(256),
    "NormalizedUserName" character varying(256),
    "Email" character varying(256),
    "NormalizedEmail" character varying(256),
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text,
    "SecurityStamp" text,
    "ConcurrencyStamp" text,
    "PhoneNumber" text,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetRoleClaims" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "RoleId" text NOT NULL,
    "ClaimType" text,
    "ClaimValue" text,
    CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserClaims" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "UserId" text NOT NULL,
    "ClaimType" text,
    "ClaimValue" text,
    CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text,
    "UserId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    "UserId" text NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250209195329_CriandoUsuariosTabelas', '9.0.1');

UPDATE "AspNetUsers" SET "Cpf" = '' WHERE "Cpf" IS NULL;
ALTER TABLE "AspNetUsers" ALTER COLUMN "Cpf" SET NOT NULL;
ALTER TABLE "AspNetUsers" ALTER COLUMN "Cpf" SET DEFAULT '';

ALTER TABLE "AspNetUsers" ADD "Estado" text;

ALTER TABLE "AspNetUsers" ADD ativo boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250215004550_EditandoUsuario', '9.0.1');

ALTER TABLE "AspNetUsers" ADD "GoogleId" text;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250320001545_AddGoogleIdToUsers', '9.0.1');

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250329120428_IniciandoTenant', '9.0.1');

ALTER TABLE "AspNetUsers" ADD "EmpresaId" uuid;

ALTER TABLE "AspNetUsers" ADD "IsMultiTenant" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "AspNetUsers" ADD "TenantId" text;

CREATE TABLE "Empresas" (
    "Id" uuid NOT NULL,
    "RazaoSocial" text NOT NULL,
    "NomeFantasia" text NOT NULL,
    "Apelido" text NOT NULL,
    "Email" text NOT NULL,
    "Telefone" text NOT NULL,
    "Ddd" text NOT NULL,
    "Endereco" text NOT NULL,
    "NumEndereco" text NOT NULL,
    "Cnpj" text NOT NULL,
    "Im" integer,
    "Ie" integer,
    "Estado" text NOT NULL,
    "Cidade" text NOT NULL,
    "Bairro" text NOT NULL,
    "Cep" text NOT NULL,
    "Status" integer NOT NULL,
    "Padrao" boolean NOT NULL,
    "DataHoraCadastro" timestamp with time zone NOT NULL,
    "TenantId" integer,
    CONSTRAINT "PK_Empresas" PRIMARY KEY ("Id")
);

CREATE INDEX "IX_AspNetUsers_EmpresaId" ON "AspNetUsers" ("EmpresaId");

ALTER TABLE "AspNetUsers" ADD CONSTRAINT "FK_AspNetUsers_Empresas_EmpresaId" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas" ("Id") ON DELETE SET NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250329140439_AdicionandoEmpresa', '9.0.1');

CREATE TABLE "Estados" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Nome" text NOT NULL,
    "Sigla" text NOT NULL,
    CONSTRAINT "PK_Estados" PRIMARY KEY ("Id")
);

CREATE TABLE "Cidades" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Nome" text NOT NULL,
    "EstadoId" integer NOT NULL,
    CONSTRAINT "PK_Cidades" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Cidades_Estados_EstadoId" FOREIGN KEY ("EstadoId") REFERENCES "Estados" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Bairros" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Nome" text NOT NULL,
    "CidadeId" integer NOT NULL,
    CONSTRAINT "PK_Bairros" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Bairros_Cidades_CidadeId" FOREIGN KEY ("CidadeId") REFERENCES "Cidades" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Bairros_CidadeId" ON "Bairros" ("CidadeId");

CREATE INDEX "IX_Cidades_EstadoId" ON "Cidades" ("EstadoId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250329170259_AdicionandoLocalidades', '9.0.1');

ALTER TABLE "AspNetUsers" DROP CONSTRAINT "FK_AspNetUsers_Empresas_EmpresaId";

ALTER TABLE "Empresas" ALTER COLUMN "TenantId" TYPE uuid USING (CASE WHEN "TenantId" IS NOT NULL THEN gen_random_uuid() ELSE NULL END);

ALTER TABLE "Empresas" ADD "DataUltimaAlteracao" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

ALTER TABLE "Empresas" ADD "UsuarioCriacao" text NOT NULL DEFAULT '';

ALTER TABLE "Empresas" ADD "UsuarioUltimaAlteracao" text NOT NULL DEFAULT '';

ALTER TABLE "AspNetUsers" ALTER COLUMN "TenantId" TYPE uuid USING (CASE WHEN "TenantId" IS NOT NULL THEN gen_random_uuid() ELSE NULL END);

CREATE TABLE "UsuariosEmpresas" (
    "EmpresaId" uuid NOT NULL,
    "UsuarioId" text NOT NULL,
    "Papel" integer NOT NULL,
    "DataVinculo" timestamp with time zone NOT NULL,
    "DataUltimaAlteracao" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_UsuariosEmpresas" PRIMARY KEY ("EmpresaId", "UsuarioId"),
    CONSTRAINT "FK_UsuariosEmpresas_AspNetUsers_UsuarioId" FOREIGN KEY ("UsuarioId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UsuariosEmpresas_Empresas_EmpresaId" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_UsuariosEmpresas_UsuarioId" ON "UsuariosEmpresas" ("UsuarioId");

ALTER TABLE "AspNetUsers" ADD CONSTRAINT "FK_AspNetUsers_Empresas_EmpresaId" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas" ("Id");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250330164623_AjustarUsuarioEmpresaRelacionamento', '9.0.1');

ALTER TABLE "Empresas" DROP COLUMN "Padrao";

ALTER TABLE "Empresas" ALTER COLUMN "NomeFantasia" DROP NOT NULL;

ALTER TABLE "Empresas" ALTER COLUMN "Apelido" DROP NOT NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250330171537_AjustandoColunasBancoEmpresa', '9.0.1');

ALTER TABLE "UsuariosEmpresas" ADD "TenantId" uuid;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250330214157_AdicionandoTenantUsuarioEmpresa', '9.0.1');

CREATE TABLE "Temas" (
    "Id" uuid NOT NULL,
    "BorderRadius" text NOT NULL,
    "DarkMode" boolean NOT NULL,
    "PrimaryColor" text NOT NULL,
    "SecondaryColor" text NOT NULL,
    "UsuarioId" text NOT NULL,
    CONSTRAINT "PK_Temas" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Temas_AspNetUsers_UsuarioId" FOREIGN KEY ("UsuarioId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_Temas_UsuarioId" ON "Temas" ("UsuarioId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250330230346_AddTemaEntity', '9.0.1');

CREATE TABLE "ConfiguracoesEmail" (
    "Id" uuid NOT NULL,
    "ServidorSMTP" text NOT NULL,
    "Porta" integer NOT NULL,
    "RemetenteEmail" text NOT NULL,
    "Senha" text NOT NULL,
    "EnableSSL" boolean NOT NULL,
    CONSTRAINT "PK_ConfiguracoesEmail" PRIMARY KEY ("Id")
);

CREATE TABLE "Emails" (
    "Id" uuid NOT NULL,
    "Destinatario" text NOT NULL,
    "Assunto" text NOT NULL,
    "Mensagem" text NOT NULL,
    "Status" integer NOT NULL,
    "DataEnvio" timestamp with time zone NOT NULL,
    "DataErro" timestamp with time zone,
    CONSTRAINT "PK_Emails" PRIMARY KEY ("Id")
);

CREATE TABLE "TemplatesEmail" (
    "Id" uuid NOT NULL,
    "Tipo" text NOT NULL,
    "Assunto" text NOT NULL,
    "Mensagem" text NOT NULL,
    CONSTRAINT "PK_TemplatesEmail" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IX_AspNetUsers_Cpf" ON "AspNetUsers" ("Cpf");

CREATE UNIQUE INDEX "IX_AspNetUsers_Email" ON "AspNetUsers" ("Email");

CREATE UNIQUE INDEX "IX_AspNetUsers_UserName" ON "AspNetUsers" ("UserName");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250401013957_AdicionandoEmail', '9.0.1');

ALTER TABLE "AspNetUsers" ALTER COLUMN "UsuarioCadastro" DROP NOT NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250415010348_AdicionandoTemaEntidadesBase', '9.0.1');

UPDATE "Empresas" SET "TenantId" = '00000000-0000-0000-0000-000000000000' WHERE "TenantId" IS NULL;
ALTER TABLE "Empresas" ALTER COLUMN "TenantId" SET NOT NULL;
ALTER TABLE "Empresas" ALTER COLUMN "TenantId" SET DEFAULT '00000000-0000-0000-0000-000000000000';

ALTER TABLE "Empresas" ADD "Ativo" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "Empresas" ADD "DataCadastro" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

ALTER TABLE "Empresas" ADD "RowVersion" bytea;

ALTER TABLE "Empresas" ADD "UsuarioCadastro" bigint NOT NULL DEFAULT 0;

CREATE TABLE "Locais" (
    "Id" uuid NOT NULL,
    "Nome" character varying(100) NOT NULL,
    "Status" integer NOT NULL,
    "Cep" text NOT NULL,
    "CidadeId" integer NOT NULL,
    "Bairro" text NOT NULL,
    "Rua" text NOT NULL,
    "DiasFuncionamento" integer NOT NULL,
    "Complemento" text,
    "HorarioAbertura" interval NOT NULL,
    "HorarioFechamento" interval NOT NULL,
    "Observacao" text,
    "ValorHora" double precision,
    "EmpresaId" uuid NOT NULL,
    "TenantId" uuid NOT NULL,
    "Ativo" boolean NOT NULL,
    "DataCadastro" timestamp with time zone NOT NULL,
    "UsuarioCadastro" bigint NOT NULL,
    "DataUltimaAlteracao" timestamp with time zone,
    "UsuarioUltimaAlteracao" bigint,
    "RowVersion" bytea,
    CONSTRAINT "PK_Locais" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Locais_Cidades_CidadeId" FOREIGN KEY ("CidadeId") REFERENCES "Cidades" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Locais_Empresas_EmpresaId" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Locais_CidadeId" ON "Locais" ("CidadeId");

CREATE INDEX "IX_Locais_EmpresaId" ON "Locais" ("EmpresaId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250418014559_AdicionandoLocalTemaeRefatorando', '9.0.1');

CREATE TABLE "Eventos" (
    "Id" uuid NOT NULL,
    "Numero" bigint,
    "Nome" character varying(100) NOT NULL,
    "Email" text NOT NULL,
    "Telefone" text NOT NULL,
    "ValorTotal" numeric NOT NULL,
    "ValorHora" numeric NOT NULL,
    "HoraInicio" interval NOT NULL,
    "HoraFim" interval NOT NULL,
    "DataEvento" date NOT NULL,
    "Status" integer NOT NULL,
    "DataAlteracao" timestamp with time zone NOT NULL,
    "IdLocal" uuid NOT NULL,
    "IdUsuario" text,
    "IdTenant" uuid,
    "Ativo" boolean NOT NULL,
    "DataCadastro" timestamp with time zone NOT NULL,
    "UsuarioCadastro" bigint NOT NULL,
    "DataUltimaAlteracao" timestamp with time zone,
    "UsuarioUltimaAlteracao" bigint,
    "RowVersion" bytea,
    CONSTRAINT "PK_Eventos" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Eventos_AspNetUsers_IdUsuario" FOREIGN KEY ("IdUsuario") REFERENCES "AspNetUsers" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_Eventos_Locais_IdLocal" FOREIGN KEY ("IdLocal") REFERENCES "Locais" ("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_Eventos_IdLocal" ON "Eventos" ("IdLocal");

CREATE INDEX "IX_Eventos_IdUsuario" ON "Eventos" ("IdUsuario");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250420003016_AdicionandoEventoeRelacionamentos', '9.0.1');

COMMIT;

