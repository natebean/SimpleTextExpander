using System;
using System.Linq;
using Keystroke.API;
using System.Configuration;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SimpleTextExpander
{
    class KeyHandler
    {
        string keyBuffer = "";
        NameValueCollection hotStrings;
        KeystrokeAPI api;

        public KeyHandler()
        {
            api = new KeystrokeAPI();
            api.CreateKeyboardHook((character) => { bool ans = KeyProcessor(character); return ans; });

            // Read all the keys from the config file
            hotStrings = ConfigurationManager.AppSettings;
            foreach (var key in hotStrings.AllKeys)
            {
                Console.WriteLine("Key: {0} Value: {1}", key, hotStrings[key]);
            }
            Console.WriteLine("The plus sign(+), caret(^), percent sign(%), tilde(~), and parentheses() have special meanings to SendKeys.To specify one of these characters, enclose it within braces({ }). For example, to specify the plus sign, use \"{+}\"");
        }

        public bool KeyProcessor(Keystroke.API.CallbackObjects.KeyPressed character)
        {

            string chrString = character.ToString();
            string backspace = "<backspace>";

            if (!chrString.Equals("<shift>") && !chrString.Equals(backspace))
            {
                keyBuffer += character;
                //Console.Write(character);
            }

            if (keyBuffer.Length > 25)
            {
                keyBuffer = keyBuffer.Remove(0, 1);
            }

            bool passOn = true;
            passOn = CheckForHotString();
            return passOn;
        }

        private Boolean CheckForHotString()
        {
            var passOn = true;

            foreach (var key in hotStrings.AllKeys)
            {
                //Console.WriteLine("Key: {0} Value: {1}", key, hotStrings[key]);
                var nowPassOn = HotStringRunner(key, hotStrings[key]);
                if (!nowPassOn) return nowPassOn;//we found a match, move on
            }

            return passOn;
        }

        private bool HotStringRunner(string key, string value)
        {
            string pattern = key;
            string result = value;
            int patternLength = pattern.Length;
            Boolean passOn = true;
            //Console.WriteLine(keyBuffer + " : " + pattern);
            Match m = Regex.Match(keyBuffer, pattern);
            if (m.Success)
            {
                api.interceptOn = false;
                Console.WriteLine(key + " : " + value);
                var prefix = string.Concat(Enumerable.Repeat("{BS}", patternLength - 1));
                SendKeys.SendWait(prefix + result);
                api.interceptOn = true;
                keyBuffer = "";
                passOn = false;
            }

            return passOn;
        }

        private string CleanUpSendValue(string value)
        {
            //The plus sign(+), caret(^), percent sign(%), tilde(~), and parentheses() have special meanings to SendKeys.To specify one of these characters, enclose it within braces({ }). For example, to specify the plus sign, use "{+}".



            return "test";

        }
    }//class

}
