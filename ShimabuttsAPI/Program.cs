using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Nancy;
using Nancy.Hosting.Self;
using ServiceStack.Redis;
using ServiceStack.Text;
using ShimabuttsIrcBot.Redis;

namespace ShimabuttsAPI
{
    class Program
    {
        static void Main()
        {
            var config = new HostConfiguration
            {
                RewriteLocalhost = false
            };
            var nancyHost = new NancyHost(config, new Uri("http://localhost:1693/"));
            ShimabuttsRedisStatic.Start();
            nancyHost.Start();
            while (true)
            {
                Thread.Sleep(1);
            }
        }
    }

    public class ShimabuttsRedisStatic : GenericShimabuttsRedis
    {
        private static ShimabuttsRedisStatic _shimabuttsRedis;

        private ShimabuttsRedisStatic(RedisClient redisClient)
            : base(redisClient)
        {
        }

        public static void Start()
        {
            if (_shimabuttsRedis == null)
            {
                var redisClient = new RedisClient("localhost");
                _shimabuttsRedis = new ShimabuttsRedisStatic(redisClient);
            }
        }

        public static ShimabuttsRedisStatic Instance()
        {
            if (_shimabuttsRedis == null)
                Start();
            return _shimabuttsRedis;
        }
    }

    public class ShimabuttsModule : NancyModule
    {
        public ShimabuttsModule()
        {
            StaticConfiguration.DisableErrorTraces = false;
            Get["/Projects"] = _ =>
            {
                var isDone = (bool)Request.Query["isDone"];
                var hasIsDoneQuery = Request.Query["isDone"] != null;
                var projectNames = ShimabuttsRedisStatic.Instance().GetAllMangoProjectNames();
                projectNames.UnionWith(ShimabuttsRedisStatic.Instance().GetAllAnimeProjectNames());

                var resultProjectNames = new HashSet<string>();
                if (hasIsDoneQuery)
                {
                    var doneProjects = ShimabuttsRedisStatic.Instance().GetAllDoneProjects();
                    if (isDone)
                    {
                        resultProjectNames.UnionWith(doneProjects);
                    }
                    else
                    {
                        projectNames.ExceptWith(doneProjects);
                        resultProjectNames.UnionWith(projectNames);
                    }
                }
                else
                {
                    resultProjectNames.UnionWith(projectNames);
                }

                var listing = new ProjectListing()
                {
                    Projects = resultProjectNames.ToArray()
                };
                return JsonSerializer.SerializeToString(listing);
            };

            Get["/Projects/{Name}/"] = _ =>
            {
                var projectName = _.Name;
                var existsValue = ShimabuttsRedisStatic.Instance().DoesProjectExist(projectName);
                if (existsValue > 0)
                {
                    Project project = GetProject(projectName);
                    project.Type = existsValue == 1 ? "Mango" : "Anime";
                    return JsonSerializer.SerializeToString(project);
                }
                return HttpStatusCode.NotFound;
            };
        }

        public Project GetProject(string name)
        {
            var project = new Project { Name = name, Aliases = ShimabuttsRedisStatic.Instance().GetAliasForProject(name) };
            var roles = new List<Role>();
            foreach (var role in (ShimabuttsIrcBot.Projects.Role[])Enum.GetValues(typeof(ShimabuttsIrcBot.Projects.Role)))
            {
                var newRole = new Role
                {
                    IsDone = ShimabuttsRedisStatic.Instance().GetRoleDone(name, role),
                    Assigned = ShimabuttsRedisStatic.Instance().GetNamesFromRole(name, role),
                    Name = role.ToString()
                };
                roles.Add(newRole);
            }
            project.Roles = roles;
            return project;
        }
    }

    public class ProjectListing
    {
        public string[] Projects { get; set; }
    }

    public class Project
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public IEnumerable<string> Aliases { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }

    public class Role
    {
        public string Name { get; set; }
        public IEnumerable<string> Assigned { get; set; }
        public bool IsDone { get; set; }
    }
}
