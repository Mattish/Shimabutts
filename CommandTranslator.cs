using NetIrc2.Events;
using ShimabuttsIrcBot.Commands;

namespace ShimabuttsIrcBot
{
    sealed public class CommandTranslator
    {
        public static BotCommand TranslateToCommand(ChatMessageEventArgs e)
        {
            var messageSplits = e.Message.ToString().Split(' ');
            if (messageSplits.Length > 0 && messageSplits[0].StartsWith("."))
            {
                var messageCommand = messageSplits[0];
                switch (messageCommand)
                {
                    case ".assign":
                        return new AssignCommand();
                    case ".roles":
                        return new RolesCommand();
                    case ".newMango":
                        return new NewMangoCommand();
                    case ".newAnime":
                        return new NewAnimeCommand();
                    case ".relieve":
                        return new RelieveCommand();
                    case ".blame":
                        return new BlameCommand();
                    case ".commands":
                        return new CommandsCommand();
                    case ".done":
                        return new DoneCommand();
                    case ".undone":
                        return new UndoneCommand();
                    case ".ftpinfo":
                        return new NotImplementedCommand();
                    case ".notify":
                        return new NotImplementedCommand();
                    case ".whatdo":
                        return new NotImplementedCommand();
                    case ".whodoes":
                        return new WhoDoesCommand();
                    case ".mattishpls":
                        return new SpeedilyMessageCommand("Mattish");
                    case ".kuwapls":
                        return new SpeedilyMessageCommand("Kuwagata");
                    case ".piropls":
                        return new SpeedilyMessageCommand("Piroko");
                    default:
                        return new DoNothingCommand();

                }
            }
            return new DoNothingCommand();
        }
    }
}