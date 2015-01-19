using System.Collections.Generic;
using NetIrc2;
using NetIrc2.Events;
using ShimabuttsIrcBot.Projects;

namespace ShimabuttsIrcBot.Commands
{
    public class BlameCommand : BotCommand
    {
        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, ProjectsWithAlias projects)
        {
            var splits = eventArgs.Message.ToString().Split(' ');
            if (splits.Length == 2)
            {
                if (projects.HasProject(splits[1]))
                {
                    var project = projects.GetByName(splits[1]);
                    var waitingAtRole = project.WaitingAt();
                    if (waitingAtRole.HasValue)
                    {
                        var waitingForHowLong = project.WaitingForHowLong();
                        ircClient.Message("#Piroket", string.Format("{0} is waiting on {1}, {2} for {3}",
                            project.Name,
                            project.WaitingAt(),
                            string.Join(",", project.CheckProjectForRole(waitingAtRole.Value)),
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