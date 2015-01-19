using System;
using System.Collections.Generic;
using ServiceStack.Redis;
using ShimabuttsIrcBot.Projects;

namespace ShimabuttsIrcBot
{
    public class GenericShimabuttsRedis
    {
        private readonly RedisClient _redisClient;

        public GenericShimabuttsRedis(RedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        public HashSet<string> GetAllMangoProjectNames()
        {
            return new HashSet<string>(_redisClient.GetAllItemsFromSet("MangoProjects"));
        }

        public HashSet<string> GetAllAnimeProjectNames()
        {
            return new HashSet<string>(_redisClient.GetAllItemsFromSet("AnimeProjects"));
        }

        public HashSet<string> GetAllAlias()
        {
            return new HashSet<string>(_redisClient.GetAllItemsFromSet("Aliases"));
        }

        public void AddNameToRole(string project, string name, Role role)
        {
            _redisClient.AddItemToSet(string.Format("Projects:{0}:Roles:{1}", project, role), name);
        }

        public void RemoveNameFromRole(string project, string name, Role role)
        {
            _redisClient.RemoveItemFromSet(string.Format("Projects:{0}:Roles:{1}", project, role), name);
        }

        public void AddAliasForProject(string project, string alias)
        {
            _redisClient.AddItemToSet(string.Format("Projects:{0}:Aliases", project), alias);
            _redisClient.AddItemToSet(string.Format("Aliases"), alias);
            _redisClient.Add(string.Format("Aliases:{0}", alias), project);
        }

        public void RemoveAliasForProject(string project, string alias)
        {
            _redisClient.RemoveItemFromSet(string.Format("Projects:{0}:Aliases", project), alias);
            _redisClient.RemoveItemFromSet(string.Format("Aliases"), alias);
            _redisClient.Remove(string.Format("Aliases:{0}", alias));
        }

        public string GetProjectFromAlias(string alias)
        {
            return _redisClient.Get<string>(string.Format("Aliases:{0}", alias));
        }

        public void SetRoleDone(string project, Role role)
        {
            _redisClient.Set(string.Format("Projects:{0}:Roles:{1}:IsDone", project, role), true);
        }

        public void SetRoleUndone(string project, Role role)
        {
            _redisClient.Set(string.Format("Projects:{0}:Roles:{1}:IsDone", project, role), false);
        }

        public void SetTimeWaiting(string project, DateTime dateTime)
        {
            _redisClient.Set(string.Format("Projects:{0}:DateTime", project), dateTime);
        }

        public DateTime GetTimeWaiting(string project)
        {
            return _redisClient.Get<DateTime>(string.Format("Projects:{0}:DateTime", project));
        }

        public void AddMangoProject(string project)
        {
            _redisClient.AddItemToSet("MangoProjects", project);
            foreach (var role in (Role[])Enum.GetValues(typeof(Role)))
            {
                SetRoleUndone(project, role);
            }
            SetRoleDone(project, Role.TM);
        }

        public void AddAnimeProject(string project)
        {
            _redisClient.AddItemToSet("AnimeProjects", project);
            foreach (var role in (Role[])Enum.GetValues(typeof(Role)))
            {
                SetRoleUndone(project, role);
            }
            SetRoleDone(project, Role.CL);
            SetRoleDone(project, Role.RD);
        }

        public void RemoveProject(string project)
        {
            _redisClient.RemoveItemFromSet("MangoProjects", project);
            _redisClient.RemoveItemFromSet("AnimeProjects", project);
            foreach (var role in (Role[])Enum.GetValues(typeof(Role)))
            {
                _redisClient.Del(string.Format("Projects:{0}:Roles:{1}", project, role));
                _redisClient.Del(string.Format("Projects:{0}:Roles:{1}:IsDone", project, role));
            }
            _redisClient.Del(string.Format("Projects:{0}:Aliases", project));
        }
    }
}