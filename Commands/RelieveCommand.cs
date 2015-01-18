using System.Collections.Generic;
using NetIrc2;
using NetIrc2.Events;
using ShimabuttsIrcBot.Projects;

namespace ShimabuttsIrcBot.Commands
{
    public class RelieveCommand : BotCommand
    {
        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, ProjectsWithAlias projects, ShimabuttsRedis redis)
        {
            var splits = eventArgs.Message.ToString().Split(' ');
            if (splits.Length == 4)
            {
                if (!projects.HasProject(splits[1]))
                {
                    ircClient.Message("#Piroket", "No project by that name.");
                    return;
                }
                var role = splits[3].ParseStringToRole();
                if (role.HasValue)
                {
                    var result = projects[splits[1]].RemoveNameFromRole(splits[2], role.Value);
                    ircClient.Message("#Piroket", result.ToString());
                    redis.RemoveNameFromRole(splits[1], splits[2], role.Value);
                    redis.SetTimeWaiting(splits[1], projects[splits[1]].GetDateTime());
                }
                else
                {
                    ircClient.Message("#Piroket", "wtf role is that");
                }
            }
            else
            {
                ircClient.Message("#Piroket", "Usage: .relieve [project] [victim] [Role]");
            }
        }
    }
}