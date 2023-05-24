using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace communityWeb.Models;

public partial class ProjectContext : DbContext
{
    public ProjectContext()
    {
    }

    public ProjectContext(DbContextOptions<ProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Award> Awards { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Community> Communities { get; set; }

    public virtual DbSet<CommunityMember> CommunityMembers { get; set; }

    public virtual DbSet<CommunityRequest> CommunityRequests { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<FriendRequest> FriendRequests { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostAward> PostAwards { get; set; }

    public virtual DbSet<PostFeedback> PostFeedbacks { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAwardBalance> UserAwardBalances { get; set; }

    public virtual DbSet<UserAwardPurchase> UserAwardPurchases { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-J5LUU4KR;Database=project;Trusted_Connection=True;TrustServerCertificate=True;Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.ToTable("Admin");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmailId)
                .HasMaxLength(50)
                .HasColumnName("email_id");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.UName)
                .HasMaxLength(50)
                .HasColumnName("uName");
        });

        modelBuilder.Entity<Award>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("description");
            entity.Property(e => e.Image)
                .HasMaxLength(50)
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.StateId).HasColumnName("state_id");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_Cities_States");
        });

        modelBuilder.Entity<Community>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_communities");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.IsApproved).HasColumnName("isApproved");
            entity.Property(e => e.Logo)
                .HasMaxLength(50)
                .HasColumnName("logo");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");

            entity.HasOne(d => d.Owner).WithMany(p => p.Communities)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK_communities_Users");
        });

        modelBuilder.Entity<CommunityMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_community_members");

            entity.ToTable("Community_members");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CommunityId).HasColumnName("community_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Community).WithMany(p => p.CommunityMembers)
                .HasForeignKey(d => d.CommunityId)
                .HasConstraintName("FK_community_members_communities");

            entity.HasOne(d => d.User).WithMany(p => p.CommunityMembers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_community_members_Users");
        });

        modelBuilder.Entity<CommunityRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_community_request");

            entity.ToTable("Community_request");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CommunityId).HasColumnName("community_id");
            entity.Property(e => e.SentByUserId).HasColumnName("sentByUser_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Community).WithMany(p => p.CommunityRequests)
                .HasForeignKey(d => d.CommunityId)
                .HasConstraintName("FK_community_request_communities");

            entity.HasOne(d => d.SentByUser).WithMany(p => p.CommunityRequestSentByUsers)
                .HasForeignKey(d => d.SentByUserId)
                .HasConstraintName("FK_community_request_Users1");

            entity.HasOne(d => d.User).WithMany(p => p.CommunityRequestUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_community_request_Users");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<FriendRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_requests");

            entity.ToTable("Friend_requests");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ReceiverId).HasColumnName("receiver_id");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");
            entity.Property(e => e.SentDate)
                .HasColumnType("datetime")
                .HasColumnName("sentDate");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Receiver).WithMany(p => p.FriendRequestReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .HasConstraintName("FK_Friend_requests_Users1");

            entity.HasOne(d => d.Sender).WithMany(p => p.FriendRequestSenders)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK_Friend_requests_Users");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CommunityId).HasColumnName("community_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.FileName)
                .HasMaxLength(50)
                .HasColumnName("fileName");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Community).WithMany(p => p.Posts)
                .HasForeignKey(d => d.CommunityId)
                .HasConstraintName("FK_Posts_communities");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Posts_Users");
        });

        modelBuilder.Entity<PostAward>(entity =>
        {
            entity.ToTable("Post_awards");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AwardId).HasColumnName("award_id");
            entity.Property(e => e.GivenDate)
                .HasColumnType("datetime")
                .HasColumnName("givenDate");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Award).WithMany(p => p.PostAwards)
                .HasForeignKey(d => d.AwardId)
                .HasConstraintName("FK_Post_awards_Awards");

            entity.HasOne(d => d.Post).WithMany(p => p.PostAwards)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK_Post_awards_Posts");

            entity.HasOne(d => d.User).WithMany(p => p.PostAwards)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Post_awards_Users");
        });

        modelBuilder.Entity<PostFeedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Comments");

            entity.ToTable("Post_feedback");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.Review)
                .HasMaxLength(50)
                .HasColumnName("review");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Post).WithMany(p => p.PostFeedbacks)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK_Post_feedback_Posts");

            entity.HasOne(d => d.User).WithMany(p => p.PostFeedbacks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Post_feedback_Users");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_States_States");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.ContactNo)
                .HasMaxLength(50)
                .HasColumnName("contactNo");
            entity.Property(e => e.EmailId)
                .HasMaxLength(50)
                .HasColumnName("email_id");
            entity.Property(e => e.Fname)
                .HasMaxLength(50)
                .HasColumnName("FName");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.Lname)
                .HasMaxLength(50)
                .HasColumnName("LName");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.RegisteredDate)
                .HasColumnType("datetime")
                .HasColumnName("registeredDate");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .HasColumnName("state");
        });

        modelBuilder.Entity<UserAwardBalance>(entity =>
        {
            entity.ToTable("User_award_balance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AwardId).HasColumnName("award_id");
            entity.Property(e => e.Balance).HasColumnName("balance");

            entity.HasOne(d => d.Award).WithMany(p => p.UserAwardBalances)
                .HasForeignKey(d => d.AwardId)
                .HasConstraintName("FK_User_award_balance_Awards");
        });

        modelBuilder.Entity<UserAwardPurchase>(entity =>
        {
            entity.ToTable("User_award_purchase");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AwardId).HasColumnName("award_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TotalAmount).HasColumnName("totalAmount");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Award).WithMany(p => p.UserAwardPurchases)
                .HasForeignKey(d => d.AwardId)
                .HasConstraintName("FK_User_award_purchase_Awards");

            entity.HasOne(d => d.User).WithMany(p => p.UserAwardPurchases)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_User_award_purchase_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
