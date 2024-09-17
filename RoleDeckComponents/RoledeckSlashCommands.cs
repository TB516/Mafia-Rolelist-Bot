using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Mafia_Bot.RoleDeckComponents.InteractionPrechecks;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Mafia_Bot.RoleDeckComponents
{
    internal class RoledeckSlashCommands : ApplicationCommandModule
    {
        [SlashCommand("help", "Lists all commands and their use.")]
        public async Task Help(InteractionContext ctx)
        {
            ctx.CreateResponseAsync("```" +
                "/help (posts this message)\n" +
                "/post <json> (formats and posts a gamemode using exported json)\n" +
                "/delete <link> (deletes a posted gamemode using a link to that message, you can also use an app context menu to delete it)\n" +
                "```", true);
        }

        [SlashCommand("post", "Posts a formatted gamemode to the current channel.")]
        public async Task PostList(InteractionContext ctx, [Option("JSON", "JSON data of the gamemode.")] string json)
        {
            await ctx.DeferAsync(true);

            json = Regex.Replace(json, @"\\t|\t|\\n|\n|\\r|\r", string.Empty);

            RoledeckMessage message;

            try
            {
                JObject jsonNode = JObject.Parse(json)!;
                message = new(ctx.Member.DisplayName, jsonNode);
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("JSON was not valid!"));
                return;
            }
            catch (Exception e)
            {
                ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(e.Message));
                return;
            }

            message.SendRoledeck(ctx);
            ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Posted gamemode!"));
        }

        [ContextMenu(DSharpPlus.ApplicationCommandType.MessageContextMenu, "Delete Gamemode")]
        [MessageResponseFrom("post")]
        public async Task DeleteList(ContextMenuContext ctx)
        {
            if (ctx.TargetMessage.Interaction.User != ctx.User)
            {
                ctx.CreateResponseAsync("That message wasn't made by you!", true);
                return;
            }

            await ctx.Channel.DeleteMessageAsync(ctx.TargetMessage);
            ctx.CreateResponseAsync("Deleted gamemode!", true);
        }
    }
}
