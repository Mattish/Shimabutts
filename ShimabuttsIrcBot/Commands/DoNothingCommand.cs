using System.Collections.Generic;
using NetIrc2;
using NetIrc2.Events;
using ShimabuttsIrcBot.Projects;

namespace ShimabuttsIrcBot.Commands
{
    public class DoNothingCommand : BotCommand
    {
        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, ProjectsWithAlias projects)
        {

        }
    }
}