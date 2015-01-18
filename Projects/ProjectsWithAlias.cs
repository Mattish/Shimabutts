using System.Collections.Generic;

namespace ShimabuttsIrcBot.Projects
{
    public class ProjectsWithAlias
    {
        private readonly Dictionary<string, Project> _projects = new Dictionary<string, Project>();
        private readonly Dictionary<string, Project> _projectAlias = new Dictionary<string, Project>();

        public Project GetByActualNameOnly(string name)
        {
            if (!_projects.ContainsKey(name))
            {
                return null;
            }
            return _projects[name];
        }

        public Project GetByAliasOnly(string name)
        {
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

        public Project this[string name]
        {
            get { return GetByName(name); }
        }

        public bool HasProject(string name)
        {
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

        public void Add(Project project)
        {
            _projects.Add(project.Name, project);
        }

        public bool AddAlias(string originalName, string alias)
        {
            var project = GetByActualNameOnly(originalName);
            if (project != null)
            {
                _projectAlias.Add(alias, project);
                return true;
            }
            return false;
        }

        public bool RemoveAlias(string name)
        {
            if (_projectAlias.ContainsKey(name))
            {
                _projectAlias.Remove(name);
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