using System.Collections.Generic;
using System.IO;

namespace WHBKYK
{
    public class ReadJson
    {
        private StreamReader[] sr = null;
        private string[] key = null;
        private string[] value = null;
        private List<string> types = null;
        private int count = 0;

        public ReadJson(string path)
        {
            types = new List<string>();
            sr = new StreamReader[2];

            sr[0] = new StreamReader(path + "/key.txt");
            sr[1] = new StreamReader(path + "/value.txt");
        }


        public string JsonParsing()
        {
            string result = "";

            key = sr[0].ReadToEnd().Split(' ');
            value = sr[1].ReadToEnd().Split('\n');

            for(int i = 0; i < key.Length - 1; i++)
            {
                string[] strArr = value[i].Split(':');
                if (strArr.Length > 2)
                {
                    types.Add(strArr[strArr.Length - 1].Replace("<class ", "").Replace(">", "").Replace("'", ""));
                    result += (key[i] + "\n{ \n\t");
                    for (int j = 0; j < strArr.Length - 1; j++)
                    {
                        result += strArr[j];
                    }

                    result += "\n}\n\n";
                }
                else
                {
                    types.Add(strArr[1].Replace("<class ", "").Replace(">", "").Replace("'", ""));
                    result += (key[i] + "\n{ \n\t") + strArr[0].Replace(",", "\n\t").Replace("[", "").Replace("]", "") + "\n}\n\n";
                }
            }

            for(int i = 0; i < sr.Length; i++)
            {
                sr[i].Close();
            }

            return result;
        }

    }
}


