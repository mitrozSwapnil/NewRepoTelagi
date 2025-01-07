using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TelgeProject.Entity;

namespace TelgeProject.Entity
{
    public partial class db_telgeprojectContext : DbContext
    {
        //public db_telgeprojectContext()
        //{
        //}

        public db_telgeprojectContext(DbContextOptions<db_telgeprojectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblApproval> TblApprovals { get; set; } = null!;
        public virtual DbSet<TblCustomer> TblCustomers { get; set; } = null!;
        public virtual DbSet<TblDocument> TblDocuments { get; set; } = null!;
        public virtual DbSet<TblLearningsfromproject> TblLearningsfromprojects { get; set; } = null!;
        public virtual DbSet<TblMilestone> TblMilestones { get; set; } = null!;
        public virtual DbSet<TblNotification> TblNotifications { get; set; } = null!;
        public virtual DbSet<TblProject> TblProjects { get; set; } = null!;
        public virtual DbSet<TblRevisionlog> TblRevisionlogs { get; set; } = null!;
        public virtual DbSet<TblSubtask> TblSubtasks { get; set; } = null!;
        public virtual DbSet<TblTask> TblTasks { get; set; } = null!;
        public virtual DbSet<TblTaskmapping> TblTaskmappings { get; set; } = null!;
        public virtual DbSet<TblTimesheet> TblTimesheets { get; set; } = null!;
        public virtual DbSet<TblUser> TblUsers { get; set; } = null!;
        public virtual DbSet<TblUsersrole> TblUsersroles { get; set; } = null!;

        public virtual DbSet<TblDepartment> TblDepartments { get; set; } = null!;
        public virtual DbSet<TblRevision> TblRevisions { get; set; } = null!;

        public virtual DbSet<TblEmployeeReport> TblEmployeeReports { get; set; } = null!;
        public virtual DbSet<TblKra> TblKras { get; set; } = null!;
        public virtual DbSet<TblKraEmployee> TblKraEmployees { get; set; } = null!;
        public virtual DbSet<tblKraDescription> tblKraDescriptions { get; set; } = null!;
        public virtual DbSet<TblUsertoken> TblUsertokens { get; set; } = null!;
        public virtual DbSet<AttachDocument> AttachDocuments { get; set; } = null!;
        public virtual DbSet<TblReporting> TblReportings { get; set; } = null!;

        public virtual DbSet<Kra> Kras { get; set; } = null!;
        public virtual DbSet<TblDescription> TblDescriptions { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<TblApproval>(entity =>
            {
                entity.HasKey(e => e.ApprovalId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_approvals");

                entity.HasIndex(e => e.ApprovedBy, "ApprovedBy");

                entity.HasIndex(e => e.CreatedBy, "CreatedBy");

                entity.HasIndex(e => e.RequestedBy, "RequestedBy");

                entity.HasIndex(e => e.TaskId, "TaskId");

                entity.HasIndex(e => e.UpdatedBy, "UpdatedBy");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'Pending'");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.ApprovedByNavigation)
                    .WithMany(p => p.TblApprovalApprovedByNavigations)
                    .HasForeignKey(d => d.ApprovedBy)
                    .HasConstraintName("tbl_approvals_ibfk_3");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblApprovalCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("tbl_approvals_ibfk_4");

                entity.HasOne(d => d.RequestedByNavigation)
                    .WithMany(p => p.TblApprovalRequestedByNavigations)
                    .HasForeignKey(d => d.RequestedBy)
                    .HasConstraintName("tbl_approvals_ibfk_2");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TblApprovals)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("tbl_approvals_ibfk_1");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.TblApprovalUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("tbl_approvals_ibfk_5");
            });

            modelBuilder.Entity<TblCustomer>(entity =>
            {
                entity.ToTable("tbl_customer");

                entity.HasIndex(e => e.Email, "Email")
                    .IsUnique();

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.BussinessName).HasMaxLength(255);

                entity.Property(e => e.Contact).HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.FullName).HasMaxLength(255);

                entity.Property(e => e.Gstno)
                    .HasMaxLength(5000)
                    .HasColumnName("GSTNO");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblDocument>(entity =>
            {
                entity.ToTable("tbl_document");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Description).HasMaxLength(2550);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.ProjectId).HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.Url)
                    .HasMaxLength(5000)
                    .HasColumnName("url");
            });

