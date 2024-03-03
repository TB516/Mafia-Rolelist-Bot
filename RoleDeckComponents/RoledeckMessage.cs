using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Mafia_Bot.RoleDeckComponents
{
    /// <summary>
    /// Discord message containing rolelist
    /// </summary>
    public class RoledeckMessage
    {
        private readonly Roledeck _deck;
        private readonly DiscordMessageBuilder _messageBuilder;

        /// <summary>
        /// Creates discord message based on entered json
        /// </summary>
        /// <param name="author"></param>
        /// <param name="json"></param>
        public RoledeckMessage(string author, JObject json)
        {
            try
            {
                _deck = new(json);
            }
            catch
            {
                throw new KeyNotFoundException("A JSON property was not found!");
            }

            _messageBuilder = new();

            _messageBuilder.WithContent($"# {FormatString(_deck.Name)} By {author}\n\n{GetRolelist()}{GetPhaseTimes()}{GetBannedRoles()}");

            _messageBuilder.AddFile($"{_deck.Name}.json", new MemoryStream(Encoding.UTF8.GetBytes(_deck.JsonString)));
        }
        /// <summary>
        /// Sends rolelist as discord message
        /// </summary>
        /// <param name="ctx"></param>
        public async void SendRoledeck(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(new(_messageBuilder));
        }
        private string GetRolelist()
        {
            string rolelist = "## Roles\n";

            for (int i = 0; i < _deck.Rolelist.Length; i++)
            {
                rolelist += "- ";
                for (int j = 0; j < _deck.Rolelist[i].Length; j++)
                {
                    rolelist += FormatString(_deck.Rolelist[i][j].ToString()) + ((_deck.Rolelist[i].Length > 1 && j < _deck.Rolelist[i].Length - 1) ? " OR " : "");
                }
                rolelist += "\n";
            }

            return rolelist;
        }
        private string GetPhaseTimes()
        {
            string phaseTimes = "## Phase Times\n";

            for (int i = 0; i < _deck.PhaseTimes.Length; i++)
            {
                phaseTimes += $"- {FormatString(((Roledeck.Phases)i).ToString())} : {_deck.PhaseTimes[i]} seconds\n";
            }

            return phaseTimes;
        }
        private string GetBannedRoles()
        {
            string bannedRoles = "## Disabled Roles\n";

            for (int i = 0; i < _deck.BannedRoles.Length; i++)
            {
                bannedRoles += $"- {FormatString(_deck.BannedRoles[i])}\n";
            }

            bannedRoles += (_deck.BannedRoles.Length == 0) ? "- None" : "";

            return bannedRoles;
        }
        /// <summary>
        /// Splits string at capitals and capitalizes the first letter
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string FormatString(string data) => char.ToUpper(data[0]) + string.Join(" ", Regex.Split(data, @"(?<!^)(?=[A-Z](?![A-Z]|$))")).Trim()[1..];
    }
}
