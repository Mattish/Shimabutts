using System.Collections.Generic;
using NetIrc2;
using NetIrc2.Events;
using ShimabuttsIrcBot.Projects;

namespace ShimabuttsIrcBot.Commands
{
    public class NewAnimeCommand : BotCommand
    {
        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, ProjectsWithAlias projects)
        {
            var splits = eventArgs.Message.ToString().Split(' ');
            if (splits.Length == 2)
            {
                if (!projects.HasProject(splits[1]))
                {
                    projects.Add(splits[1], false);
                    ircClient.Message("#Piroket", string.Format("Created new Anime project {0}", splits[1]));
                }
                else
                {
                    ircClient.Message("#Piroket", "Project by that name already exists.");
                }
            }
            else
            {
                ircClient.Message("#Piroket", "Usage: .newAnime [project]");
            }
        }
    }
}