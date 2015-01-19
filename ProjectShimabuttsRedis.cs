using System;
using ServiceStack.Redis;
using ShimabuttsIrcBot.Projects;
using HashSetString = System.Collections.Generic.HashSet<string>;

namespace ShimabuttsIrcBot
{
    public class ProjectShimabuttsRedis
    {
        private readonly RedisClient _redisClient;
        private readonly string _projectName;

        public ProjectShimabuttsRedis(RedisClient redisClient, string projectName)
        {
            _redisClient = redisClient;
            _projectName = projectName;
        }

        public void AddNameToRole(string name, Role role)
        {
            _redisClient.AddItemToSet(string.Format("Projects:{0}:Roles:{1}", _projectName, role), name);
        }

        public void RemoveNameFromRole(string name, Role role)
        {
            _redisClient.RemoveItemFromSet(string.Format("Projects:{0}:Roles:{1}", _projectName, role), name);
        }

        public HashSetString GetNamesFromRole(Role role)
        {
            return new HashSetString(_redisClient.GetAllItemsFromSet(string.Format("Projects:{0}:Roles:{1}", _projectName, role)));
        }

        public void AddAliasForProject(string alias)
        {
            _redisClient.AddItemToSet(string.Format("Projects:{0}:Aliases", _projectName), alias);
        }

        public void RemoveAliasForProject(string alias)
        {
            _redisClient.RemoveItemFromSet(string.Format("Projects:{0}:Aliases", _projectName), alias);
        }

        public HashSetString GetAliasForProject()
        {
            return new HashSetString(_redisClient.GetAllItemsFromSet(string.Format("Projects:{0}:Aliases", _projectName)));
        }

        public void SetRoleDone(Role role)
        {
            _redisClient.Set(string.Format("Projects:{0}:Roles:{1}:IsDone", _projectName, role), true);
        }

        public void SetRoleUndone(Role role)
        {
            _redisClient.Set(string.Format("Projects:{0}:Roles:{1}:IsDone", _projectName, role), false);
        }

        public bool GetRoleDone(Role role)
        {
            return _redisClient.Get<bool>(string.Format("Projects:{0}:Roles:{1}:IsDone", _projectName, role));
        }

        public void SetTimeWaiting(DateTime dateTime)
        {
            _redisClient.Set(string.Format("Projects:{0}:DateTime", _projectName), dateTime);
        }

        public DateTime GetTimeWaiting()
        {
            return _redisClient.Get<DateTime>(string.Format("Projects:{0}:DateTime", _projectName));
        }
    }
}