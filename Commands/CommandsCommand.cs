using NetIrc2;
using NetIrc2.Events;
using ShimabuttsIrcBot.Projects;

namespace ShimabuttsIrcBot.Commands
{
    public class CommandsCommand : BotCommand
    {
        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, ProjectsWithAlias projects)
        {
            ircClient.Notice(eventArgs.Sender.Nickname, ".newMango [project] | .newAnime [project] | .assign [project] [victim] [role] | .blame [project] | .commands | .done [project] |" +
                                                        " .ftpinfo | .notify [victim] [message] | .whatdo [victim] | .addAlias [project] [alias] | .removeAlias [alias] | .whodoes [project] | .mattishpls [message or request plsnospam]");
        }
    }
}