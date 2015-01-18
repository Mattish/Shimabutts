using System.Collections.Generic;
using NetIrc2;
using NetIrc2.Events;

namespace ShimabuttsIrcBot.Commands
{
    public class WhoDoesCommand : BotCommand
    {
        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, Dictionary<string, Project.Project> projects, ShimabuttsRedis redis)
        {
            var splits = eventArgs.Message.ToString().Split(' ');
            if (splits.Length == 2)
            {
                if (projects.ContainsKey(splits[1]))
                {
                    ircClient.Message("#Piroket", projects[splits[1]].GetSummary());
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