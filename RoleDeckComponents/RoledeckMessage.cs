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
        private static readonly Uri s_wikiLink = new("https://mafia.jackpapel.com/wiki");
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
                throw;
            }

            _messageBuilder = new();

            _messageBuilder.WithContent($"# \"{FormatString(_deck.Name)}\" By {author}\n\n{GetRolelist()}{GetPhaseTimes()}{GetBannedRoles()}");

            _messageBuilder.AddFile($"{_deck.Name}.json", new MemoryStream(Encoding.UTF8.GetBytes(_deck.JsonString)));
        }
        /// <summary>
        /// Sends rolelist as discord message
        /// </summary>
        /// <param name="ctx"></param>
        public async void SendRoledeck(InteractionContext ctx)
        {
            await ctx.Channel.SendMessageAsync(_messageBuilder);
        }
        private string GetRolelist()
        {
            string rolelist = "## Roles\n";

            for (int i = 0; i < _deck.Rolelist.Length; i++)
            {
                rolelist += "- ";
                for (int j = 0; j < _deck.Rolelist[i].Length; j++)
                {
                    string roleset = FormatString(_deck.Rolelist[i][j].ToString());

                    switch (roleset)
                    {
                        case "Town":
                        case "Mafia":
                        case "Neutral":
                        case "Fiends":
                        case "Cult":
                        case "Town Investigative":
                        case "Town Killing":
                        case "Town Support":
                        case "Town Protective":
                        case "Mafia Killing":
                        case "Mafia Support":
                        case "Neutral Evil":
                        case "Any":
                            rolelist += $"[{roleset}]({s_wikiLink}/generated/role_set)" + ((_deck.Rolelist[i].Length > 1 && j < _deck.Rolelist[i].Length - 1) ? " OR " : "");
                            break;
                        default:
                            rolelist += $"[{roleset}]({s_wikiLink}/role/{_deck.Rolelist[i][j].ToLower()})" + ((_deck.Rolelist[i].Length > 1 && j < _deck.Rolelist[i].Length - 1) ? " OR " : "");
                            break;
                    }
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
            string enabledRoles = "## Enabled Roles\n";

            for (int i = 0; i < _deck.EnabledRoles.Length; i++)
            {
                string roleset = FormatString(_deck.EnabledRoles[i].ToString());

                switch (roleset)
                {
                    case "Town":
                    case "Mafia":
                    case "Neutral":
                    case "Fiends":
                    case "Cult":
                        enabledRoles += $"- [{roleset}]({s_wikiLink}/standard/{_deck.EnabledRoles[i].ToLower()})\n";
                        break;
                    case "Town Investigative":
                    case "Town Killing":
                    case "Town Support":
                    case "Town Protective":
                    case "Mafia Killing":
                    case "Mafia Support":
                    case "Neutral Evil":
                    case "Any":
                        enabledRoles += $"- [{roleset}]({s_wikiLink}/generated/role_set)\n";
                        break;
                    default:
                        enabledRoles += $"- [{roleset}]({s_wikiLink}/role/{_deck.EnabledRoles[i].ToLower()})\n";
                        break;
                }
            }

            enabledRoles += (_deck.EnabledRoles.Length == 0) ? "- None" : "";

            return enabledRoles;
        }
        /// <summary>
        /// Splits string at capitals and capitalizes the first letter
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string FormatString(string data) => char.ToUpper(data[0]) + string.Join(" ", Regex.Split(data, @"(?<!^)(?=[A-Z](?![A-Z]|$))")).Trim()[1..];
    }
}
