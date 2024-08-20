using Highgeek.McWebApp.Common.Models.Minecraft.DisplayName;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Helpers
{
    public class ColorAdapter
    {
        public static string ToHtmlString(string input)
        {
            string output = "";
            input = input.Replace("§", "&");
            if (input.Contains("&"))
            {
                string[] strings = input.Split("&");
                foreach (string s in strings)
                {
                    if (s.Length > 1)
                    {
                        output += "<span style= \"font-family:'Minecraft';color:" + GetColorFromMinecraftCode(s.Substring(0, 1)) + ";\">" + s.Substring(1, s.Length - 1) + "</span>";
                    }
                }
                return output;
            }
            else
            {
                return input;
            }
        }

        public static string GetColorFromMinecraftCode(string input)
        {
            switch(input)
            {
                case "0":
                    return "#000000";
                case "1":
                    return "#1166FF";
                case "2":
                    return "#00AA00";
                case "3":
                    return "#00AAAA";
                case "4":
                    return "#FF2000";
                case "5":
                    return "#AA00AA";
                case "6":
                    return "#FFAA00";
                case "7":
                    return "#AAAAAA";
                case "8":
                    return "#555555";
                case "9":
                    return "#5555FF";
                case "a":
                    return "#55FF55";
                case "b":
                    return "#55FFFF";
                case "c":
                    return "#FF5555";
                case "d":
                    return "#FF55FF";
                case "e":
                    return "#FFFF55";
                case "f":
                    return "#FFFFFF";
                default:
                    return GetOtherFormating(input);
            }
        }
        public static string GetOtherFormating(string input)
        {
            return "";
        }
    }
}
