using System;
using System.Text.RegularExpressions;

namespace StringHandler 
{
    public static class StatColorHandler
    {
        public static string DamagePaint(object text)
        {
            text = text.ToString();
            var container = "<#ff5cff>" + text + "</color>";
            return container;
        }

        public static string RestPaint(object text)
        {
            text = text.ToString();
            var container = "<#00f7ff>" + text + "</color>";
            return container;
        }

        public static string HealthPaint(object text)
        {
            text = text.ToString();
            var container = "<#009d4a>" + text + "</color>";
            return container;
        }

        public static string StatPaint(object text)
        {
            text = text.ToString();
            var container = "<#d0ff00>" + text + "</color>";
            return container;
        }
    }

    public static class KeywordHandler
    {
        public static string KeywordPaint(Keyword keyword)
        {
            var container = "<#c46f00><i><lowercase>" + keyword.ToString() + "</lowercase></i></color>";
            return container;
        }

        public static string KeywordDescription(Keyword keyword)
        {
            var desc = DescriptionDictionary.Main.GetDescription(keyword.ToString());
            var container = KeywordPaint(keyword) + "\n" + desc + "\n\n";
            return container;
        }
    }

    public static class ExtensionMethods
    {
        public static string ToSplittedString(this Enum target)
        {
            string[] split =  Regex.Split(target.ToString(), @"(?<!^)(?=[A-Z])");
            var container = string.Empty;
            foreach(string str in split)
            {
                container += str + " ";
            }
            return container;
        }
    }
}