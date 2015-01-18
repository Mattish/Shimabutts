using System;
using System.Collections.Generic;
using System.Threading;
using NetIrc2;
using NetIrc2.Events;

namespace ShimabuttsIrcBot
{
    public class ShimabuttsBot
    {
        private readonly ShimabuttsSettings _settings;
        private readonly IrcClient _client;
        private ShimabuttsRedis _redisButts;
        private readonly Dictionary<string, Project.Project> _projects = new Dictionary<string, Project.Project>();

        public ShimabuttsBot(ShimabuttsSettings settings)
        {
            _settings = settings;
            _client = new IrcClient();
            _client.Connect(settings.IrcServer);
            Console.WriteLine("Connecting to {0}", settings.IrcServer);
            _client.GotNotice += _client_GotNotice;
            _client.Connected += client_Connected;
            _client.GotIrcError += _client_GotIrcError;
            _client.GotMessage += _client_GotMessage;
        }

        public void Start()
        {
            _redisButts = new ShimabuttsRedis();
            foreach (var project in _redisButts.GetAllProjects())
            {
                _projects.Add(project.Name, project);
                Console.WriteLine("Loaded project {0}", project.Name);
            }

            _client.Connect(_settings.IrcServer);
            while (true)
            {
                Thread.Sleep(1);
            }
        }

        private void _client_GotMessage(object sender, ChatMessageEventArgs e)
        {
            var command = CommandTranslator.TranslateToCommand(e);
            command.RunCommand(e, _client, _projects, _redisButts);
        }

        private void _client_GotNotice(object sender, ChatMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private void _client_GotIrcError(object sender, IrcErrorEventArgs e)
        {
            Console.WriteLine(e.Error);
        }

        private void client_Connected(object sender, EventArgs e)
        {
            Console.WriteLine(@"Client connected");
            Thread.Sleep(1000);
            Console.WriteLine(@"Ghosting...");
            _client.Message("NickServ", string.Format("GHOST {0} {1}", _settings.Nick, _settings.IdentPass));
            Thread.Sleep(1000);
            Console.WriteLine(@"Set Nick...");
            _client.LogIn(_settings.Nick, _settings.Nick, _settings.Nick);
            Thread.Sleep(1000);
            Console.WriteLine(@"Set IDENT...");
            _client.Message("NickServ", string.Format("IDENTIFY {0}", _settings.IdentPass));
            Thread.Sleep(1000);
            Console.WriteLine(@"Set IDENT...");
            _client.Message("HostServ", "ON");
            Thread.Sleep(1000);
            _client.Join(_settings.Channel, _settings.ChannelKey);
            _client.Message(_settings.Channel, string.Format("Loaded up with {0} projects", _projects.Count));
        }
    }
}