            modelBuilder.Entity<TblLearningsfromproject>(entity =>
            {
                entity.ToTable("tbl_learningsfromproject");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblMilestone>(entity =>
            {
                entity.HasKey(e => e.MilestoneId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_milestones");

                entity.HasIndex(e => e.CreatedBy, "CreatedBy");

                entity.HasIndex(e => e.ProjectId, "ProjectId");

                entity.HasIndex(e => e.UpdatedBy, "UpdatedBy");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.MilestoneName).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'Pending'");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblMilestoneCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("tbl_milestones_ibfk_2");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TblMilestones)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("tbl_milestones_ibfk_1");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.TblMilestoneUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("tbl_milestones_ibfk_3");
            });

            modelBuilder.Entity<TblNotification>(entity =>
            {
                entity.HasKey(e => e.NotificationId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_notifications");

                entity.HasIndex(e => e.CreatedBy, "CreatedBy");

                entity.HasIndex(e => e.ProjectId, "ProjectId");

                entity.HasIndex(e => e.SubTaskId, "SubTaskId");

                entity.HasIndex(e => e.TaskId, "TaskId");

                entity.HasIndex(e => e.UserId, "UserId");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsRead).HasDefaultValueSql("'0'");

                entity.Property(e => e.Message).HasMaxLength(5000);

                entity.Property(e => e.NotificationFor).HasMaxLength(255);

                entity.Property(e => e.NotificationType).HasMaxLength(255);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblNotificationCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("tbl_notifications_ibfk_5");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TblNotifications)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("tbl_notifications_ibfk_2");

                entity.HasOne(d => d.SubTask)
                    .WithMany(p => p.TblNotifications)
                    .HasForeignKey(d => d.SubTaskId)
                    .HasConstraintName("tbl_notifications_ibfk_4");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TblNotifications)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("tbl_notifications_ibfk_3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblNotificationUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tbl_notifications_ibfk_1");
            });

            modelBuilder.Entity<TblProject>(entity =>
            {
                entity.HasKey(e => e.ProjectId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_projects");

                entity.HasIndex(e => e.CreatedBy, "CreatedBy");

                entity.HasIndex(e => e.CustomerId, "CustomerId");

                entity.HasIndex(e => e.ProjectManager, "ProjectManager");

                entity.HasIndex(e => e.DeparmentId, "DeparmentId");

                entity.HasIndex(e => e.TeamLeadId, "TeamLeadId");

                entity.HasIndex(e => e.UpdatedBy, "UpdatedBy");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Architecture)
                    .HasMaxLength(255)
                    .HasColumnName("ARCHITECTURE");

                entity.Property(e => e.Budget).HasPrecision(10);

                entity.Property(e => e.Contractor)
                    .HasMaxLength(255)
                    .HasColumnName("CONTRACTOR");

                entity.Property(e => e.Country).HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Discription).HasMaxLength(1000);

                entity.Property(e => e.Document).HasMaxLength(5000);

                entity.Property(e => e.HourlyRate).HasPrecision(10);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.Location).HasMaxLength(255);

                entity.Property(e => e.ModellingDetailingFirm)
                    .HasMaxLength(255)
                    .HasColumnName("MODELLING_DETAILING_FIRM");

                entity.Property(e => e.PrecasterFabricator)
                    .HasMaxLength(255)
                    .HasColumnName("PRECASTER_FABRICATOR");

                entity.Property(e => e.ProjectAwardedFrom)
                    .HasMaxLength(255)
                    .HasColumnName("PROJECT_AWARDED_FROM");

                entity.Property(e => e.ProjectName).HasMaxLength(255);

                entity.Property(e => e.ProjectNumber).HasMaxLength(255);

                entity.Property(e => e.State).HasMaxLength(255);

                entity.Property(e => e.Status).HasMaxLength(255);

