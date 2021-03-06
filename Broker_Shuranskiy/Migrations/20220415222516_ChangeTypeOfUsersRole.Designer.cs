// <auto-generated />
using System;
using Broker_Shuranskiy.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Broker_Shuranskiy.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220415222516_ChangeTypeOfUsersRole")]
    partial class ChangeTypeOfUsersRole
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.16")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Broker_Shuranskiy.Models.Bags", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Id_Stock")
                        .HasColumnType("int");

                    b.Property<int>("Stock_Count")
                        .HasColumnType("int");

                    b.Property<long?>("UsersId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UsersId");

                    b.ToTable("Bags");
                });

            modelBuilder.Entity("Broker_Shuranskiy.Models.Stocks", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Average_5yers_div_yeld")
                        .HasColumnType("real");

                    b.Property<float>("Min_Lot")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("Broker_Shuranskiy.Models.Users", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Balance")
                        .HasColumnType("real");

                    b.Property<string>("First_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Second_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("User_Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Broker_Shuranskiy.Models.Bags", b =>
                {
                    b.HasOne("Broker_Shuranskiy.Models.Users", null)
                        .WithMany("Bag")
                        .HasForeignKey("UsersId");
                });

            modelBuilder.Entity("Broker_Shuranskiy.Models.Users", b =>
                {
                    b.Navigation("Bag");
                });
#pragma warning restore 612, 618
        }
    }
}
