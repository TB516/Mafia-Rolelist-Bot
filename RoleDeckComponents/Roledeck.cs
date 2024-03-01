using System.Text.Json.Nodes;

namespace Mafia_Bot.RoleDeckComponents
{
    internal class Roledeck
    {
        private JsonNode _roleDeck;
        private JsonArray[] _rolelist;
        private JsonNode _phaseTimes;
        private JsonArray _roleBans;

        public string JsonString => _roleDeck.ToString();
        public JsonArray[] Rolelist => _rolelist;
        public JsonNode PhaseTimes => _phaseTimes;
        public JsonArray BannedRoled => _roleBans;

        public Roledeck(string json)
        {
            _roleDeck = JsonNode.Parse(json)!;
            _rolelist = new JsonArray[_roleDeck["roleList"]!.AsArray()!.Count];

            for (short i = 0; i < _rolelist.Length; i++)
            {
                _rolelist[i] = (_roleDeck["roleList"]![i]!["options"]!.AsArray());
            }
            _phaseTimes = _roleDeck["phaseTimes"]!;
            _roleBans = _roleDeck["disabledRoles"]!.AsArray();
        }
    }
}
