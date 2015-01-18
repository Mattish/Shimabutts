using System;
using System.Collections.Generic;

namespace ShimabuttsIrcBot.Projects
{
    public class SequencedRoles
    {
        private readonly Dictionary<Role, bool> _roleProgress = new Dictionary<Role, bool>();
        private DateTime _lastChangedTime = DateTime.UtcNow;

        public SequencedRoles(bool isMango = true)
        {
            foreach (var role in (Role[])Enum.GetValues(typeof(Role)))
            {
                _roleProgress.Add(role, false);
            }

            if (isMango)
            {
                _roleProgress[Role.TM] = true;
            }
            else
            {
                _roleProgress[Role.CL] = true;
                _roleProgress[Role.RD] = true;
            }
        }

        public void SetAsDone(Role role)
        {
            var waitingAtRole = WaitingAt();
            _roleProgress[role] = true;
            var waitingAtRoleNew = WaitingAt();
            if (!waitingAtRole.HasValue && !waitingAtRoleNew.HasValue) // Both null
                return;
            if (!waitingAtRole.HasValue && waitingAtRoleNew.HasValue)
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
            _roleProgress[role] = false;
            var waitingAtRoleNew = WaitingAt();
            if (!waitingAtRole.HasValue && !waitingAtRoleNew.HasValue) // Both null
                return;
            if (!waitingAtRole.HasValue && waitingAtRoleNew.HasValue)
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
                if (!_roleProgress[role])
                {
                    return role;
                }
            }
            return null;
        }
    }
}