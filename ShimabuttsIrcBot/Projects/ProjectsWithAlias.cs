using System.Collections.Generic;
using ServiceStack.Redis;
using ShimabuttsIrcBot.Redis;

namespace ShimabuttsIrcBot.Projects
{
    public class ProjectsWithAlias : GenericShimabuttsRedis
    {
        private readonly RedisClient _redis;
        private readonly Dictionary<string, Project> _projects = new Dictionary<string, Project>();
        private readonly Dictionary<string, Project> _projectAlias = new Dictionary<string, Project>();

        public ProjectsWithAlias(RedisClient redisClient)
            : base(redisClient)
        {
            _redis = redisClient;
            Load();

        }

        private void Load()
        {
            var mangoProjectNames = _redis.GetAllItemsFromSet("MangoProjects");
            var animeProjectNames = _redis.GetAllItemsFromSet("AnimeProjects");

            //Mango
            foreach (var projectName in mangoProjectNames)
            {
                AddExistingProject(projectName);
                var aliasForProject = _redis.GetAllItemsFromSet(string.Format("Projects:{0}:Aliases", projectName));
                foreach (var alias in aliasForProject)
                {
                    AddAlias(projectName, alias);
                }
            }

            //Anime
            foreach (var projectName in animeProjectNames)
            {
                AddExistingProject(projectName, false);
                var aliasForProject = _redis.GetAllItemsFromSet(string.Format("Projects:{0}:Aliases", projectName));
                foreach (var alias in aliasForProject)
                {
                    AddAlias(projectName, alias);
                }
            }
        }

        private void AddExistingProject(string name, bool isMango = true)
        {
            _projects[name] = new Project(name, _redis, isMango);
        }

        private void ProjectAndAliasCheck()
        {
            var dbMangoProjectNames = GetAllMangoProjectNames();
            var dbAnimeProjectNames = GetAllAnimeProjectNames();
            var dbAliases = GetAllAlias();

            foreach (var dbProjectName in dbMangoProjectNames)
            {
                if (_projects.ContainsKey(dbProjectName))
                    continue;
                _projects.Add(dbProjectName, new Project(dbProjectName, _redis));
            }

            foreach (var dbProjectName in dbAnimeProjectNames)
            {
                if (_projects.ContainsKey(dbProjectName))
                    continue;
                _projects.Add(dbProjectName, new Project(dbProjectName, _redis, false));
            }

            foreach (var dbAlias in dbAliases)
            {
                if (_projectAlias.ContainsKey(dbAlias))
                    continue;
                var projectName = GetProjectFromAlias(dbAlias);
                if (projectName.Length < 1) // Check for empty Alias
                {
                    RemoveAlias(dbAlias);
                }
                _projectAlias[dbAlias] = _projects[projectName];
            }
        }

        public Project GetByActualNameOnly(string name)
        {
            ProjectAndAliasCheck();
            if (!_projects.ContainsKey(name))
            {
                return null;
            }
            return _projects[name];
        }

        public Project GetByAliasOnly(string name)
        {
            ProjectAndAliasCheck();
            if (!_projectAlias.ContainsKey(name))
            {
                return null;
            }
            return _projectAlias[name];
        }

        public int Count
        {
            get { return _projects.Count; }
        }

        public bool HasProject(string name)
        {
            ProjectAndAliasCheck();
            if (!_projects.ContainsKey(name))
            {
                if (!_projectAlias.ContainsKey(name))
                {
                    return false;
                }
                return true;
            }
            return true;
        }

        public Project GetByName(string name)
        {
            ProjectAndAliasCheck();
            if (!_projects.ContainsKey(name))
            {
                if (!_projectAlias.ContainsKey(name))
                {
                    return null;
                }
                return _projectAlias[name];
            }
            return _projects[name];
        }

        public void Add(string name, bool isMango = true)
        {
            _projects[name] = new Project(name, _redis, isMango);
            if (isMango)
                AddMangoProject(name);
            else
                AddAnimeProject(name);
        }

        public bool AddAlias(string originalName, string alias)
        {
            var project = GetByActualNameOnly(originalName);
            if (project != null)
            {
                _projectAlias[alias] = project;
                AddAliasForProject(originalName, alias);
                return true;
            }
            return false;
        }

        public bool RemoveAlias(string name)
        {
            if (_projectAlias.ContainsKey(name))
            {
                var project = _projectAlias[name];
                _projectAlias.Remove(name);
                RemoveAliasForProject(project.Name, name);
                return true;
            }
            return false;
        }

        public bool Remove(string name)
        {
            var project = GetByName(name);
            if (project != null)
            {
                _projects.Remove(project.Name);
                RemoveProject(project.Name);
                var aliasToRemove = new List<string>();

                foreach (var projectAlia in _projectAlias)
                {
                    if (projectAlia.Value == project)
                        aliasToRemove.Add(projectAlia.Key);
                }

                foreach (var alias in aliasToRemove)
                {
                    _projectAlias.Remove(alias);
                }
                return true;
            }
            return false;
        }

    }
}