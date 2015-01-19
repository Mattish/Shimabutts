using System.Collections.Generic;
using NetIrc2;
using NetIrc2.Events;
using ShimabuttsIrcBot.Projects;

namespace ShimabuttsIrcBot.Commands
{
    public class WhoDoesCommand : BotCommand
    {
        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, ProjectsWithAlias projects)
        {
            var splits = eventArgs.Message.ToString().Split(' ');
            if (splits.Length == 2)
            {
                if (projects.HasProject(splits[1]))
                {
                    ircClient.Message("#Piroket", projects.GetByName(splits[1]).GetSummary());
                }
                else
                {
                    ircClient.Message("#Piroket", "No project by that name.");
                }
            }
            else
            {
                ircClient.Message("#Piroket", "Usage: .whodoes [project]");
            }
        }
    }
}