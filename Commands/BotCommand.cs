using System.Collections.Generic;
using NetIrc2;
using NetIrc2.Events;
using ShimabuttsIrcBot.Projects;

namespace ShimabuttsIrcBot.Commands
{
    public abstract class BotCommand
    {
        public void RunCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, ProjectsWithAlias projects, ShimabuttsRedis redis)
        {
            SpecificCommand(eventArgs, ircClient, projects, redis);
        }

        protected abstract void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, ProjectsWithAlias projects, ShimabuttsRedis redis);
    }
}