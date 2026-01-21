using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shortha.Infrastructre.Migrations
{
    /// <inheritdoc />
    public partial class SyncDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:hstore", ",,");

            // Create tables with IF NOT EXISTS
            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS ""Packages"" (
    ""Id"" text NOT NULL,
    ""IsDeleted"" boolean NOT NULL,
    ""Name"" text NOT NULL,
    ""Description"" character varying(500) NOT NULL,
    ""MaxUrls"" integer NOT NULL,
    ""Price"" numeric(18,2) NOT NULL,
    ""IsActive"" boolean NOT NULL DEFAULT TRUE,
    ""CreatedAt"" timestamp without time zone NOT NULL,
    ""UpdatedAt"" timestamp without time zone NOT NULL,
    ""CreatedBy"" text,
    ""UpdatedBy"" text,
    ""DurationInDays"" integer NOT NULL,
    CONSTRAINT ""PK_Packages"" PRIMARY KEY (""Id"")
);
");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS ""Roles"" (
    ""Id"" text NOT NULL,
    ""IsDeleted"" boolean NOT NULL,
    ""CreatedAt"" timestamp without time zone NOT NULL,
    ""UpdatedAt"" timestamp without time zone NOT NULL,
    ""CreatedBy"" text,
    ""UpdatedBy"" text,
    ""Name"" character varying(100) NOT NULL,
    ""IsActive"" character varying(1) NOT NULL DEFAULT '1',
    CONSTRAINT ""PK_Roles"" PRIMARY KEY (""Id"")
);
");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS ""Users"" (
    ""Id"" text NOT NULL,
    ""IsDeleted"" boolean NOT NULL,
    ""IsPremium"" boolean NOT NULL DEFAULT FALSE,
    ""Email"" text NOT NULL,
    ""Name"" text NOT NULL,
    ""Picture"" text NOT NULL,
    ""IsBlocked"" boolean NOT NULL DEFAULT FALSE,
    ""CreatedAt"" timestamp without time zone NOT NULL,
    ""LastLoginAt"" timestamp without time zone NOT NULL,
    ""UpdatedAt"" timestamp without time zone NOT NULL,
    ""CreatedBy"" text NOT NULL,
    ""UpdatedBy"" text NOT NULL,
    ""Provider"" text NOT NULL,
    ""RoleId"" text NOT NULL,
    CONSTRAINT ""PK_Users"" PRIMARY KEY (""Id""),
    CONSTRAINT ""FK_Users_Roles_RoleId"" FOREIGN KEY (""RoleId"") REFERENCES ""Roles"" (""Id"") ON DELETE RESTRICT
);
");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS ""Api"" (
    ""Id"" text NOT NULL,
    ""IsDeleted"" boolean NOT NULL,
    ""CreatedAt"" timestamp without time zone NOT NULL,
    ""UpdatedAt"" timestamp without time zone NOT NULL,
    ""CreatedBy"" text,
    ""UpdatedBy"" text,
    ""Key"" character varying(64) NOT NULL,
    ""Name"" character varying(100) NOT NULL,
    ""UserId"" text NOT NULL,
    ""IsActive"" boolean NOT NULL DEFAULT TRUE,
    ""LastUsed"" timestamp without time zone,
    ""ExpiresAt"" timestamp without time zone,
    CONSTRAINT ""PK_Api"" PRIMARY KEY (""Id""),
    CONSTRAINT ""FK_Api_Users_UserId"" FOREIGN KEY (""UserId"") REFERENCES ""Users"" (""Id"")
);
");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS ""AppConnection"" (
    ""Id"" text NOT NULL,
    ""IsDeleted"" boolean NOT NULL,
    ""CreatedAt"" timestamp without time zone NOT NULL,
    ""UpdatedAt"" timestamp without time zone NOT NULL,
    ""CreatedBy"" text,
    ""UpdatedBy"" text,
    ""PairCode"" text NOT NULL,
    ""ActivatedAt"" timestamp without time zone,
    ""UserId"" text,
    ""Version"" text NOT NULL,
    ""ConnectKey"" character varying(256),
    ""SecretKey"" text NOT NULL,
    ""Device"" text NOT NULL,
    ""DeviceMetadata"" hstore,
    ""Status"" text NOT NULL DEFAULT 'Pending',
    CONSTRAINT ""PK_AppConnection"" PRIMARY KEY (""Id""),
    CONSTRAINT ""FK_AppConnection_Users_UserId"" FOREIGN KEY (""UserId"") REFERENCES ""Users"" (""Id"") ON DELETE SET NULL
);
");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS ""audit_trails"" (
    ""Id"" uuid NOT NULL,
    ""UserId"" text,
    ""TrailType"" text NOT NULL,
    ""DateUtc"" timestamp without time zone NOT NULL,
    ""EntityName"" character varying(100) NOT NULL,
    ""PrimaryKey"" character varying(100),
    ""OldValues"" jsonb NOT NULL,
    ""NewValues"" jsonb NOT NULL,
    ""ChangedColumns"" jsonb NOT NULL,
    CONSTRAINT ""PK_audit_trails"" PRIMARY KEY (""Id""),
    CONSTRAINT ""FK_audit_trails_Users_UserId"" FOREIGN KEY (""UserId"") REFERENCES ""Users"" (""Id"") ON DELETE SET NULL
);
");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS ""Payments"" (
    ""Id"" text NOT NULL,
    ""PaymentHash"" text NOT NULL,
    ""IsDeleted"" boolean NOT NULL,
    ""UserId"" text NOT NULL,
    ""PaymentDate"" timestamp without time zone,
    ""CreatedAt"" timestamp without time zone NOT NULL,
    ""PaymentLink"" text,
    ""UpdatedAt"" timestamp without time zone NOT NULL,
    ""ExpirationDate"" timestamp without time zone NOT NULL,
    ""Amount"" numeric(18,2) NOT NULL,
    ""Currency"" character varying(3) NOT NULL,
    ""PaymentMethod"" character varying(50),
    ""TransactionId"" text,
    ""Status"" integer NOT NULL,
    ""CreatedBy"" text,
    ""UpdatedBy"" text,
    ""PackageId"" text NOT NULL,
    CONSTRAINT ""PK_Payments"" PRIMARY KEY (""Id""),
    CONSTRAINT ""FK_Payments_Packages_PackageId"" FOREIGN KEY (""PackageId"") REFERENCES ""Packages"" (""Id""),
    CONSTRAINT ""FK_Payments_Users_UserId"" FOREIGN KEY (""UserId"") REFERENCES ""Users"" (""Id"")
);
");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS ""Urls"" (
    ""Id"" text NOT NULL,
    ""IsDeleted"" boolean NOT NULL,
    ""OriginalUrl"" character varying(2048) NOT NULL,
    ""ShortCode"" character varying(20) NOT NULL,
    ""ClickCount"" integer NOT NULL DEFAULT 0,
    ""CreatedAt"" timestamp without time zone NOT NULL,
    ""UpdatedAt"" timestamp without time zone NOT NULL,
    ""ExpiresAt"" timestamp without time zone,
    ""IsActive"" boolean NOT NULL DEFAULT TRUE,
    ""UserId"" text,
    ""CreationSource"" integer NOT NULL,
    ""CreatedBy"" text,
    ""UpdatedBy"" text,
    ""ApiKey"" text,
    CONSTRAINT ""PK_Urls"" PRIMARY KEY (""Id""),
    CONSTRAINT ""FK_Urls_Api_ApiKey"" FOREIGN KEY (""ApiKey"") REFERENCES ""Api"" (""Id"") ON DELETE CASCADE,
    CONSTRAINT ""FK_Urls_Users_UserId"" FOREIGN KEY (""UserId"") REFERENCES ""Users"" (""Id"")
);
");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS ""Subscriptions"" (
    ""Id"" text NOT NULL,
    ""IsDeleted"" boolean NOT NULL,
    ""StartDate"" timestamp without time zone NOT NULL,
    ""EndDate"" timestamp without time zone NOT NULL,
    ""IsActive"" boolean NOT NULL DEFAULT TRUE,
    ""IsPending"" boolean NOT NULL,
    ""CreatedAt"" timestamp without time zone NOT NULL,
    ""UpdatedAt"" timestamp without time zone NOT NULL,
    ""PaymentId"" text NOT NULL,
    ""UserId"" text NOT NULL,
    ""PackageId"" text NOT NULL,
    ""CreatedBy"" text NOT NULL,
    ""UpdatedBy"" text NOT NULL,
    CONSTRAINT ""PK_Subscriptions"" PRIMARY KEY (""Id""),
    CONSTRAINT ""FK_Subscriptions_Packages_PackageId"" FOREIGN KEY (""PackageId"") REFERENCES ""Packages"" (""Id""),
    CONSTRAINT ""FK_Subscriptions_Payments_PaymentId"" FOREIGN KEY (""PaymentId"") REFERENCES ""Payments"" (""Id"") ON DELETE CASCADE,
    CONSTRAINT ""FK_Subscriptions_Users_UserId"" FOREIGN KEY (""UserId"") REFERENCES ""Users"" (""Id"") ON DELETE CASCADE
);
");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS ""Visits"" (
    ""Id"" text NOT NULL,
    ""VisitDate"" timestamp without time zone NOT NULL,
    ""UserAgent"" character varying(1000),
    ""IpAddress"" character varying(45),
    ""Referrer"" character varying(2048),
    ""Country"" character varying(100),
    ""Region"" character varying(100),
    ""City"" character varying(100),
    ""DeviceBrand"" character varying(50),
    ""DeviceType"" character varying(50),
    ""Browser"" character varying(50),
    ""Os"" character varying(50),
    ""IsBot"" boolean NOT NULL DEFAULT FALSE,
    ""Language"" character varying(10),
    ""TimeZone"" character varying(50),
    ""UrlId"" text NOT NULL,
    ""CreatedBy"" text NOT NULL,
    ""UpdatedBy"" text NOT NULL,
    ""AppUserId"" text,
    CONSTRAINT ""PK_Visits"" PRIMARY KEY (""Id""),
    CONSTRAINT ""FK_Visits_Urls_UrlId"" FOREIGN KEY (""UrlId"") REFERENCES ""Urls"" (""Id"") ON DELETE CASCADE,
    CONSTRAINT ""FK_Visits_Users_AppUserId"" FOREIGN KEY (""AppUserId"") REFERENCES ""Users"" (""Id"")
);
");

            // Create indexes if they don't exist
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_Api_UserId"" ON ""Api"" (""UserId"");");
            migrationBuilder.Sql(@"CREATE UNIQUE INDEX IF NOT EXISTS ""IX_AppConnection_UserId_ConnectKey"" ON ""AppConnection"" (""UserId"", ""ConnectKey"");");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_audit_trails_EntityName"" ON ""audit_trails"" (""EntityName"");");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_audit_trails_UserId"" ON ""audit_trails"" (""UserId"");");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_Payments_PackageId"" ON ""Payments"" (""PackageId"");");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_Payments_UserId"" ON ""Payments"" (""UserId"");");
            migrationBuilder.Sql(@"CREATE UNIQUE INDEX IF NOT EXISTS ""IX_Roles_Name"" ON ""Roles"" (""Name"");");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_Subscriptions_PackageId"" ON ""Subscriptions"" (""PackageId"");");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_Subscriptions_PaymentId"" ON ""Subscriptions"" (""PaymentId"");");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_Subscriptions_UserId"" ON ""Subscriptions"" (""UserId"");");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_Urls_ApiKey"" ON ""Urls"" (""ApiKey"");");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_Urls_UserId"" ON ""Urls"" (""UserId"");");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_Users_RoleId"" ON ""Users"" (""RoleId"");");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_Visits_AppUserId"" ON ""Visits"" (""AppUserId"");");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_Visits_UrlId"" ON ""Visits"" (""UrlId"");");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppConnection");

            migrationBuilder.DropTable(
                name: "audit_trails");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Visits");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Urls");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "Api");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