                entity.Property(e => e.StructuralDesigner)
                    .HasMaxLength(255)
                    .HasColumnName("STRUCTURAL_DESIGNER");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblProjectCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("tbl_projects_ibfk_3");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TblProjects)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("tbl_projects_ibfk_2");

                entity.HasOne(d => d.ProjectManagerNavigation)
                    .WithMany(p => p.TblProjectProjectManagerNavigations)
                    .HasForeignKey(d => d.ProjectManager)
                    .HasConstraintName("tbl_projects_ibfk_1");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.TblProjectUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("tbl_projects_ibfk_4");
            });

            modelBuilder.Entity<TblRevisionlog>(entity =>
            {
                entity.ToTable("tbl_revisionlogs");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Description).HasMaxLength(5000);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblSubtask>(entity =>
            {
                entity.HasKey(e => e.SubTaskId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_subtasks");

                entity.HasIndex(e => e.AssignedTo, "AssignedTo");

                entity.HasIndex(e => e.CreatedBy, "CreatedBy");

                entity.HasIndex(e => e.TaskId, "TaskId");

                entity.HasIndex(e => e.UpdatedBy, "UpdatedBy");

                entity.Property(e => e.ActualHours).HasDefaultValueSql("'0'");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Description).HasMaxLength(2550);

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.IsMainTask).HasDefaultValueSql("'0'");

                entity.Property(e => e.Priority)
                    .HasMaxLength(255)
                    .HasColumnName("priority")
                    .HasDefaultValueSql("'Low'");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'Pending'");

                entity.Property(e => e.SubTaskName).HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.AssignedToNavigation)
                    .WithMany(p => p.TblSubtaskAssignedToNavigations)
                    .HasForeignKey(d => d.AssignedTo)
                    .HasConstraintName("tbl_subtasks_ibfk_2");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblSubtaskCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("tbl_subtasks_ibfk_3");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TblSubtasks)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("tbl_subtasks_ibfk_1");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.TblSubtaskUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("tbl_subtasks_ibfk_4");
            });

            modelBuilder.Entity<TblTask>(entity =>
            {
                entity.HasKey(e => e.TaskId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_tasks");

                entity.HasIndex(e => e.AssignedTo, "AssignedTo");

                entity.HasIndex(e => e.CreatedBy, "CreatedBy");

                entity.HasIndex(e => e.ProjectId, "ProjectId");

                entity.HasIndex(e => e.UpdatedBy, "UpdatedBy");

                entity.Property(e => e.Document).HasMaxLength(5000);

                entity.Property(e => e.ActualHours).HasDefaultValueSql("'0'");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Description).HasMaxLength(2550);

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Priority)
                    .HasMaxLength(255)
                    .HasColumnName("Priority")
                    .HasDefaultValueSql("'Low'");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'Pending'");

                entity.Property(e => e.TaskName).HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.AssignedToNavigation)
                    .WithMany(p => p.TblTaskAssignedToNavigations)
                    .HasForeignKey(d => d.AssignedTo)
                    .HasConstraintName("tbl_tasks_ibfk_2");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblTaskCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("tbl_tasks_ibfk_3");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TblTasks)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("tbl_tasks_ibfk_1");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.TblTaskUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("tbl_tasks_ibfk_4");
            });

            modelBuilder.Entity<TblTaskmapping>(entity =>
            {
                entity.ToTable("tbl_taskmappings");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblTimesheet>(entity =>
            {
                entity.HasKey(e => e.TimesheetId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_timesheets");

                entity.HasIndex(e => e.CreatedBy, "CreatedBy");

                entity.HasIndex(e => e.TaskId, "TaskId");

                entity.HasIndex(e => e.UpdatedBy, "UpdatedBy");

                entity.HasIndex(e => e.UserId, "UserId");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TblTimesheetCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("tbl_timesheets_ibfk_3");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TblTimesheets)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("tbl_timesheets_ibfk_1");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.TblTimesheetUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("tbl_timesheets_ibfk_4");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblTimesheetUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tbl_timesheets_ibfk_2");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_users");

                entity.HasIndex(e => e.Email, "Email")
                    .IsUnique();

                entity.HasIndex(e => e.Role, "Role");

                entity.Property(e => e.Contact).HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.FullName).HasMaxLength(255);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.PasswordHash).HasMaxLength(255);

                entity.Property(e => e.Profile).HasMaxLength(5000);

                entity.Property(e => e.Responsibilities).HasMaxLength(255);

                entity.Property(e => e.Status).HasDefaultValueSql("'0'");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.TblUsers)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("tbl_users_ibfk_1");
            });

            modelBuilder.Entity<TblUsersrole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_usersrole");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.Role).HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblDepartment>(entity =>
            {
                entity.HasKey(e => e.DepartmentId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_department");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DepartmentName).HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });


            modelBuilder.Entity<TblRevision>(entity =>
            {
                entity.HasKey(e => e.RevisionId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_revision");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Document).HasMaxLength(5000);

                entity.Property(e => e.ExecutedBy).HasMaxLength(100);

                entity.Property(e => e.ManHours).HasMaxLength(45);

                entity.Property(e => e.SubId).HasMaxLength(45);

                entity.Property(e => e.Reason).HasMaxLength(2000);

                entity.Property(e => e.ReceivedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.Property(e => e.RevisionBy).HasMaxLength(100);

                entity.Property(e => e.ScopeOfWork).HasMaxLength(2000);

                entity.Property(e => e.SubmittedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblKra>(entity =>
            {
                entity.ToTable("tbl_kra");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(45);

                entity.Property(e => e.FkUserId).HasColumnName("fk_UserId");

                entity.Property(e => e.IsDelete).HasColumnName("isDelete");

                entity.Property(e => e.IsTarget).HasColumnName("isTarget");

                entity.Property(e => e.KraEndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("kraEndDate");

                entity.Property(e => e.KraName)
                    .HasMaxLength(250)
                    .HasColumnName("kra_Name");

                entity.Property(e => e.KraStartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("kraStartDate");

                entity.Property(e => e.Quarter).HasColumnName("quarter");
            });

            modelBuilder.Entity<TblKraEmployee>(entity =>
            {
                entity.ToTable("tbl_kra_employee");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.FkEmployeeId).HasColumnName("fk_employeeId");

                entity.Property(e => e.FkKraId).HasColumnName("fk_KraId");

                entity.Property(e => e.KraGeneratedpersonId).HasColumnName("kra_generatedpersonId");

                entity.Property(e => e.KraIsSubmit).HasColumnName("kra_isSubmit");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(45);
            });

           

            modelBuilder.Entity<TblEmployeeReport>(entity =>
            {
                entity.ToTable("tbl_employee_report");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AchivedDeadline)
                    .HasMaxLength(45)
                    .HasColumnName("Achived_Deadline");

                entity.Property(e => e.AchivedQuantity).HasColumnName("Achived_Quantity");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(45);

                entity.Property(e => e.EmpComment)
                    .HasMaxLength(250)
                    .HasColumnName("Emp_Comment");

                entity.Property(e => e.EmpRating).HasColumnName("Emp_Rating");

                entity.Property(e => e.FinalRating).HasColumnName("Final_Rating");

                entity.Property(e => e.FkDescriptionId).HasColumnName("fk_DescriptionId");

                entity.Property(e => e.ReviewerComment)
                    .HasMaxLength(250)
                    .HasColumnName("Reviewer_Comment");

                entity.Property(e => e.ReviewerRating).HasColumnName("Reviewer_Rating");

                entity.Property(e => e.TargetDeadline)
                    .HasMaxLength(45)
                    .HasColumnName("Target_Deadline");

                entity.Property(e => e.TargetQuantity).HasColumnName("Target_Quantity");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(45);
            });

            modelBuilder.Entity<tblKraDescription>(entity =>
            {
                entity.ToTable("tbl_kradescription");

                entity.HasIndex(e => e.KraIdFk, "KraId_Fk_idx");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.KraIdFk).HasColumnName("KraId_Fk");

                entity.HasOne(d => d.KraIdFkNavigation)
                    .WithMany(p => p.TblKradescriptions)
                    .HasForeignKey(d => d.KraIdFk)
                    .HasConstraintName("KraId_Fk");
            });

            modelBuilder.Entity<TblUsertoken>(entity =>
            {
                entity.ToTable("tbl_usertoken");

                entity.HasIndex(e => e.UserId, "UserId_idx");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.IsDelete).HasColumnName("isDelete");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblUsertokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("UserId");
            });

            modelBuilder.Entity<AttachDocument>(entity =>
            {
                entity.ToTable("attach_documents");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DocumentName).HasMaxLength(5000);

                entity.Property(e => e.Flag).HasMaxLength(45);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblReporting>(entity =>
            {
                entity.ToTable("tbl_reporting");

                entity.Property(e => e.ReportingPerson).HasColumnName("Reporting_Person");
                entity.Property(e => e.FkUserId).HasColumnName("FkUserId");
            });


            modelBuilder.Entity<Kra>(entity =>
            {
                entity.ToTable("kra");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(45);

                entity.Property(e => e.FkUserId).HasColumnName("fk_UserId");

                entity.Property(e => e.IsDelete)
                    .HasColumnName("isDelete")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IsTarget).HasColumnName("isTarget");

                entity.Property(e => e.KraEndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("kraEndDate");

                entity.Property(e => e.KraName)
                    .HasMaxLength(250)
                    .HasColumnName("kra_Name");

                entity.Property(e => e.KraStartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("kraStartDate");

                entity.Property(e => e.Quarter).HasColumnName("quarter");
            });

            modelBuilder.Entity<TblDescription>(entity =>
            {
                entity.ToTable("tbl_description");

                entity.HasIndex(e => e.KraIdfk, "KraIDFK_idx");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.KraIdfk).HasColumnName("KraIDFK");

                entity.HasOne(d => d.KraIdfkNavigation)
                    .WithMany(p => p.TblDescriptions)
                    .HasForeignKey(d => d.KraIdfk)
                    .HasConstraintName("KraIDFK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
