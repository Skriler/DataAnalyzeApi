using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAnalyzeApi.Migrations
{
    /// <inheritdoc />
    public partial class DataObjectCoordinateEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataObjectCoordinates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    X = table.Column<double>(type: "double precision", nullable: false),
                    Y = table.Column<double>(type: "double precision", nullable: false),
                    ObjectId = table.Column<long>(type: "bigint", nullable: false),
                    ClusteringAnalysisResultId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataObjectCoordinates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataObjectCoordinates_ClusteringAnalysisResults_ClusteringA~",
                        column: x => x.ClusteringAnalysisResultId,
                        principalTable: "ClusteringAnalysisResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataObjectCoordinates_DataObjects_ObjectId",
                        column: x => x.ObjectId,
                        principalTable: "DataObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataObjectCoordinates_ClusteringAnalysisResultId",
                table: "DataObjectCoordinates",
                column: "ClusteringAnalysisResultId");

            migrationBuilder.CreateIndex(
                name: "IX_DataObjectCoordinates_ObjectId",
                table: "DataObjectCoordinates",
                column: "ObjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataObjectCoordinates");
        }
    }
}
