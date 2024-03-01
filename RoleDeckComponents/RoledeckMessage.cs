using DSharpPlus.Entities;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Mafia_Bot.RoleDeckComponents
{
    public class RoledeckMessage
    {
        private string _deckName;
        private Roledeck _deck;
        private DiscordMessageBuilder _messageBuilder;

        public RoledeckMessage(string deckName, string json)
        {
            _deckName = deckName;
            _deck = new(json);

            _messageBuilder = new();
        }
        private string GetRolelist()
        {
            string rolelist = "";

            for (short i = 0; i < _deck.Rolelist.Length; i++)
            {
                rolelist += $"{i + 1}. ";
                    
                for (short j = 0; j < _deck.Rolelist[i].Count; j++)
                {
                    //The role option type to get
                    string type = _deck.Rolelist[i][j]!["type"]!.ToString();

                    rolelist += FormatString(_deck.Rolelist[i][j]![type]!);
                    rolelist += (_deck.Rolelist[i].Count > 1 && j < _deck.Rolelist[i].Count - 1) ? " OR " : "";
                }

                rolelist += "\n";
            }

            return rolelist;
        }
        private static string FormatString(JsonNode data) => string.Join(" ", Regex.Split(data.ToString(), @"(?<!^)(?=[A-Z](?![A-Z]|$))")).ToUpper();
    }
}
