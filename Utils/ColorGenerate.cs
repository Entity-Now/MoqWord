using ColorHelper;
using MoqWord.Components.Components.RandomColor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Utils
{
    public static class ColorGenerate
    {
        private static Random Random = new Random(DateTime.Now.Second);

        public static T GetRandomColor<T>() where T : IColor
        {
            return GetRandomColor<T>(new RgbRandomColorFilter());
        }

        public static T GetLightRandomColor<T>() where T : IColor
        {
            return GetRandomColor<T>(new RgbRandomColorFilter
            {
                minR = 170,
                minG = 170,
                minB = 170
            });
        }

        public static T GetDarkRandomColor<T>() where T : IColor
        {
            return GetRandomColor<T>(new RgbRandomColorFilter
            {
                maxR = 80,
                maxG = 80,
                maxB = 80
            });
        }

        private static T GetRandomColor<T>(RgbRandomColorFilter filter) where T : IColor
        {
            return ConvertRgbToNecessaryColorType<T>(new RGB((byte)Random.Next(filter.minR, filter.maxR), (byte)Random.Next(filter.minG, filter.maxG), (byte)Random.Next(filter.minB, filter.maxB)));
        }

        private static T ConvertRgbToNecessaryColorType<T>(RGB rgb) where T : IColor
        {
            if (typeof(T) == typeof(RGB))
            {
                return (T)(object)rgb;
            }

            if (typeof(T) == typeof(HEX))
            {
                return (T)(object)ColorConverter.RgbToHex(rgb);
            }

            if (typeof(T) == typeof(CMYK))
            {
                return (T)(object)ColorConverter.RgbToCmyk(rgb);
            }

            if (typeof(T) == typeof(HSV))
            {
                return (T)(object)ColorConverter.RgbToHsv(rgb);
            }

            if (typeof(T) == typeof(HSL))
            {
                return (T)(object)ColorConverter.RgbToHsl(rgb);
            }

            if (typeof(T) == typeof(XYZ))
            {
                return (T)(object)ColorConverter.RgbToXyz(rgb);
            }

            if (typeof(T) == typeof(YIQ))
            {
                return (T)(object)ColorConverter.RgbToYiq(rgb);
            }

            if (typeof(T) == typeof(YUV))
            {
                return (T)(object)ColorConverter.RgbToYuv(rgb);
            }

            throw new ArgumentException("Incorrect class type");
        }
    }
}
