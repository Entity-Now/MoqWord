using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MoqWord.Extensions
{
    public static class TextExpand
    {
        /// <summary>
        /// 使用正则表达式分割字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string SplitChar(this string source, string pattern)
        {
            return Regex.Split(source, pattern)[0];
        }
        /// <summary>
        /// 根据汉字和英文字符的数量计算读出文本所需的秒数。
        /// </summary>
        /// <param name="text">输入的文本。</param>
        /// <param name="speechSpeed">播放速度</param>
        /// <returns>读出文本所需的秒数。</returns>
        public static double CalculateReadingTime(this string text, double speechSpeed)
        {
            // 限制 speechSpeed 在 -100 到 100 之间
            speechSpeed = Math.Max(-100, Math.Min(100, speechSpeed));

            // 将 speechSpeed 映射到 0.5 到 2 的范围
            double speedFactor = 1.5 - (speechSpeed / 200.0);

            // 基础发音时间（秒）
            double baseTimePerChineseChar = 0.25;
            double baseTimePerEnglishChar = 0.2;

            // 调整后的发音时间
            double timePerChineseChar = baseTimePerChineseChar * speedFactor;
            double timePerEnglishChar = baseTimePerEnglishChar * speedFactor;

            int chineseCharCount = 0;
            int englishCharCount = 0;

            // 遍历文本中的每个字符
            foreach (char c in text)
            {
                if (IsChinese(c))
                {
                    chineseCharCount++;
                }
                else if (IsEnglish(c))
                {
                    englishCharCount++;
                }
                else
                {
                    englishCharCount++;
                }
            }

            // 计算总时间
            double totalTime = (chineseCharCount * timePerChineseChar) + (englishCharCount * timePerEnglishChar);
            return totalTime;
        }

        /// <summary>
        /// 判断字符是否为汉字。
        /// </summary>
        /// <param name="c">输入的字符。</param>
        /// <returns>如果是汉字，则返回true；否则返回false。</returns>
        private static bool IsChinese(char c)
        {
            return c >= 0x4E00 && c <= 0x9FFF;
        }

        /// <summary>
        /// 判断字符是否为英文字符。
        /// </summary>
        /// <param name="c">输入的字符。</param>
        /// <returns>如果是英文字符，则返回true；否则返回false。</returns>
        private static bool IsEnglish(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

    }
}
