using System.Collections.Generic;
using NetIrc2;
using NetIrc2.Events;

namespace ShimabuttsIrcBot.Commands
{
    public class NotImplementedCommand : BotCommand
    {
        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, Dictionary<string, Project.Project> projects, ShimabuttsRedis redis)
        {
            ircClient.Message("#Piroket", "Not yet implemented.");
        }
    }
}