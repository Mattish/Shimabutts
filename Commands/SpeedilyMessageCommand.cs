using System;
using System.Collections.Generic;
using NetIrc2;
using NetIrc2.Events;
using ShimabuttsIrcBot.Projects;

namespace ShimabuttsIrcBot.Commands
{
    public class SpeedilyMessageCommand : BotCommand
    {
        private string _name;
        public SpeedilyMessageCommand(string name)
        {
            _name = name;
        }

        protected override void SpecificCommand(ChatMessageEventArgs eventArgs, IrcClient ircClient, ProjectsWithAlias projects, ShimabuttsRedis redis)
        {
            var splits = eventArgs.Message.ToString().Split(' ');
            if (splits.Length > 1)
            {
                Console.WriteLine("{2}pls - {0}: {1}", eventArgs.Sender, eventArgs.Message, _name.ToLower());
                ircClient.Message(_name,
                    string.Format("{0} - {1}", eventArgs.Sender, string.Join(" ", splits, 1, splits.Length - 1)));
                ircClient.Message("#Piroket", string.Format("Speedily sent your request to {0}!", _name));
            }
            else
                ircClient.Message("#Piroket", string.Format("Usage: .{0}pls [comment/request]", _name.ToLower()));
        }
    }
}