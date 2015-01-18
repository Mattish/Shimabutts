using NetIrc2;
using NetIrc2.Events;
using ShimabuttsIrcBot.Projects;

namespace ShimabuttsIrcBot.Commands
{
    public class RemoveAliasCommand : BotCommand
    {
        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, ProjectsWithAlias projects,
            ShimabuttsRedis redis)
        {
            var splits = eventArgs.Message.ToString().Split(' ');
            if (splits.Length == 2)
            {
                var alias = splits[1];

                var project = projects.GetByAliasOnly(alias);
                if (project != null)
                {
                    projects.RemoveAlias(alias);
                    redis.RemoveAliasForProject(project.Name, alias);
                    ircClient.Message("#Piroket", string.Format("Alias {0} removed for {1}", alias, project.Name));
                }
                else
                {
                    ircClient.Message("#Piroket", string.Format("No alias {0} exists", alias));
                }
            }
            else
            {
                ircClient.Message("#Piroket", "Usage: .removeAlias [alias]");
            }
        }
    }
}