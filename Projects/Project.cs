using System.Collections.Generic;

namespace ShimabuttsIrcBot.Projects
{
    public class Project : SequencedRoles
    {
        private readonly Dictionary<Role, HashSet<string>> _roles = new Dictionary<Role, HashSet<string>>();
        private readonly Dictionary<string, HashSet<Role>> _rolesReserve = new Dictionary<string, HashSet<Role>>();

        public string Name { get; private set; }

        public Project(string name, bool isMango = true)
            : base(isMango)
        {
            Name = name;
        }

        public string GetSummary()
        {
            string summary = "";
            foreach (var role in _roles)
            {
                if (role.Value != null && role.Value.Count > 0)
                {
                    summary += role.Key.ToString();
                    summary += string.Join(",", role.Value);
                    summary += " ";
                }
            }
            return summary;
        }

        public IEnumerable<Role> CheckProjectForName(string name)
        {
            if (_rolesReserve.ContainsKey(name))
            {
                return _rolesReserve[name];
            }
            else
            {
                return new Role[0];
            }
        }

        public IEnumerable<string> CheckProjectForRole(Role role)
        {
            if (_roles.ContainsKey(role))
            {
                return _roles[role];
            }
            else
            {
                return new string[0];
            }
        }

        public ProjectQueryResponse AddNameToRole(string name, Role role)
        {
            if (!_roles.ContainsKey(role))
            {
                _roles.Add(role, new HashSet<string>());
            }
            if (_roles[role].Contains(name))
                return ProjectQueryResponse.AlreadyThere;
            _roles[role].Add(name);
            if (!_rolesReserve.ContainsKey(name))
            {
                _rolesReserve.Add(name, new HashSet<Role>());
            }
            _rolesReserve[name].Add(role);
            return ProjectQueryResponse.Added;
        }

        public ProjectQueryResponse RemoveNameFromRole(string name, Role role)
        {
            if (!_roles.ContainsKey(role))
            {
                _roles.Add(role, new HashSet<string>());
            }
            if (_roles[role].Contains(name))
            {
                _roles[role].Remove(name);
                _rolesReserve[name].Remove(role);
                return ProjectQueryResponse.Removed;
            }
            return ProjectQueryResponse.WasntThere;
        }
    }
}