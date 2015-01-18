using System;
using System.Collections.Generic;
using NetIrc2;
using NetIrc2.Events;
using ShimabuttsIrcBot.Project;

namespace ShimabuttsIrcBot.Commands
{
    public class RolesCommand : BotCommand
    {
        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, Dictionary<string, Project.Project> projects, ShimabuttsRedis redis)
        {
            ircClient.Message("#Piroket", string.Join(",", Enum.GetValues(typeof(Role)).ToString()));
        }
    }
}