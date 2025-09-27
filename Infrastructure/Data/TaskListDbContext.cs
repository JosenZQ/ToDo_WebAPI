using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class TaskListDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public TaskListDbContext()
    {
    }

    public TaskListDbContext(DbContextOptions<TaskListDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAction> UserActions { get; set; }

    public virtual DbSet<UserTask> UserTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryCode);

            entity.Property(e => e.CategoryCode)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Category1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Category");
            entity.Property(e => e.CategoryId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserCode);

            entity.Property(e => e.UserCode)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(130)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Salt)
                .HasMaxLength(130)
                .IsUnicode(false);
            entity.Property(e => e.UserId).ValueGeneratedOnAdd();
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserAction>(entity =>
        {
            entity.HasKey(e => e.ActionId);

            entity.Property(e => e.ActionDate).HasColumnType("datetime");
            entity.Property(e => e.ActionDescr)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserCode)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.ActionCode)
                .HasMaxLength(4)
                .IsUnicode(false);

            entity.HasOne(d => d.UserCodeNavigation).WithMany(p => p.UserActions)
                .HasForeignKey(d => d.UserCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserActions_Users");
        });

        modelBuilder.Entity<UserTask>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK_Task");

            entity.Property(e => e.CategoryCode)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserCode)
                .HasMaxLength(4)
                .IsUnicode(false);

            entity.HasOne(d => d.CategoryCodeNavigation).WithMany(p => p.UserTasks)
                .HasForeignKey(d => d.CategoryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Task_Categories");

            entity.HasOne(d => d.UserCodeNavigation).WithMany(p => p.UserTasks)
                .HasForeignKey(d => d.UserCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Task_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
