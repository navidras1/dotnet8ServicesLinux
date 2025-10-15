using System;
using System.Collections.Generic;
using ChatV1.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;

namespace ChatV1.DataAccess.Context;

public partial class ChatV1Context : DbContext
{
    private readonly IEncryptionProvider _provider = new Repository.ChatEncryptionProvider();
    public ChatV1Context()
    {
    }

    public ChatV1Context(DbContextOptions<ChatV1Context> options)
        : base(options)
    {
        _provider = new Repository.ChatEncryptionProvider();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseEncryption(_provider);
    }

    public virtual DbSet<ChatLog> ChatLogs { get; set; }

    public virtual DbSet<ChatRoom> ChatRooms { get; set; }

    public virtual DbSet<ChatRoomLog> ChatRoomLogs { get; set; }

    public virtual DbSet<ChatRoomMemeber> ChatRoomMemebers { get; set; }

    public virtual DbSet<ChatRoomType> ChatRoomTypes { get; set; }

    public virtual DbSet<ChatStatus> ChatStatuses { get; set; }

    public virtual DbSet<EmpMaster> EmpMasters { get; set; }

    public virtual DbSet<LogRequestResponse> LogRequestResponses { get; set; }

    public virtual DbSet<UserContanct> UserContancts { get; set; }

    public virtual DbSet<UserChatRoomReciever> UserChatRoomRecievers { get; set; }

    public virtual DbSet<SuperUserApi> SuperUserApi { get; set; }

    public virtual DbSet<OfflineAction> OfflineActions { get; set; }
    public virtual DbSet<ActionType> ActionTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=82.115.25.140;Database=n1;Username=postgres;Password=mysecretpassword");

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<ChatLog>(entity =>
    //    {
    //        entity.ToTable("ChatLog");

    //        entity.Property(e => e.CreateDate).HasColumnType("datetime");
    //        entity.Property(e => e.FromUserName).HasMaxLength(250);
    //        entity.Property(e => e.ToEmPid).HasColumnName("ToEmPId");
    //        entity.Property(e => e.ToUserName).HasMaxLength(250);

    //        entity.HasOne(d => d.ChatStatus).WithMany(p => p.ChatLogs)
    //            .HasForeignKey(d => d.ChatStatusId)
    //            .OnDelete(DeleteBehavior.ClientSetNull)
    //            .HasConstraintName("FK_ChatLog_ChatStatus");
    //    });

    //    modelBuilder.Entity<ChatRoom>(entity =>
    //    {
    //        entity.Property(e => e.ChatRoomName).HasMaxLength(500);
    //        entity.Property(e => e.CreateDatetime).HasColumnType("datetime");
    //        entity.Property(e => e.CreatorUserName).HasMaxLength(500);
    //        entity.Property(e => e.Description).HasMaxLength(1000);

    //        entity.HasOne(d => d.ChatRoomType).WithMany(p => p.ChatRooms)
    //            .HasForeignKey(d => d.ChatRoomTypeId)
    //            .HasConstraintName("FK_ChatRooms_ChatRoomType");
    //    });

    //    modelBuilder.Entity<ChatRoomLog>(entity =>
    //    {
    //        entity.ToTable("ChatRoomLog");

    //        entity.HasOne(d => d.ChatLog).WithMany(p => p.ChatRoomLogs)
    //            .HasForeignKey(d => d.ChatLogId)
    //            .OnDelete(DeleteBehavior.ClientSetNull)
    //            .HasConstraintName("FK_ChatRoomLog_ChatLog");

    //        entity.HasOne(d => d.ChatRoom).WithMany(p => p.ChatRoomLogs)
    //            .HasForeignKey(d => d.ChatRoomId)
    //            .OnDelete(DeleteBehavior.ClientSetNull)
    //            .HasConstraintName("FK_ChatRoomLog_ChatRooms");
    //    });

    //    modelBuilder.Entity<ChatRoomMemeber>(entity =>
    //    {
    //        entity.Property(e => e.CreateDateTime).HasColumnType("datetime");
    //        entity.Property(e => e.UserName).HasMaxLength(500);

    //        entity.HasOne(d => d.ChatRoom).WithMany(p => p.ChatRoomMemebers)
    //            .HasForeignKey(d => d.ChatRoomId)
    //            .OnDelete(DeleteBehavior.ClientSetNull)
    //            .HasConstraintName("FK_ChatRoomMemebers_ChatRooms");
    //    });

    //    modelBuilder.Entity<ChatRoomType>(entity =>
    //    {
    //        entity.ToTable("ChatRoomType");

    //        entity.Property(e => e.Description).HasMaxLength(500);
    //        entity.Property(e => e.Name).HasMaxLength(500);
    //    });

    //    modelBuilder.Entity<ChatStatus>(entity =>
    //    {
    //        entity.ToTable("ChatStatus");

    //        entity.Property(e => e.ChatStatus1)
    //            .HasMaxLength(50)
    //            .HasColumnName("ChatStatus");
    //        entity.Property(e => e.Description).HasMaxLength(500);
    //    });

    //    modelBuilder.Entity<EmpMaster>(entity =>
    //    {
    //        entity.ToTable("EmpMaster");

    //        entity.Property(e => e.LastSeenDate).HasColumnType("datetime");
    //        entity.Property(e => e.UserName).HasMaxLength(500);
    //    });

    //    modelBuilder.Entity<LogRequestResponse>(entity =>
    //    {
    //        entity.ToTable("LogRequestResponse");

    //        entity.Property(e => e.ControllerName).HasMaxLength(500);
    //        entity.Property(e => e.CreatedDate).HasColumnType("datetime");
    //        entity.Property(e => e.Request).HasMaxLength(500);
    //        entity.Property(e => e.Response).HasMaxLength(500);
    //        entity.Property(e => e.UserName).HasMaxLength(500);
    //    });

    //    modelBuilder.Entity<UserContanct>(entity =>
    //    {
    //        entity.Property(e => e.UserName).HasMaxLength(500);

    //        entity.HasOne(d => d.EmpMaster).WithMany(p => p.UserContancts)
    //            .HasForeignKey(d => d.EmpMasterId)
    //            .HasConstraintName("FK_UserContancts_EmpMaster");
    //    });

    //    OnModelCreatingPartial(modelBuilder);
    //}

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
