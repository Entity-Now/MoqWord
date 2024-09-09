using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;

namespace MoqWord.Helpers
{
    //class Program
    //{
    //    static void Main()
    //    {
    //        // 获取嵌入资源文件
    //        string embeddedResourceContent = ResourceHelper.GetEmbeddedResource("YourNamespace.YourResourceFile.txt");
    //        Console.WriteLine(embeddedResourceContent);

    //        // 获取资源文件
    //        string resourceContent = ResourceHelper.GetResource("pack://application:,,,/YourResourceFile.xaml");
    //        Console.WriteLine(resourceContent);

    //        // 获取内容文件
    //        string contentFileContent = ResourceHelper.GetContentFile("pack://application:,,,/YourContentFile.txt");
    //        Console.WriteLine(contentFileContent);
    //    }
    //}
    public static class ResourceHelper
    {
        /// <summary>
        ///  获取嵌入资源文件
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static Stream GetEmbeddedResourceStream(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new FileNotFoundException("Resource not found: " + resourceName);

            return stream;
        }
        /// <summary>
        /// 获取嵌入的资源文件，base64图片
        /// </summary>
        /// <returns></returns>
        public static string GetEmbeddedImage(string resourceName)
        {
            var stream = GetEmbeddedResourceStream(resourceName);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                var imageBytes = memoryStream.ToArray();
                var base64Image = Convert.ToBase64String(imageBytes);
                return $"data:image/png;base64,{base64Image}";
            }
        }

        /// <summary>
        /// 获取资源文件
        /// </summary>
        /// <param name="relativeUri"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string GetResource(string relativeUri)
        {
            Uri uri = new Uri(relativeUri, UriKind.RelativeOrAbsolute);
            StreamResourceInfo resourceInfo = Application.GetResourceStream(uri);

            if (resourceInfo == null)
                throw new FileNotFoundException("Resource not found: " + relativeUri);

            using (StreamReader reader = new StreamReader(resourceInfo.Stream))
            {
                return reader.ReadToEnd();
            }
        }
        /// <summary>
        /// 获取内容文件
        /// </summary>
        /// <param name="relativeUri"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string GetContentFile(string relativeUri)
        {
            Uri uri = new Uri(relativeUri, UriKind.RelativeOrAbsolute);
            StreamResourceInfo resourceInfo = Application.GetContentStream(uri);

            if (resourceInfo == null)
                throw new FileNotFoundException("Content file not found: " + relativeUri);

            using (StreamReader reader = new StreamReader(resourceInfo.Stream))
            {
                return reader.ReadToEnd();
            }
        }
        /// <summary>
        /// 获取指定路径的文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileText(string filePath)
        {
            var file = new FileInfo(filePath);
            if (file.Exists)
            {
                var readStream = file.OpenText();
                return readStream.ReadToEnd();
            }
            return string.Empty;
        }
    }
}
