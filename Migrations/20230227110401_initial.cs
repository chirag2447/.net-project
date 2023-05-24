using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace communityWeb.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    uName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    email_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Awards",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    description = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    price = table.Column<double>(type: "float", nullable: true),
                    image = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Awards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    email_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    state = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    contactNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    registeredDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User_award_balance",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    award_id = table.Column<int>(type: "int", nullable: true),
                    balance = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_award_balance", x => x.id);
                    table.ForeignKey(
                        name: "FK_User_award_balance_Awards",
                        column: x => x.award_id,
                        principalTable: "Awards",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Communities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    owner_id = table.Column<int>(type: "int", nullable: true),
                    logo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isApproved = table.Column<bool>(type: "bit", nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_communities", x => x.id);
                    table.ForeignKey(
                        name: "FK_communities_Users",
                        column: x => x.owner_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Friend_requests",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sender_id = table.Column<int>(type: "int", nullable: true),
                    receiver_id = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    sentDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requests", x => x.id);
                    table.ForeignKey(
                        name: "FK_Friend_requests_Users",
                        column: x => x.sender_id,
                        principalTable: "Users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Friend_requests_Users1",
                        column: x => x.receiver_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "User_award_purchase",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    award_id = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    totalAmount = table.Column<double>(type: "float", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_award_purchase", x => x.id);
                    table.ForeignKey(
                        name: "FK_User_award_purchase_Awards",
                        column: x => x.award_id,
                        principalTable: "Awards",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_User_award_purchase_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Community_members",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    community_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_members", x => x.id);
                    table.ForeignKey(
                        name: "FK_community_members_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_community_members_communities",
                        column: x => x.community_id,
                        principalTable: "Communities",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Community_request",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    community_id = table.Column<int>(type: "int", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    sentByUser_id = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_request", x => x.id);
                    table.ForeignKey(
                        name: "FK_community_request_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_community_request_Users1",
                        column: x => x.sentByUser_id,
                        principalTable: "Users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_community_request_communities",
                        column: x => x.community_id,
                        principalTable: "Communities",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    community_id = table.Column<int>(type: "int", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fileName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Posts_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Posts_communities",
                        column: x => x.community_id,
                        principalTable: "Communities",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Post_awards",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<int>(type: "int", nullable: true),
                    award_id = table.Column<int>(type: "int", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    givenDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post_awards", x => x.id);
                    table.ForeignKey(
                        name: "FK_Post_awards_Awards",
                        column: x => x.award_id,
                        principalTable: "Awards",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Post_awards_Posts",
                        column: x => x.post_id,
                        principalTable: "Posts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Post_awards_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Post_feedback",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<int>(type: "int", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    review = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Post_feedback_Posts",
                        column: x => x.post_id,
                        principalTable: "Posts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Post_feedback_Users",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Communities_owner_id",
                table: "Communities",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_Community_members_community_id",
                table: "Community_members",
                column: "community_id");

            migrationBuilder.CreateIndex(
                name: "IX_Community_members_user_id",
                table: "Community_members",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Community_request_community_id",
                table: "Community_request",
                column: "community_id");

            migrationBuilder.CreateIndex(
                name: "IX_Community_request_sentByUser_id",
                table: "Community_request",
                column: "sentByUser_id");

            migrationBuilder.CreateIndex(
                name: "IX_Community_request_user_id",
                table: "Community_request",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_requests_receiver_id",
                table: "Friend_requests",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_requests_sender_id",
                table: "Friend_requests",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_Post_awards_award_id",
                table: "Post_awards",
                column: "award_id");

            migrationBuilder.CreateIndex(
                name: "IX_Post_awards_post_id",
                table: "Post_awards",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_Post_awards_user_id",
                table: "Post_awards",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Post_feedback_post_id",
                table: "Post_feedback",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_Post_feedback_user_id",
                table: "Post_feedback",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_community_id",
                table: "Posts",
                column: "community_id");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_user_id",
                table: "Posts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_award_balance_award_id",
                table: "User_award_balance",
                column: "award_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_award_purchase_award_id",
                table: "User_award_purchase",
                column: "award_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_award_purchase_user_id",
                table: "User_award_purchase",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Community_members");

            migrationBuilder.DropTable(
                name: "Community_request");

            migrationBuilder.DropTable(
                name: "Friend_requests");

            migrationBuilder.DropTable(
                name: "Post_awards");

            migrationBuilder.DropTable(
                name: "Post_feedback");

            migrationBuilder.DropTable(
                name: "User_award_balance");

            migrationBuilder.DropTable(
                name: "User_award_purchase");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Awards");

            migrationBuilder.DropTable(
                name: "Communities");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
