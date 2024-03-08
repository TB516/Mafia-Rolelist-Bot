using Newtonsoft.Json.Linq;

namespace Mafia_Bot.RoleDeckComponents
{
    /// <summary>
    /// Wrapper for rolelist data
    /// </summary>
    internal struct Roledeck
    {
        /// <summary>
        /// Phases of day/night
        /// </summary>
        public enum Phases
        {
            briefing,
            obituary,
            discussion,
            nomination,
            testimony,
            judgement,
            finalWords,
            dusk,
            night,
        }

        private readonly JObject _roleDeck;
        private readonly string _name;
        private readonly string[][] _rolelist;
        private readonly int[] _phaseTimes;
        private readonly string[] _roleBans;

        /// <summary>
        /// JSON data of rolelist
        /// </summary>
        public readonly string JsonString => _roleDeck.ToString();
        /// <summary>
        /// Name of rolelist
        /// </summary>
        public readonly string Name => _name;
        /// <summary>
        /// String representation of the rolelist
        /// </summary>
        public readonly string[][] Rolelist => _rolelist;
        /// <summary>
        /// Array of phasetimes
        /// </summary>
        public readonly int[] PhaseTimes => _phaseTimes;
        /// <summary>
        /// String representation of banned roles
        /// </summary>
        public readonly string[] BannedRoles => _roleBans;

        /// <summary>
        /// Wrapper for rolelist data
        /// </summary>
        /// <param name="json">JSON node to parse</param>
        /// <exception cref="KeyNotFoundException">Thrown if JSON property is not found</exception>
        public Roledeck(JObject json)
        {
            _roleDeck = json;

            if (_roleDeck["name"] == null)
            {
                _roleDeck.Add("name", "Unnamed Game Mode");
            }
            else if (_roleDeck["name"]!.ToString().Trim() == "")
            {
                _roleDeck["name"] = "Unnamed Game Mode";
            }
            _name = _roleDeck["name"]!.ToString();

            _rolelist = new string[_roleDeck["roleList"]!.Count()][];
            _phaseTimes = new int[_roleDeck["phaseTimes"]!.Count()];
            _roleBans = new string[_roleDeck["disabledRoles"]!.Count()];

            PopulateRolelist();
            PopulatePhaseTimes();
            PopulateRoleBans();
        }
        private readonly void PopulateRolelist()
        {
            for (int i = 0; i < _rolelist.Length; i++)
            {
                _rolelist[i] = new string[_roleDeck["roleList"]![i]!["options"]!.Count()];

                for (int j = 0; j < _rolelist[i].Length; j++)
                {
                    string type = _roleDeck["roleList"]![i]!["options"]![j]!["type"]!.ToString();
                    _rolelist[i][j] = _roleDeck["roleList"]![i]!["options"]![j]![type]!.ToString();
                }
            }
        }
        private readonly void PopulatePhaseTimes()
        {
            for (int i = 0; i < _phaseTimes.Length; i++)
            {
                _phaseTimes[i] = (int)_roleDeck["phaseTimes"]![((Phases)i).ToString()]!;
            }
        }
        private readonly void PopulateRoleBans()
        {
            for (int i = 0; i < _roleBans.Length; i++)
            {
                _roleBans[i] = _roleDeck["disabledRoles"]![i]!.ToString();
            }
        }
    }
}
