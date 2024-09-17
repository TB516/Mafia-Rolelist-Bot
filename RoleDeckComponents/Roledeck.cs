using Newtonsoft.Json.Linq;

namespace Mafia_Bot.RoleDeckComponents
{
    /// <summary>
    /// Wrapper for rolelist data
    /// </summary>
    internal struct Roledeck
    {
        public static readonly string[] RoleTypes = ["town", "mafia", "neutral", "fiends", "cult", "townInvestigative", "townKilling", "townSupport", "townProtective", "mafiaKilling", "mafiaSupport", "neutralEvil", "any"];
        private static readonly string s_rolesUrl = "https://raw.githubusercontent.com/mafia-rust/mafia/main/client/src/resources/roles.json";
        private static readonly int s_phasesCount = Enum.GetNames(typeof(Phases)).Length;

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
        private readonly string[] _enabledRoles;

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
        /// String representation of enabled roles
        /// </summary>
        public readonly string[] EnabledRoles => _enabledRoles;

        /// <summary>
        /// Wrapper for rolelist data
        /// </summary>
        /// <param name="json">JSON node to parse</param>
        /// <exception cref="KeyNotFoundException">Thrown if JSON property is not found</exception>
        public Roledeck(JObject json)
        {
            _roleDeck = json;

            ValidateGamemode();

            #region Set Name
            if (_roleDeck["name"]!.ToString().Trim() == "")
            {
                _roleDeck["name"] = "Unnamed Game Mode";
            }
            _name = _roleDeck["name"]!.ToString();
            #endregion

            _rolelist = new string[_roleDeck["roleList"]!.Count()][];
            _phaseTimes = new int[_roleDeck["phaseTimes"]!.Count()];
            _enabledRoles = new string[_roleDeck["enabledRoles"]!.Count()];

            PopulateRolelist();
            PopulatePhaseTimes();
            PopulateEnabledRoles();
        }
        private readonly void ValidateGamemode()
        {
            if (_roleDeck["name"] == null)
            {
                _roleDeck.Add("name", "");
            }
            
            if (_roleDeck["roleList"] == null)
            {
                throw new KeyNotFoundException("The Role List was not found in the entered JSON!");
            }
            else if (_roleDeck["phaseTimes"] == null)
            {
                throw new KeyNotFoundException("Phase Times were not found in the entered JSON!");
            }
            else if (_roleDeck["enabledRoles"] == null)
            {
                throw new KeyNotFoundException("Enabled Roles were not found in the entered JSON!");
            }

            ValidatePhaseTimes();
            ValidateRoles();
        }
        private readonly void ValidatePhaseTimes()
        {
            if (_roleDeck["phaseTimes"]!.Count() > s_phasesCount)
            {
                throw new Exception("Unexpected info found in Phase Times!");
            }

            for (int i = 0; i < s_phasesCount; i++)
            {
                if (_roleDeck["phaseTimes"]![((Phases)i).ToString()] == null)
                {
                    throw new MissingFieldException("Missing data for phase: " + ((Phases)i).ToString() + "!");
                }
            }
        }
        private readonly void ValidateRoles()
        {
            HttpResponseMessage response = new HttpClient().GetAsync(s_rolesUrl).GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("There was an error validating the roles, please try again.");
            }

            JObject roles = JObject.Parse(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

            for (int i = 0; i < _roleDeck["roleList"]!.Count(); i++)
            {
                for (int j = 0; j < _roleDeck["roleList"]![i]!["options"]!.Count(); j++)
                {
                    string type = _roleDeck["roleList"]![i]!["options"]![j]!["type"]!.ToString();
                    if (!RoleTypes.Contains(_roleDeck["roleList"]![i]!["options"]![j]![type]!.ToString()) && !roles.ContainsKey(_roleDeck["roleList"]![i]!["options"]![j]![type]!.ToString()))
                    {
                        throw new KeyNotFoundException($"Role : {_roleDeck["roleList"]![i]!["options"]![j]![type]} is not a valid role or type!");
                    }
                }
            }

            for (int i = 0; i < _roleDeck["enabledRoles"]!.Count(); i++)
            {
                if (!roles.ContainsKey(_roleDeck["enabledRoles"]![i]!.ToString()))
                {
                    throw new KeyNotFoundException($"Role : {_roleDeck["enabledRoles"]![i]} is not a valid role to have enabled!");
                }
            }
        }
        private readonly void PopulateRolelist()
        {
            for (int i = 0; i < _rolelist.Length; i++)
            {
                switch (_roleDeck["roleList"]![i]!["type"]!.ToString())
                {
                    case "any":
                        _rolelist[i] = ["any"];
                        break;
                    default:
                        _rolelist[i] = new string[_roleDeck["roleList"]![i]!["options"]!.Count()];

                        for (int j = 0; j < _rolelist[i].Length; j++)
                        {
                            string type = _roleDeck["roleList"]![i]!["options"]![j]!["type"]!.ToString();
                            _rolelist[i][j] = _roleDeck["roleList"]![i]!["options"]![j]![type]!.ToString();
                        }
                        break;
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
        private readonly void PopulateEnabledRoles()
        {
            for (int i = 0; i < _enabledRoles.Length; i++)
            {
                _enabledRoles[i] = _roleDeck["enabledRoles"]![i]!.ToString();
            }
        }
    }
}
