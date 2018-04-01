using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StreamsDemo
{
    // C# 6.0 in a Nutshell. Joseph Albahari, Ben Albahari. O'Reilly Media. 2015
    // Chapter 15: Streams and I/O
    // Chapter 6: Framework Fundamentals - Text Encodings and Unicode
    // https://msdn.microsoft.com/ru-ru/library/system.text.encoding(v=vs.110).aspx

    /// <summary>
    /// Different ways to work with file streams
    /// </summary>
    public static class StreamsExtension
    {
        #region Public members

        #region TODO: Implement by byte copy logic using class FileStream as a backing store stream .

        /// <summary>
        /// Do the byte copy of the file.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <returns>number of written bytes</returns>
        public static int ByByteCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            using (var fs = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            using (var fd = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
            {
                for (int i = 0; i < fs.Length; i++) 
                {
                    fd.WriteByte((byte)fs.ReadByte());
                }

                return (int)fd.Length;
            }
        }

        #endregion

        #region TODO: Implement by byte copy logic using class MemoryStream as a backing store stream.

        /// <summary>
        /// Do the byte copy of the file using memory stream.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <returns>number of written bytes</returns>
        public static int InMemoryByByteCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            byte[] data;

            using (TextReader reader = new StreamReader(sourcePath)) 
            {
                byte[] source = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                var ms = new MemoryStream(source);
                ms.Close();
                data = ms.ToArray();
            }

            using (TextWriter writer = new StreamWriter(destinationPath)) 
            {
                char[] source = Encoding.UTF8.GetChars(data);
                writer.Write(source);
                return source.Length;
            }

            // TODO: step 1. Use StreamReader to read entire file in string

            // TODO: step 2. Create byte array on base string content - use  System.Text.Encoding class

            // TODO: step 3. Use MemoryStream instance to read from byte array (from step 2)

            // TODO: step 4. Use MemoryStream instance (from step 3) to write it content in new byte array

            // TODO: step 5. Use Encoding class instance (from step 2) to create char array on byte array content

            // TODO: step 6. Use StreamWriter here to write char array content in new file
        }
        #endregion

        #region TODO: Implement by block copy logic using FileStream buffer.

        /// <summary>
        /// Do the block copy of the file.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <returns>number of written bytes</returns>
        public static int ByBlockCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            byte[] data;

            using (var fs = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {
                data = new byte[(int)fs.Length];
                fs.Read(data, 0, data.Length);
            }

            using (var fs = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(data, 0, data.Length);
                return (int)fs.Length;
            }
        }

        #endregion

        #region TODO: Implement by block copy logic using MemoryStream.

        /// <summary>
        /// Do the block copy of the file using memory stream.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <returns>number of written bytes</returns>
        public static int InMemoryByBlockCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            byte[] data;

            using (TextReader reader = new StreamReader(sourcePath))
            {
                byte[] source = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                var ms = new MemoryStream(source);
                ms.Close();
                data = ms.ToArray();
            }

            using (TextWriter writer = new StreamWriter(destinationPath))
            {
                char[] source = Encoding.UTF8.GetChars(data);
                writer.Write(source);
                return source.Length;
            }
        }

        #endregion

        #region TODO: Implement by block copy logic using class-decorator BufferedStream.

        /// <summary>
        /// Do the block copy logic using class-decorator buffered stream.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <returns>number of written bytes</returns>
        public static int BufferedCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            byte[] data = File.ReadAllBytes(sourcePath);
            using (var fd = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
            using (var bd = new BufferedStream(fd, data.Length)) 
            {
                bd.Write(data, 0, data.Length);
                return (int)fd.Length;
            }
        }

        #endregion

        #region TODO: Implement by line copy logic using FileStream and classes text-adapters StreamReader/StreamWriter

        /// <summary>
        /// Do the line copy.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <returns>number of written lines</returns>
        public static int ByLineCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            List<string> list = new List<string>();
            using (var fs = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            using (TextReader reader = new StreamReader(fs)) 
            {
                do
                {
                    list.Add(reader.ReadLine());
                }
                while (list[list.Count - 1] != null);
            }

            using (var fd = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
            using (TextWriter writer = new StreamWriter(fd))
            {
                int count = 0;
                foreach (string i in list) 
                {
                    writer.WriteLine(i);
                    count++;
                }

                return count;
            }
        }

        #endregion

        #region TODO: Implement content comparison logic of two files 

        /// <summary>
        /// Determines whether [is content equals] [the specified source path].
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <returns>
        ///   <c>true</c> if [is content equals] [the specified source path]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsContentEquals(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            using (var fsource = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            using (var fdest = new FileStream(destinationPath, FileMode.Open, FileAccess.Read))
            {
                if (fsource.Length != fdest.Length)
                {
                    return false;
                }
               
                for (int i = 0; i < fsource.Length; i++)
                {
                    if (fsource.ReadByte() != fdest.ReadByte())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion

        #endregion

        #region Private members

        #region TODO: Implement validation logic

        /// <summary>
        /// Input validation check.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <exception cref="ArgumentNullException">
        /// source path is null
        /// or
        /// destination path is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// source path does not exists
        /// or
        /// destination path does not exists
        /// </exception>
        private static void InputValidation(string sourcePath, string destinationPath)
        {
            if (sourcePath == null)
            {
                throw new ArgumentNullException(nameof(sourcePath));
            }

            if (destinationPath == null)
            {
                throw new ArgumentNullException(nameof(destinationPath));
            }

            if (!File.Exists(sourcePath)) 
            {
                throw new ArgumentException(nameof(sourcePath));
            }

            if (!File.Exists(destinationPath))
            {
                throw new ArgumentException(nameof(destinationPath));
            }
        }

        #endregion

        #endregion
    }
}
