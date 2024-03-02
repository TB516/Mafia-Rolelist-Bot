using DSharpPlus.SlashCommands;
using System.Text.Json.Nodes;

namespace Mafia_Bot.RoleDeckComponents
{
    internal class RoledeckSlashCommands : ApplicationCommandModule
    {
        [SlashCommand("post", "Stores a role deck for future use")]
        public async Task PostList(InteractionContext ctx, [Option("JSON", "JSON data of the role deck")] string json)
        {
            JsonNode jsonNode;
            try
            {
                jsonNode = JsonNode.Parse(json)!;
            }
            catch
            {
                await ctx.CreateResponseAsync("JSON was not valid!", true);
                return;
            }

            RoledeckMessage message = new(ctx.Member.Nickname, jsonNode);
            message.SendRoledeck(ctx);
        }
    }
}
