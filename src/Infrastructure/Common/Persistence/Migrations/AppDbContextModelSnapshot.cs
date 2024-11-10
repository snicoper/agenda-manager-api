﻿// <auto-generated />
using System;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AgendaManager.Infrastructure.Common.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AgendaManager.Domain.Appointments.Aggregates.Appointment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CalendarId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("LastModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CalendarId");

                    b.HasIndex("ServiceId");

                    b.HasIndex("UserId");

                    b.ToTable("Appointments", (string)null);
                });

            modelBuilder.Entity("AgendaManager.Domain.Appointments.Aggregates.AppointmentStatusChange", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AppointmentId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("interval");

                    b.Property<bool>("IsCurrentStatus")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentId");

                    b.ToTable("AppointmentStatusChanges", (string)null);
                });

            modelBuilder.Entity("AgendaManager.Domain.AuditRecords.Aggregates.AuditRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<int>("ActionType")
                        .HasColumnType("integer");

                    b.Property<string>("AggregateId")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("AggregateName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("LastModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NamespaceName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NewValue")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("OldValue")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("PropertyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AggregateId");

                    b.HasIndex("AggregateName");

                    b.HasIndex("AggregateName", "AggregateId");

                    b.ToTable("AuditRecords", (string)null);
                });

            modelBuilder.Entity("AgendaManager.Domain.Calendars.Aggregates.Calendar", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTimeOffset>("LastModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Calendars", (string)null);
                });

            modelBuilder.Entity("AgendaManager.Domain.Calendars.Aggregates.CalendarHoliday", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<int[]>("AvailableDays")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<Guid>("CalendarId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTimeOffset>("LastModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CalendarId");

                    b.ToTable("CalendarHolidays", (string)null);
                });

            modelBuilder.Entity("AgendaManager.Domain.Resources.Aggregates.Resource", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CalendarId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTimeOffset>("LastModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CalendarId");

                    b.HasIndex("TypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Resources", (string)null);
                });

            modelBuilder.Entity("AgendaManager.Domain.Resources.Aggregates.ResourceSchedule", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<int>("AvailableDays")
                        .HasColumnType("integer");

                    b.Property<Guid>("CalendarId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTimeOffset>("LastModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid>("ResourceId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CalendarId");

                    b.HasIndex("ResourceId");

                    b.ToTable("ResourceSchedules", (string)null);
                });

            modelBuilder.Entity("AgendaManager.Domain.Resources.Aggregates.ResourceType", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTimeOffset>("LastModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("RequiredRole")
                        .HasColumnType("boolean");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ResourceTypes", (string)null);
                });

            modelBuilder.Entity("AgendaManager.Domain.Services.Aggregates.Service", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CalendarId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("interval");

                    b.Property<DateTimeOffset>("LastModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CalendarId");

                    b.ToTable("Services", (string)null);
                });

            modelBuilder.Entity("AgendaManager.Domain.Users.Aggregates.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("LastModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Permissions", (string)null);
                });

            modelBuilder.Entity("AgendaManager.Domain.Users.Aggregates.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<bool>("Editable")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("AgendaManager.Domain.Users.Aggregates.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("RoleId");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("uuid")
                        .HasColumnName("PermissionId");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("LastModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermissions", (string)null);
                });

            modelBuilder.Entity("ResourceTypeService", b =>
                {
                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uuid")
                        .HasColumnName("ServiceId");

                    b.Property<Guid>("ResourceTypeId")
                        .HasColumnType("uuid")
                        .HasColumnName("ResourceTypeId");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("LastModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ServiceId", "ResourceTypeId");

                    b.HasIndex("ResourceTypeId");

                    b.ToTable("ServiceResourceTypes", (string)null);
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("UserId");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("RoleId");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("LastModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("AgendaManager.Domain.Appointments.Aggregates.Appointment", b =>
                {
                    b.HasOne("AgendaManager.Domain.Calendars.Aggregates.Calendar", "Calendar")
                        .WithMany()
                        .HasForeignKey("CalendarId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AgendaManager.Domain.Services.Aggregates.Service", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AgendaManager.Domain.Users.Aggregates.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("AgendaManager.Domain.Common.ValueObjects.Period.Period", "Period", b1 =>
                        {
                            b1.Property<Guid>("AppointmentId")
                                .HasColumnType("uuid");

                            b1.Property<DateTimeOffset>("EndDate")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("EndDate");

                            b1.Property<DateTimeOffset>("StartDate")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("StartDate");

                            b1.HasKey("AppointmentId");

                            b1.ToTable("Appointments");

                            b1.WithOwner()
                                .HasForeignKey("AppointmentId");
                        });

                    b.Navigation("Calendar");

                    b.Navigation("Period")
                        .IsRequired();

                    b.Navigation("Service");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AgendaManager.Domain.Appointments.Aggregates.AppointmentStatusChange", b =>
                {
                    b.HasOne("AgendaManager.Domain.Appointments.Aggregates.Appointment", "Appointment")
                        .WithMany("StatusChanges")
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("AgendaManager.Domain.Common.ValueObjects.Period.Period", "Period", b1 =>
                        {
                            b1.Property<Guid>("AppointmentStatusChangeId")
                                .HasColumnType("uuid");

                            b1.Property<DateTimeOffset>("EndDate")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("EndDate");

                            b1.Property<DateTimeOffset>("StartDate")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("StartDate");

                            b1.HasKey("AppointmentStatusChangeId");

                            b1.ToTable("AppointmentStatusChanges");

                            b1.WithOwner()
                                .HasForeignKey("AppointmentStatusChangeId");
                        });

                    b.Navigation("Appointment");

                    b.Navigation("Period")
                        .IsRequired();
                });

            modelBuilder.Entity("AgendaManager.Domain.Calendars.Aggregates.CalendarHoliday", b =>
                {
                    b.HasOne("AgendaManager.Domain.Calendars.Aggregates.Calendar", "Calendar")
                        .WithMany()
                        .HasForeignKey("CalendarId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("AgendaManager.Domain.Common.ValueObjects.Period.Period", "Period", b1 =>
                        {
                            b1.Property<Guid>("CalendarHolidayId")
                                .HasColumnType("uuid");

                            b1.Property<DateTimeOffset>("EndDate")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("EndDate");

                            b1.Property<DateTimeOffset>("StartDate")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("StartDate");

                            b1.HasKey("CalendarHolidayId");

                            b1.ToTable("CalendarHolidays");

                            b1.WithOwner()
                                .HasForeignKey("CalendarHolidayId");
                        });

                    b.Navigation("Calendar");

                    b.Navigation("Period")
                        .IsRequired();
                });

            modelBuilder.Entity("AgendaManager.Domain.Resources.Aggregates.Resource", b =>
                {
                    b.HasOne("AgendaManager.Domain.Calendars.Aggregates.Calendar", "Calendar")
                        .WithMany()
                        .HasForeignKey("CalendarId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AgendaManager.Domain.Resources.Aggregates.ResourceType", "Type")
                        .WithMany("Resources")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AgendaManager.Domain.Users.Aggregates.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.OwnsOne("AgendaManager.Domain.Common.ValueObjects.ColorScheme.ColorScheme", "ColorScheme", b1 =>
                        {
                            b1.Property<Guid>("ResourceId")
                                .HasColumnType("uuid");

                            b1.Property<string>("BackgroundColor")
                                .IsRequired()
                                .HasMaxLength(6)
                                .HasColumnType("character varying(6)")
                                .HasColumnName("BackgroundColor");

                            b1.Property<string>("TextColor")
                                .IsRequired()
                                .HasMaxLength(6)
                                .HasColumnType("character varying(6)")
                                .HasColumnName("TextColor");

                            b1.HasKey("ResourceId");

                            b1.ToTable("Resources");

                            b1.WithOwner()
                                .HasForeignKey("ResourceId");
                        });

                    b.Navigation("Calendar");

                    b.Navigation("ColorScheme")
                        .IsRequired();

                    b.Navigation("Type");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AgendaManager.Domain.Resources.Aggregates.ResourceSchedule", b =>
                {
                    b.HasOne("AgendaManager.Domain.Calendars.Aggregates.Calendar", "Calendar")
                        .WithMany()
                        .HasForeignKey("CalendarId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AgendaManager.Domain.Resources.Aggregates.Resource", "Resource")
                        .WithMany()
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("AgendaManager.Domain.Common.ValueObjects.Period.Period", "Period", b1 =>
                        {
                            b1.Property<Guid>("ResourceScheduleId")
                                .HasColumnType("uuid");

                            b1.Property<DateTimeOffset>("EndDate")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("EndDate");

                            b1.Property<DateTimeOffset>("StartDate")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("StartDate");

                            b1.HasKey("ResourceScheduleId");

                            b1.ToTable("ResourceSchedules");

                            b1.WithOwner()
                                .HasForeignKey("ResourceScheduleId");
                        });

                    b.Navigation("Calendar");

                    b.Navigation("Period")
                        .IsRequired();

                    b.Navigation("Resource");
                });

            modelBuilder.Entity("AgendaManager.Domain.Services.Aggregates.Service", b =>
                {
                    b.HasOne("AgendaManager.Domain.Calendars.Aggregates.Calendar", "Calendar")
                        .WithMany()
                        .HasForeignKey("CalendarId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("AgendaManager.Domain.Common.ValueObjects.ColorScheme.ColorScheme", "ColorScheme", b1 =>
                        {
                            b1.Property<Guid>("ServiceId")
                                .HasColumnType("uuid");

                            b1.Property<string>("BackgroundColor")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("BackgroundColor");

                            b1.Property<string>("TextColor")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("TextColor");

                            b1.HasKey("ServiceId");

                            b1.ToTable("Services");

                            b1.WithOwner()
                                .HasForeignKey("ServiceId");
                        });

                    b.Navigation("Calendar");

                    b.Navigation("ColorScheme")
                        .IsRequired();
                });

            modelBuilder.Entity("AgendaManager.Domain.Users.Aggregates.User", b =>
                {
                    b.OwnsOne("AgendaManager.Domain.Common.ValueObjects.Token.Token", "RefreshToken", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<DateTimeOffset>("Expires")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("RefreshTokenExpires");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(200)
                                .IsUnicode(false)
                                .HasColumnType("character varying(200)")
                                .HasColumnName("RefreshToken");

                            b1.HasKey("UserId");

                            b1.HasIndex("Value")
                                .IsUnique();

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("RefreshToken");
                });

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.HasOne("AgendaManager.Domain.Users.Aggregates.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AgendaManager.Domain.Users.Aggregates.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ResourceTypeService", b =>
                {
                    b.HasOne("AgendaManager.Domain.Resources.Aggregates.ResourceType", null)
                        .WithMany()
                        .HasForeignKey("ResourceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AgendaManager.Domain.Services.Aggregates.Service", null)
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("AgendaManager.Domain.Users.Aggregates.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AgendaManager.Domain.Users.Aggregates.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AgendaManager.Domain.Appointments.Aggregates.Appointment", b =>
                {
                    b.Navigation("StatusChanges");
                });

            modelBuilder.Entity("AgendaManager.Domain.Resources.Aggregates.ResourceType", b =>
                {
                    b.Navigation("Resources");
                });
#pragma warning restore 612, 618
        }
    }
}
