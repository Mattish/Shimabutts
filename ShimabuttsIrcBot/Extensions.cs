
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
                case "cl":
                    return Role.CL;
                case "TL":
                case "tl":
                    return Role.TL;
                case "TLC":
                case "tlc":
                    return Role.TLC;
                case "ED":
                case "ed":
                    return Role.ED;
                case "TM":
                case "tm":
                    return Role.TM;
                case "RD":
                case "rd":
                    return Role.RD;
                case "TS":
                case "ts":
                    return Role.TS;
                case "QC":
                case "qc":
                    return Role.QC;
                default:
                    return null;
            }
        }
    }
}