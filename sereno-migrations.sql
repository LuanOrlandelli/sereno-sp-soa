CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261112200816_InitialCreate') THEN
    CREATE TABLE "PontosRefresco" (
        "Id" uuid NOT NULL,
        "Nome" character varying(150) NOT NULL,
        "Endereco" character varying(250) NOT NULL,
        "Latitude" double precision NOT NULL,
        "Longitude" double precision NOT NULL,
        "TemAgua" boolean NOT NULL,
        "TemSombra" boolean NOT NULL,
        "CriadoEm" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_PontosRefresco" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261112200816_InitialCreate') THEN
    CREATE TABLE "Totens" (
        "Id" uuid NOT NULL,
        "Codigo" character varying(50) NOT NULL,
        "Ativo" boolean NOT NULL,
        "PontoRefrescoId" uuid,
        "CriadoEm" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Totens" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Totens_PontosRefresco_PontoRefrescoId" FOREIGN KEY ("PontoRefrescoId") REFERENCES "PontosRefresco" ("Id") ON DELETE SET NULL
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261112200816_InitialCreate') THEN
    CREATE TABLE "Leituras" (
        "Id" uuid NOT NULL,
        "TotemId" uuid NOT NULL,
        "Temperatura" double precision NOT NULL,
        "Umidade" double precision NOT NULL,
        "IndiceCalor" double precision NOT NULL,
        "Nivel" character varying(20) NOT NULL,
        "ColetadaEm" timestamp with time zone NOT NULL,
        "CriadoEm" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Leituras" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Leituras_Totens_TotemId" FOREIGN KEY ("TotemId") REFERENCES "Totens" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261112200816_InitialCreate') THEN
    CREATE INDEX "IX_Leituras_ColetadaEm" ON "Leituras" ("ColetadaEm");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261112200816_InitialCreate') THEN
    CREATE INDEX "IX_Leituras_TotemId" ON "Leituras" ("TotemId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261112200816_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_Totens_Codigo" ON "Totens" ("Codigo");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261112200816_InitialCreate') THEN
    CREATE INDEX "IX_Totens_PontoRefrescoId" ON "Totens" ("PontoRefrescoId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261112200816_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20261112200816_InitialCreate', '10.0.4');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261112223351_AddAlertas') THEN
    CREATE TABLE "Alertas" (
        "Id" uuid NOT NULL,
        "LeituraSensorId" uuid NOT NULL,
        "TotemCodigo" character varying(50) NOT NULL,
        "Origem" character varying(30) NOT NULL,
        "Nivel" character varying(20) NOT NULL,
        "Mensagem" character varying(500) NOT NULL,
        "Ativo" boolean NOT NULL,
        "CriadoEm" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Alertas" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Alertas_Leituras_LeituraSensorId" FOREIGN KEY ("LeituraSensorId") REFERENCES "Leituras" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261112223351_AddAlertas') THEN
    CREATE INDEX "IX_Alertas_Ativo_CriadoEm" ON "Alertas" ("Ativo", "CriadoEm");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261112223351_AddAlertas') THEN
    CREATE UNIQUE INDEX "IX_Alertas_LeituraSensorId" ON "Alertas" ("LeituraSensorId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261112223351_AddAlertas') THEN
    CREATE INDEX "IX_Alertas_TotemCodigo" ON "Alertas" ("TotemCodigo");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261112223351_AddAlertas') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20261112223351_AddAlertas', '10.0.4');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113225846_AddPrevisoes') THEN
    CREATE TABLE "PrevisoesCalor" (
        "Id" uuid NOT NULL,
        "DataReferencia" date NOT NULL,
        "Probabilidade" double precision NOT NULL,
        "Nivel" character varying(20) NOT NULL,
        "VersaoModelo" character varying(100) NOT NULL,
        "Mensagem" character varying(500) NOT NULL,
        "CriadoEm" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_PrevisoesCalor" PRIMARY KEY ("Id"),
        CONSTRAINT "CK_PrevisoesCalor_Probabilidade" CHECK ("Probabilidade" >= 0 AND "Probabilidade" <= 1)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113225846_AddPrevisoes') THEN
    CREATE UNIQUE INDEX "IX_PrevisoesCalor_DataReferencia" ON "PrevisoesCalor" ("DataReferencia");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113225846_AddPrevisoes') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20261113225846_AddPrevisoes', '10.0.4');
    END IF;
END $EF$;
COMMIT;

