namespace Binance.NetCore.Data.Interface
{
    public interface IFileRepository
    {
        /// <summary>
        /// Check if config file exists
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Boolean of validation</returns>
        bool FileExists(string path);

        /// <summary>
        /// Get data from a file
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="path">Path to file</param>
        /// <returns>New object from file data</returns>
        T GetDataFromFile<T>(string path);

        /// <summary>
        /// Update file
        /// </summary>
        /// <typeparam name="T">Data type of data to be written</typeparam>
        /// <param name="data">Data to be written</param>
        /// <param name="path">Path to file</param>
        /// <returns>Boolean when complete</returns>
        bool UpdateFile<T>(T data, string path);
    }
}
