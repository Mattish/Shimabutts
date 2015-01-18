
using ShimabuttsIrcBot.Projects;

namespace ShimabuttsIrcBot
{
    public static class Extensions
    {
        public static Role? ParseStringToRole(this string roleName)
        {
            switch (roleName)
            {
                case "CL":
                    return Role.CL;
                case "TL":
                    return Role.TL;
                case "TLC":
                    return Role.TLC;
                case "ED":
                    return Role.ED;
                case "TM":
                    return Role.TM;
                case "RD":
                    return Role.RD;
                case "TS":
                    return Role.TS;
                case "QC":
                    return Role.QC;
                default:
                    return null;
            }
        }
    }
}