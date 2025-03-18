using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendaManager.Infrastructure.Common.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "OutboxMessages",
                newName: "MessageStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MessageStatus",
                table: "OutboxMessages",
                newName: "Status");
        }
    }
}
