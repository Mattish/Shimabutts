using System.Collections.Generic;
using NetIrc2;
using NetIrc2.Events;

namespace ShimabuttsIrcBot.Commands
{
    public class AssignCommand : BotCommand
    {
        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, Dictionary<string, Project.Project> projects, ShimabuttsRedis redis)
        {
            var splits = eventArgs.Message.ToString().Split(' ');
            if (splits.Length == 4)
            {
                if (!projects.ContainsKey(splits[1]))
                {
                    ircClient.Message("#Piroket", "No project by that name.");
                    return;
                }
                var role = splits[3].ParseStringToRole();
                if (role.HasValue)
                {
                    var result = projects[splits[1]].AddNameToRole(splits[2], role.Value);
                    ircClient.Message("#Piroket", result.ToString());
                    redis.AddNameToRole(splits[1], splits[2], role.Value);
                    redis.SetTimeWaiting(splits[1], projects[splits[1]].GetDateTime());
                }
                else
                {
                    ircClient.Message("#Piroket", "wtf role is that");
                }
            }
            else
            {
                ircClient.Message("#Piroket", "Usage: .assign [project] [victim] [Role]");
            }
        }
    }
}