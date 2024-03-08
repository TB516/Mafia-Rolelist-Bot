using DSharpPlus.SlashCommands;

namespace Mafia_Bot.RoleDeckComponents.InteractionPrechecks
{
    internal class UserIsInteractionInvoker : ContextMenuCheckBaseAttribute
    {
        public override Task<bool> ExecuteChecksAsync(ContextMenuContext ctx)
        {
            if (ctx.Interaction.User == ctx.User)
            {
                return Task.FromResult(true);
            }
            else
            {
                ctx.CreateResponseAsync("That message wasn't made by you!", true);
                return Task.FromResult(false);
            }
            
        }
    }
}
