using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ENGHelperBot.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "examples",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WordId = table.Column<long>(type: "bigint", nullable: false),
                    original = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    translation = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_examples", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "phrases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WordId = table.Column<long>(type: "bigint", nullable: false),
                    original = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    translation = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phrases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "translation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_translation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "words",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    transcription = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_words", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dictionaries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dictionaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dictionaries_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "words_translations",
                columns: table => new
                {
                    word_id = table.Column<long>(type: "bigint", nullable: false),
                    translation_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_words_translations", x => new { x.word_id, x.translation_id });
                    table.ForeignKey(
                        name: "FK_words_translations_translation_translation_id",
                        column: x => x.translation_id,
                        principalTable: "translation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_words_translations_words_word_id",
                        column: x => x.word_id,
                        principalTable: "words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dictionaries_words",
                columns: table => new
                {
                    dictionary_id = table.Column<long>(type: "bigint", nullable: false),
                    word_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dictionaries_words", x => new { x.dictionary_id, x.word_id });
                    table.ForeignKey(
                        name: "FK_dictionaries_words_dictionaries_dictionary_id",
                        column: x => x.dictionary_id,
                        principalTable: "dictionaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dictionaries_words_words_word_id",
                        column: x => x.word_id,
                        principalTable: "words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dictionaries_UserId",
                table: "dictionaries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_dictionaries_words_word_id",
                table: "dictionaries_words",
                column: "word_id");

            migrationBuilder.CreateIndex(
                name: "IX_words_translations_translation_id",
                table: "words_translations",
                column: "translation_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dictionaries_words");

            migrationBuilder.DropTable(
                name: "examples");

            migrationBuilder.DropTable(
                name: "phrases");

            migrationBuilder.DropTable(
                name: "words_translations");

            migrationBuilder.DropTable(
                name: "dictionaries");

            migrationBuilder.DropTable(
                name: "translation");

            migrationBuilder.DropTable(
                name: "words");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
