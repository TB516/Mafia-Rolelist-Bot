using DSharpPlus.SlashCommands;

namespace Mafia_Bot.RoleDeckComponents.InteractionPrechecks
{
    internal class MessageResponseFromAttribute(string commandName) : ContextMenuCheckBaseAttribute
    {
        public override Task<bool> ExecuteChecksAsync(ContextMenuContext ctx)
        {
            if (ctx.TargetMessage.Interaction != null && ctx.TargetMessage.ApplicationId == ctx.Client.CurrentApplication.Id && ctx.TargetMessage.Interaction.Name == commandName)
            {
                return Task.FromResult(true);
            }
            else
            {
                ctx.CreateResponseAsync("That is not a valid command response for this action to be performed on!", true);
                return Task.FromResult(false);
            }
        }
    }
}
