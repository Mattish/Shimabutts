using System.Collections.Generic;
using NetIrc2;
using NetIrc2.Events;

namespace ShimabuttsIrcBot.Commands
{
    public class NewMangoCommand : BotCommand
    {
        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, Dictionary<string, Project.Project> projects, ShimabuttsRedis redis)
        {
            var splits = eventArgs.Message.ToString().Split(' ');
            if (splits.Length == 2)
            {
                if (!projects.ContainsKey(splits[1]))
                {
                    projects.Add(splits[1], new Project.Project(splits[1]));
                    ircClient.Message("#Piroket", string.Format("Created new Mango project {0}", splits[1]));
                    redis.AddMangoProject(splits[1]);
                }
                else
                {
                    ircClient.Message("#Piroket", "Project by that name already exists.");
                }
            }
            else
            {
                ircClient.Message("#Piroket", "Usage: .newMango [project]");
            }
        }
    }
}