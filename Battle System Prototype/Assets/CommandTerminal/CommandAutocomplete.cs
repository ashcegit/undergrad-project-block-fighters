using System.Collections.Generic;

namespace CommandTerminal
{
    public class CommandAutocomplete
    {
        List<string> knownWords=new List<string>();
        List<string> buffer=new List<string>();

        public void register(string word) {
            knownWords.Add(word);
        }

        public string[] complete(ref string text) {
            string partialWord = eatLastWord(ref text);
            string known;
            buffer.Clear();

            for (int i=0;i<knownWords.Count;i++) {
                known=knownWords[i];
                if (known.StartsWith(partialWord)) {buffer.Add(known);}
            }
            return buffer.ToArray();
        }

        string eatLastWord(ref string text) {
            int last_space = text.LastIndexOf(' ');
            string result = text.Substring(last_space + 1);
            text = text.Substring(0, last_space + 1); // Remaining (keep space)
            return result;
        }
    }
}
