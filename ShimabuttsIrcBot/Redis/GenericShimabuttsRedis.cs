using System;
using System.Collections.Generic;
using ServiceStack.Redis;
using ShimabuttsIrcBot.Projects;

namespace ShimabuttsIrcBot.Redis
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

        public HashSet<string> GetAllDoneProjects()
        {
            return new HashSet<string>(_redisClient.GetAllItemsFromSet("Projects:IsDone"));
        }

        /*
         * Returns 1 for Mango, 2 for Anime, 0 for doesn't exist.
         */
        public int DoesProjectExist(string project)
        {
            var mangoContains = _redisClient.SetContainsItem("MangoProjects", project);
            var animeContains = _redisClient.SetContainsItem("AnimeProjects", project);
            if (mangoContains)
                return 1;
            if (animeContains)
                return 2;
            return 0;
        }

        public void AddNameToRole(string project, string name, Role role)
        {
            _redisClient.AddItemToSet(string.Format("Projects:{0}:Roles:{1}", project, role), name);
        }

        public void RemoveNameFromRole(string project, string name, Role role)
        {
            _redisClient.RemoveItemFromSet(string.Format("Projects:{0}:Roles:{1}", project, role), name);
        }

        public HashSet<string> GetNamesFromRole(string project, Role role)
        {
            return new HashSet<string>(_redisClient.GetAllItemsFromSet(string.Format("Projects:{0}:Roles:{1}", project, role)));
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

        public HashSet<string> GetAliasForProject(string project)
        {
            return new HashSet<string>(_redisClient.GetAllItemsFromSet(string.Format("Projects:{0}:Aliases", project)));
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

        public bool GetRoleDone(string project, Role role)
        {
            return _redisClient.Get<bool>(string.Format("Projects:{0}:Roles:{1}:IsDone", project, role));
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

        public void SetIsDone(string project)
        {
            _redisClient.AddItemToSet("Projects:IsDone", project);
        }

        public void SetUndone(string project)
        {
            _redisClient.RemoveItemFromSet("Projects:IsDone", project);
        }
    }
}