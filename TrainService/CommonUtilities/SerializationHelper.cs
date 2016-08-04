using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;


namespace CommonUtilities
{
    public class SerializationHelper
    {
        #region De-serialize

        public static T DeserializeFromBin<T>(Stream stream) where T : class
        {
            var formatter = new BinaryFormatter();

            return formatter.Deserialize(stream) as T;

        }

        public static T DeserializeFromBin<T>(string fileName) where T : class
        {
            var formatter = new BinaryFormatter();
            using (var reader = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return DeserializeFromBin<T>(reader);
            }

        }

        #endregion

        #region Serialize


        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static bool SerializeToBin(object graph, Stream stream)
        {
            var formatter = new BinaryFormatter();

            formatter.Serialize(stream, graph);
            return true;

        }

        [Activity]
        public static bool SerializeToBin(object graph, string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var fs = File.OpenWrite(file))
            {
                return SerializeToBin(graph, fs);
            }
        }

        #endregion
    }
}
