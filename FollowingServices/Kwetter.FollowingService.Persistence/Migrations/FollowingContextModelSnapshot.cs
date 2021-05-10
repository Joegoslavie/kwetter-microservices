﻿// <auto-generated />
using System;
using Kwetter.FollowingService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kwetter.FollowingService.Persistence.Migrations
{
    [DbContext(typeof(FollowingContext))]
    partial class FollowingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Kwetter.FollowingService.Persistence.Entity.BlockEntity", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("BlockedId")
                        .HasColumnType("int");

                    b.Property<DateTime>("BlockedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "BlockedId");

                    b.ToTable("Blocked");
                });

            modelBuilder.Entity("Kwetter.FollowingService.Persistence.Entity.FollowingEntity", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("FollowingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FollowingSince")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "FollowingId");

                    b.ToTable("Followings");
                });
#pragma warning restore 612, 618
        }
    }
}
