using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAnalyzeApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalysisResultEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "AnalysisResultSequence");

            migrationBuilder.CreateTable(
                name: "ClusteringAnalysisResults",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"AnalysisResultSequence\"')"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequestHash = table.Column<string>(type: "text", nullable: false),
                    DatasetId = table.Column<long>(type: "bigint", nullable: false),
                    Algorithm = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClusteringAnalysisResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClusteringAnalysisResults_Datasets_DatasetId",
                        column: x => x.DatasetId,
                        principalTable: "Datasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SimilarityAnalysisResults",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"AnalysisResultSequence\"')"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequestHash = table.Column<string>(type: "text", nullable: false),
                    DatasetId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimilarityAnalysisResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimilarityAnalysisResults_Datasets_DatasetId",
                        column: x => x.DatasetId,
                        principalTable: "Datasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clusters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClusteringAnalysisResultId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clusters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clusters_ClusteringAnalysisResults_ClusteringAnalysisResult~",
                        column: x => x.ClusteringAnalysisResultId,
                        principalTable: "ClusteringAnalysisResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SimilarityPairs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ObjectAId = table.Column<long>(type: "bigint", nullable: false),
                    ObjectBId = table.Column<long>(type: "bigint", nullable: false),
                    SimilarityPercentage = table.Column<double>(type: "double precision", nullable: false),
                    SimilarityAnalysisResultId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimilarityPairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimilarityPairs_DataObjects_ObjectAId",
                        column: x => x.ObjectAId,
                        principalTable: "DataObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SimilarityPairs_DataObjects_ObjectBId",
                        column: x => x.ObjectBId,
                        principalTable: "DataObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SimilarityPairs_SimilarityAnalysisResults_SimilarityAnalysi~",
                        column: x => x.SimilarityAnalysisResultId,
                        principalTable: "SimilarityAnalysisResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClusterDataObjects",
                columns: table => new
                {
                    ClusterId = table.Column<long>(type: "bigint", nullable: false),
                    ObjectsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClusterDataObjects", x => new { x.ClusterId, x.ObjectsId });
                    table.ForeignKey(
                        name: "FK_ClusterDataObjects_Clusters_ClusterId",
                        column: x => x.ClusterId,
                        principalTable: "Clusters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClusterDataObjects_DataObjects_ObjectsId",
                        column: x => x.ObjectsId,
                        principalTable: "DataObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClusterDataObjects_ObjectsId",
                table: "ClusterDataObjects",
                column: "ObjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_ClusteringAnalysisResults_DatasetId",
                table: "ClusteringAnalysisResults",
                column: "DatasetId");

            migrationBuilder.CreateIndex(
                name: "IX_ClusteringAnalysisResults_RequestHash",
                table: "ClusteringAnalysisResults",
                column: "RequestHash");

            migrationBuilder.CreateIndex(
                name: "IX_Clusters_ClusteringAnalysisResultId",
                table: "Clusters",
                column: "ClusteringAnalysisResultId");

            migrationBuilder.CreateIndex(
                name: "IX_SimilarityAnalysisResults_DatasetId",
                table: "SimilarityAnalysisResults",
                column: "DatasetId");

            migrationBuilder.CreateIndex(
                name: "IX_SimilarityAnalysisResults_RequestHash",
                table: "SimilarityAnalysisResults",
                column: "RequestHash");

            migrationBuilder.CreateIndex(
                name: "IX_SimilarityPairs_ObjectAId",
                table: "SimilarityPairs",
                column: "ObjectAId");

            migrationBuilder.CreateIndex(
                name: "IX_SimilarityPairs_ObjectBId",
                table: "SimilarityPairs",
                column: "ObjectBId");

            migrationBuilder.CreateIndex(
                name: "IX_SimilarityPairs_SimilarityAnalysisResultId",
                table: "SimilarityPairs",
                column: "SimilarityAnalysisResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClusterDataObjects");

            migrationBuilder.DropTable(
                name: "SimilarityPairs");

            migrationBuilder.DropTable(
                name: "Clusters");

            migrationBuilder.DropTable(
                name: "SimilarityAnalysisResults");

            migrationBuilder.DropTable(
                name: "ClusteringAnalysisResults");

            migrationBuilder.DropSequence(
                name: "AnalysisResultSequence");
        }
    }
}
