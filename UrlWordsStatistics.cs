using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Backend_1
{
    public class UrlWordsStatisticsMethod : IUrlStatisticsMethod
    {
        private class WordsStatisticsData
        {
            public string Word { get; private set; }
            public int Count { get; private set; }

            public WordsStatisticsData(string word, int count)
            {
                Word = word;
                Count = count;
            }

            public override string ToString()
            {
                return $"{Word} - {Count}";
            }
        }

        public string GetStatistics(string htmlCode)
        {
            htmlCode = RemoveTags(htmlCode);

            string[] words = GetTextWords(htmlCode);
            List<WordsStatisticsData> data = GetWordsCount(words);

            string statistics = string.Join("\n", data);
            statistics += $"\nОбщее количество найденных уникальных слов: {data.Count}";

            return statistics;
        }

        private string RemoveTags(string htmlCode)
        {
            string[] removePatterns =
            {
                @"<script.*?>.*?<\/script>",
                @"<style.*?>.*?<\/style>",
                @"<\/?.*?>",
                @"\s+"
            };

            for (int i = 0; i < removePatterns.Length; i++)
            {
                Regex rgx = new Regex(removePatterns[i], RegexOptions.Singleline);
                htmlCode = rgx.Replace(htmlCode, " ");
            }

            return htmlCode;
        }

        private string[] GetTextWords(string htmlCode)
        {
            string[] words = htmlCode.Split(' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t', '=', '/');
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = words[i].ToLower();
            }

            return words;
        }

        private List<WordsStatisticsData> GetWordsCount(string[] words)
        {            
            List<WordsStatisticsData> dataList = new List<WordsStatisticsData>();

            List<string> checkedWords = new List<string>(words.Length);
            checkedWords.Add("");

            foreach (var word in words)
            {
                if (checkedWords.Contains(word) || WordIsWrong(word))
                {
                    continue;
                }

                checkedWords.Add(word);

                WordsStatisticsData data = new WordsStatisticsData(word, words.Count(str => str == word));
                dataList.Add(data);
            }

            dataList = dataList.OrderBy(data => data.Count).ThenBy(data => data.Word).Reverse().ToList();

            return dataList;
        }

        private bool WordIsWrong(string word)
        {
            if (!Regex.IsMatch(word, @"[a-zA-Zа-яА-Я]"))
            {
                return true;
            }

            string[] wrongPatterns =
            {
                @"[^a-zA-Zа-яА-Я0-9]",
                @"\d\d"
            };

            foreach (var pattern in wrongPatterns)
            {
                if (Regex.IsMatch(word, pattern))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
