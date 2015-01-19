using System.Configuration;

namespace ShimabuttsIrcBot
{
    class Program
    {
        private static void Main()
        {
            var settings = new ShimabuttsSettings
            {
                IrcServer = ConfigurationManager.AppSettings["IrcServer"],
                Nick = ConfigurationManager.AppSettings["Nick"],
                IdentPass = ConfigurationManager.AppSettings["IdentPass"],
                Channel = ConfigurationManager.AppSettings["Channel"],
                ChannelKey = ConfigurationManager.AppSettings["ChannelKey"],
            };

            var shimabutts = new ShimabuttsBot(settings);
            shimabutts.Start();
        }
    }

}
