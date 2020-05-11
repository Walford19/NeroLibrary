using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;


namespace Nero
{
    public class MemoryService
    {
        /// <summary>
        ///  Comprimi os bytes
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] inputData)
        {
            if (inputData == null)
                throw new ArgumentNullException("inputData must be non-null");

            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(inputData, 0, inputData.Length);
            }
            return output.ToArray();
        }

        public static byte[] Decompress(byte[] inputData)
        {
            if (inputData == null)
                throw new ArgumentNullException("inputData must be non-null");

            MemoryStream input = new MemoryStream(inputData);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }
    }
}
