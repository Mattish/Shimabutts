using System;
using System.Collections.Generic;
using ServiceStack.Redis;

namespace ShimabuttsIrcBot.Projects
{
    public class Project
    {
        private readonly ProjectShimabuttsRedis _redis;
        private DateTime _lastChangedTime = DateTime.UtcNow;

        public string Name { get; private set; }
        public bool IsMango { get; private set; }

        public Project(string name, RedisClient redis, bool isMango = true)
        {
            _redis = new ProjectShimabuttsRedis(redis, name);
            Name = name;
            IsMango = isMango;
            RoleDoneCheck(isMango);
        }

        private Dictionary<Role, bool> RoleDoneCheck(bool isMango)
        {
            var dictionary = new Dictionary<Role, bool>();
            foreach (var role in (Role[])Enum.GetValues(typeof(Role)))
            {
                var isRoleDone = _redis.GetRoleDone(role);
                dictionary[role] = isRoleDone;
            }

            if (isMango)
            {
                _redis.SetRoleDone(Role.TM);
            }
            else
            {
                _redis.SetRoleDone(Role.CL);
                _redis.SetRoleDone(Role.RD);
            }
            return dictionary;
        }

        private Dictionary<Role, HashSet<string>> RolePersonCheck()
        {
            var dictionary = new Dictionary<Role, HashSet<string>>();
            foreach (var role in (Role[])Enum.GetValues(typeof(Role)))
            {
                var whosDoing = _redis.GetNamesFromRole(role);
                dictionary[role] = whosDoing;
            }
            return dictionary;
        }

        public string GetSummary()
        {
            string summary = "";
            foreach (var role in RolePersonCheck())
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
            var rolesPersonDoes = new HashSet<Role>();
            foreach (var role in RolePersonCheck())
            {
                if (role.Value.Contains(name))
                {
                    rolesPersonDoes.Add(role.Key);
                }
            }
            return rolesPersonDoes;
        }

        public IEnumerable<string> CheckProjectForRole(Role role)
        {
            return _redis.GetNamesFromRole(role);
        }

        public ProjectQueryResponse AddNameToRole(string name, Role role)
        {
            _redis.AddNameToRole(name, role);
            _redis.SetTimeWaiting(GetDateTime());
            return ProjectQueryResponse.Added;
        }

        public ProjectQueryResponse RemoveNameFromRole(string name, Role role)
        {
            _redis.RemoveNameFromRole(name, role);
            _redis.SetTimeWaiting(GetDateTime());
            return ProjectQueryResponse.Removed;
        }

        public void SetAsDone(Role role)
        {
            var waitingAtRole = WaitingAt();
            _redis.SetRoleDone(role);
            var waitingAtRoleNew = WaitingAt();
            if (!waitingAtRole.HasValue && !waitingAtRoleNew.HasValue) // Both null
                return;
            if (!waitingAtRole.HasValue)
            {
                // First is null
                _lastChangedTime = DateTime.UtcNow;
                return;
            }
            if (waitingAtRoleNew != null && waitingAtRole.Value != waitingAtRoleNew.Value) // First is null
                _lastChangedTime = DateTime.UtcNow;
        }

        public void SetAsUndone(Role role)
        {
            var waitingAtRole = WaitingAt();
            _redis.SetRoleUndone(role);
            var waitingAtRoleNew = WaitingAt();
            if (!waitingAtRole.HasValue && !waitingAtRoleNew.HasValue) // Both null
                return;
            if (!waitingAtRole.HasValue)
            {
                // First is null
                _lastChangedTime = DateTime.UtcNow;
                return;
            }
            if (waitingAtRoleNew != null && waitingAtRole.Value != waitingAtRoleNew.Value) // First is null
                _lastChangedTime = DateTime.UtcNow;
        }

        public TimeSpan WaitingForHowLong()
        {
            return DateTime.UtcNow - _lastChangedTime;
        }

        public void SetDateTime(DateTime dateTime)
        {
            _lastChangedTime = dateTime;
        }

        public DateTime GetDateTime()
        {
            return _lastChangedTime;
        }

        public Role? WaitingAt()
        {
            foreach (var role in (Role[])Enum.GetValues(typeof(Role)))
            {
                if (!_redis.GetRoleDone(role))
                {
                    return role;
                }
            }
            return null;
        }
    }
}