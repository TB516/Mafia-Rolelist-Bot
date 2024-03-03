using DSharpPlus.SlashCommands;
using Newtonsoft.Json.Linq;

namespace Mafia_Bot.RoleDeckComponents
{
    internal class RoledeckSlashCommands : ApplicationCommandModule
    {
        [SlashCommand("post", "Stores a role deck for future use")]
        public async Task PostList(InteractionContext ctx, [Option("JSON", "JSON data of the role deck")] string json)
        {
            JObject jsonNode;
            RoledeckMessage message;
            try
            {
                jsonNode = JObject.Parse(json)!;
                message = new(ctx.Member.Nickname, jsonNode);
            }
            catch
            {
                await ctx.CreateResponseAsync("JSON was not valid!", true);
                return;
            }
            message.SendRoledeck(ctx);
        }
    }
}
