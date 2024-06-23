using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autoflow.Portal.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_ReceiveUserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_SendUserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConversationMaps_Conversations_ConversationId",
                table: "UserConversationMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConversationMaps_Users_UserId",
                table: "UserConversationMaps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Conversations",
                table: "Conversations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserConversationMaps",
                table: "UserConversationMaps");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "messages");

            migrationBuilder.RenameTable(
                name: "Conversations",
                newName: "conversations");

            migrationBuilder.RenameTable(
                name: "UserConversationMaps",
                newName: "user_conversation_maps");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_SendUserId",
                table: "messages",
                newName: "IX_messages_SendUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ReceiveUserId",
                table: "messages",
                newName: "IX_messages_ReceiveUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ConversationId",
                table: "messages",
                newName: "IX_messages_ConversationId");

            migrationBuilder.RenameIndex(
                name: "IX_UserConversationMaps_ConversationId",
                table: "user_conversation_maps",
                newName: "IX_user_conversation_maps_ConversationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_messages",
                table: "messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_conversations",
                table: "conversations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_conversation_maps",
                table: "user_conversation_maps",
                columns: new[] { "UserId", "ConversationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_messages_conversations_ConversationId",
                table: "messages",
                column: "ConversationId",
                principalTable: "conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_messages_users_ReceiveUserId",
                table: "messages",
                column: "ReceiveUserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_messages_users_SendUserId",
                table: "messages",
                column: "SendUserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_conversation_maps_conversations_ConversationId",
                table: "user_conversation_maps",
                column: "ConversationId",
                principalTable: "conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_conversation_maps_users_UserId",
                table: "user_conversation_maps",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_messages_conversations_ConversationId",
                table: "messages");

            migrationBuilder.DropForeignKey(
                name: "FK_messages_users_ReceiveUserId",
                table: "messages");

            migrationBuilder.DropForeignKey(
                name: "FK_messages_users_SendUserId",
                table: "messages");

            migrationBuilder.DropForeignKey(
                name: "FK_user_conversation_maps_conversations_ConversationId",
                table: "user_conversation_maps");

            migrationBuilder.DropForeignKey(
                name: "FK_user_conversation_maps_users_UserId",
                table: "user_conversation_maps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_messages",
                table: "messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_conversations",
                table: "conversations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_conversation_maps",
                table: "user_conversation_maps");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "messages",
                newName: "Messages");

            migrationBuilder.RenameTable(
                name: "conversations",
                newName: "Conversations");

            migrationBuilder.RenameTable(
                name: "user_conversation_maps",
                newName: "UserConversationMaps");

            migrationBuilder.RenameIndex(
                name: "IX_messages_SendUserId",
                table: "Messages",
                newName: "IX_Messages_SendUserId");

            migrationBuilder.RenameIndex(
                name: "IX_messages_ReceiveUserId",
                table: "Messages",
                newName: "IX_Messages_ReceiveUserId");

            migrationBuilder.RenameIndex(
                name: "IX_messages_ConversationId",
                table: "Messages",
                newName: "IX_Messages_ConversationId");

            migrationBuilder.RenameIndex(
                name: "IX_user_conversation_maps_ConversationId",
                table: "UserConversationMaps",
                newName: "IX_UserConversationMaps_ConversationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Conversations",
                table: "Conversations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserConversationMaps",
                table: "UserConversationMaps",
                columns: new[] { "UserId", "ConversationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_ReceiveUserId",
                table: "Messages",
                column: "ReceiveUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_SendUserId",
                table: "Messages",
                column: "SendUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConversationMaps_Conversations_ConversationId",
                table: "UserConversationMaps",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConversationMaps_Users_UserId",
                table: "UserConversationMaps",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
