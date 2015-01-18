using NetIrc2;
using NetIrc2.Events;
using ShimabuttsIrcBot.Projects;

namespace ShimabuttsIrcBot.Commands
{
    public class AddAliasCommand : BotCommand
    {
        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, ProjectsWithAlias projects,
            ShimabuttsRedis redis)
        {
            var splits = eventArgs.Message.ToString().Split(' ');
            if (splits.Length == 3)
            {
                var originalName = splits[1];
                var alias = splits[2];

                var project = projects.GetByActualNameOnly(originalName);
                if (project != null)
                {
                    projects.AddAlias(originalName, alias);
                    ircClient.Message("#Piroket", string.Format("Project {0} now available by {1}", originalName, alias));
                }
                else
                {
                    ircClient.Message("#Piroket", string.Format("No project by the name {0}", originalName));
                }
            }
            else
            {
                ircClient.Message("#Piroket", "Usage: .addAlias [project] [alias]");
            }
        }
    }
}