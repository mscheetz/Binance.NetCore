using Binance.NetCore.Data.Interface;
using Newtonsoft.Json;
using System.IO;

namespace Binance.NetCore.Data
{
    public class FileRepository : IFileRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FileRepository()
        {
        }

        /// <summary>
        /// Check if config file exists
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Boolean of validation</returns>
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Get data from a file
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="path">Path to file</param>
        /// <returns>New object from file data</returns>
        public T GetDataFromFile<T>(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();

                var config = JsonConvert.DeserializeObject<T>(json);

                json = null;

                return config;
            }
        }

        /// <summary>
        /// Update file
        /// </summary>
        /// <typeparam name="T">Data type of data to be written</typeparam>
        /// <param name="data">Data to be written</param>
        /// <param name="path">Path to file</param>
        /// <returns>Boolean when complete</returns>
        public bool UpdateFile<T>(T data, string path)
        {
            var json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);

            json = null;
            
            return true;
        }
    }
}
