using System.Text.Json.Nodes;

namespace Mafia_Bot.RoleDeckComponents
{
    public class RoledeckMessage
    {
        private string _deckName;
        private Roledeck _deck;

        public RoledeckMessage(string deckName, string json)
        {
            _deckName = deckName;
            _deck = new(json);
        }
    }
}
