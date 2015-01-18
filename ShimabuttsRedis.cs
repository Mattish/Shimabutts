using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ServiceStack.Redis;
using ShimabuttsIrcBot.Projects;

namespace ShimabuttsIrcBot
{
    public class ShimabuttsRedis
    {
        private readonly RedisClient _redisClient;

        public ShimabuttsRedis()
        {
            _redisClient = new RedisClient("localhost");
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
        }

        public void RemoveAliasForProject(string project, string alias)
        {
            _redisClient.RemoveItemFromSet(string.Format("Projects:{0}:Aliases", project), alias);
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

        public ProjectsWithAlias GetAllProjects()
        {
            var projectsWithAlias = new ProjectsWithAlias();

            var mangoProjectNames = _redisClient.GetAllItemsFromSet("MangoProjects");
            var animeProjectNames = _redisClient.GetAllItemsFromSet("AnimeProjects");

            //Mango
            foreach (var projectName in mangoProjectNames)
            {
                var newProject = new Project(projectName);
                foreach (var role in (Role[])Enum.GetValues(typeof(Role)))
                {
                    var whoInRole = _redisClient.GetAllItemsFromSet(string.Format("Projects:{0}:Roles:{1}", projectName, role));
                    foreach (var name in whoInRole)
                    {
                        newProject.AddNameToRole(name, role);
                    }
                    var isRoleDone = _redisClient.Get<bool>(string.Format("Projects:{0}:Roles:{1}:IsDone", projectName, role));
                    if (isRoleDone)
                        newProject.SetAsDone(role);
                    else
                        newProject.SetAsUndone(role);
                    var dateTime = GetTimeWaiting(projectName);
                    newProject.SetDateTime(dateTime);
                }
                projectsWithAlias.Add(newProject);
                var aliasForProject = _redisClient.GetAllItemsFromSet(string.Format("Projects:{0}:Aliases", projectName));
                foreach (var alias in aliasForProject)
                {
                    projectsWithAlias.AddAlias(projectName, alias);
                }
            }

            //Anime
            foreach (var projectName in animeProjectNames)
            {
                var newProject = new Project(projectName, false);
                foreach (var role in (Role[])Enum.GetValues(typeof(Role)))
                {
                    var whoInRole = _redisClient.GetAllItemsFromSet(string.Format("Projects:{0}:Roles:{1}", projectName, role));
                    foreach (var name in whoInRole)
                    {
                        newProject.AddNameToRole(name, role);
                    }
                    var isRoleDone = _redisClient.Get<bool>(string.Format("Projects:{0}:Roles:{1}:IsDone", projectName, role));
                    if (isRoleDone)
                        newProject.SetAsDone(role);
                    else
                        newProject.SetAsUndone(role);
                }
                projectsWithAlias.Add(newProject);
                var aliasForProject = _redisClient.GetAllItemsFromSet(string.Format("Projects:{0}:Aliases", projectName));
                foreach (var alias in aliasForProject)
                {
                    projectsWithAlias.AddAlias(projectName, alias);
                }
            }

            return projectsWithAlias;
        }
    }
}