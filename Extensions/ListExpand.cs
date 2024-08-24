using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Extensions
{
    public static class ListExpand
    {
        /// <summary>
        /// 判断一个数组中是否包含另一个数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mainArray"></param>
        /// <param name="subArray"></param>
        /// <returns></returns>
        public static bool ContainsSubArray<T>(this T[] mainArray, T[] subArray)
        {
            if (subArray.Length == 0 || mainArray.Length < subArray.Length)
                return false;

            for (int i = 0; i <= mainArray.Length - subArray.Length; i++)
            {
                if (mainArray.Skip(i).Take(subArray.Length).SequenceEqual(subArray))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
