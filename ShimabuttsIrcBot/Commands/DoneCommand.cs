using NetIrc2;
using NetIrc2.Events;
using ShimabuttsIrcBot.Projects;

namespace ShimabuttsIrcBot.Commands
{
    public class DoneCommand : BotCommand
    {
        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, ProjectsWithAlias projects)
        {
            var splits = eventArgs.Message.ToString().Split(' ');
            if (splits.Length == 3)
            {
                if (projects.HasProject(splits[1]))
                {
                    var role = splits[2].ParseStringToRole();
                    if (!role.HasValue)
                    {
                        ircClient.Message("#Piroket", "wtf role is that");
                        return;
                    }
                    var project = projects.GetByName(splits[1]);
                    project.SetAsDone(role.Value);
                    var waitingAtRole = project.WaitingAt();
                    if (waitingAtRole.HasValue)
                    {
                        ircClient.Message("#Piroket", string.Format("{0} is done for {1}. Waiting on {2} - {3}",
                            role,
                            project.Name,
                            project.WaitingAt(),
                            string.Join(",", project.CheckProjectForRole(waitingAtRole.Value)))
                            );

                    }
                    else
                    {
                        ircClient.Message("#Piroket", string.Format("{0} is done!", project.Name));
                    }
                }
                else
                {
                    ircClient.Message("#Piroket", "No project by that name.");
                }
            }
            else
            {
                ircClient.Message("#Piroket", "Usage: .done [project] [role]");
            }
        }
    }
}