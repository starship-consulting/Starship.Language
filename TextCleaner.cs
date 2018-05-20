using System;
using System.Text.RegularExpressions;

namespace Starship.Language {
    public static class TextCleaner {

        public static string Clean(string input) {
            return input.ToLower().Replace("\t", " ");
        }

        public static string[] CleanSplit(string token, string input) {
            return Clean(input).Split(new[] { token }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string RemoveInvalidCharacters(string input) {
            var rgx = new Regex("[^a-zA-Z0-9 -]");
            return rgx.Replace(input, "");
        }
    }
}