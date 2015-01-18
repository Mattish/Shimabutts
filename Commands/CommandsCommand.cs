using System.Collections.Generic;
using NetIrc2;
using NetIrc2.Events;

namespace ShimabuttsIrcBot.Commands
{
    public class CommandsCommand : BotCommand
    {
        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, Dictionary<string, Project.Project> projects, ShimabuttsRedis redis)
        {
            ircClient.Notice(eventArgs.Sender.Nickname, ".newMango [project] | .newAnime [project] | .assign [project] [victim] [role] | .blame [project] | .commands | .done [project] |" +
                                                        " .ftpinfo | .notify [victim] [message] | .whatdo [victim] | .whodoes [project] | .mattishpls [message or request plsnospam]");
        }
    }
}