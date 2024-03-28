using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using DSharpPlus.SlashCommands;
using Mafia_Bot.RoleDeckComponents.InteractionPrechecks;
using Newtonsoft.Json.Linq;

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
            JObject jsonNode;
            RoledeckMessage message;
            try
            {
                jsonNode = JObject.Parse(json)!;
                message = new(ctx.Member.Nickname, jsonNode);
            }
            catch
            {
                ctx.CreateResponseAsync("Invalid gamemode data entered!", true);
                return;
            }
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

        [SlashCommand("delete", "Deletes a gamemode posting if created by you.")]
        public async Task DeleteList(InteractionContext ctx, [Option("Gamemode", "Link to gamemode to delete.")] string link)
        {
            if (!ulong.TryParse(link.Split('/')[^1], out ulong id))
            {
                ctx.CreateResponseAsync("Thats not a valid message link!", true);
                return;
            }

            DiscordMessage message;
            try
            {
                 message = await ctx.Channel.GetMessageAsync(id);
            }
            catch (UnauthorizedException)
            {
                ctx.CreateResponseAsync("The bot doesnt have permissions to access that message!", true);
                return;
            }
            catch (NotFoundException)
            {
                ctx.CreateResponseAsync("The requested message was not found!", true);
                return;
            }
            catch (Exception e)
            {
                ctx.CreateResponseAsync($"There was an error accessing the message: {e.Message}", true);
                return;
            }

            if (!(message.Interaction != null && message.Author.IsCurrent && message.Interaction.Name == "post"))
            {
                ctx.CreateResponseAsync("That is not a valid gamemode posting!", true);
                return;
            }
            else if (ctx.Interaction.User.Id != ctx.User.Id)
            {
                ctx.CreateResponseAsync("You cannot delete someone else's gamemode posting!");
                return;
            }

            try
            {
                ctx.Channel.DeleteMessageAsync(message);
                ctx.CreateResponseAsync("Deleted gamemode post!", true);
            }
            catch (UnauthorizedException)
            {
                ctx.CreateResponseAsync("The bot doesnt have permissions to access that message!", true);
                return;
            }
            catch (NotFoundException)
            {
                ctx.CreateResponseAsync("The requested message was not found!", true);
                return;
            }
            catch (Exception e)
            {
                ctx.CreateResponseAsync($"There was an error accessing the message: {e.Message}", true);
                return;
            }
        }
    }
}
