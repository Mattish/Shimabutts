using System.Collections.Generic;
using NetIrc2;
using NetIrc2.Events;

namespace ShimabuttsIrcBot.Commands
{
    public class BlameCommand : BotCommand
    {
        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, Dictionary<string, Project.Project> projects, ShimabuttsRedis redis)
        {
            var splits = eventArgs.Message.ToString().Split(' ');
            if (splits.Length == 2)
            {
                if (projects.ContainsKey(splits[1]))
                {
                    var waitingAtRole = projects[splits[1]].WaitingAt();
                    if (waitingAtRole.HasValue)
                    {
                        var waitingForHowLong = projects[splits[1]].WaitingForHowLong();
                        ircClient.Message("#Piroket", string.Format("{0} is waiting on {1}, {2} for {3}",
                            splits[1],
                            projects[splits[1]].WaitingAt(),
                            string.Join(",", projects[splits[1]].CheckProjectForRole(waitingAtRole.Value)),
                            string.Format("{0} days, {1} hours and {2} minutes.", waitingForHowLong.Days, waitingForHowLong.Hours, waitingForHowLong.Minutes))
                            );
                    }
                    else
                    {
                        ircClient.Message("#Piroket", string.Format("{0} is done!", splits[1]));
                    }
                }
                else
                {
                    ircClient.Message("#Piroket", "No project by that name.");
                }
            }
            else
            {
                ircClient.Message("#Piroket", "Usage: .blame [project]");
            }
        }
    }
}