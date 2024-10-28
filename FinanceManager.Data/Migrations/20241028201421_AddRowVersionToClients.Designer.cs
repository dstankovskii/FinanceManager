﻿// <auto-generated />
using System;
using FinanceManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinanceManager.Data.Migrations
{
    [DbContext(typeof(FinanceManagerDbContext))]
    [Migration("20241028201421_AddRowVersionToClients")]
    partial class AddRowVersionToClients
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FinanceManager.Data.Entities.ClientEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("balance");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea")
                        .HasColumnName("row_version");

                    b.HasKey("Id")
                        .HasName("pk_clients");

                    b.ToTable("clients", (string)null);
                });

            modelBuilder.Entity("FinanceManager.Data.Entities.TransactionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("amount");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid")
                        .HasColumnName("client_id");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamptz")
                        .HasColumnName("date_time");

                    b.Property<DateTime?>("RevertedAt")
                        .HasColumnType("timestamptz")
                        .HasColumnName("reverted_at");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_transactions");

                    b.HasIndex("ClientId")
                        .HasDatabaseName("ix_transactions_client_id");

                    b.ToTable("transactions", (string)null);
                });

            modelBuilder.Entity("FinanceManager.Data.Entities.TransactionEntity", b =>
                {
                    b.HasOne("FinanceManager.Data.Entities.ClientEntity", "Client")
                        .WithMany("Transactions")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_transactions_clients_client_id");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("FinanceManager.Data.Entities.ClientEntity", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}