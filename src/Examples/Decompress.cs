using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace TorchSharp.Examples.Utils
{
    public static class Decompress
    {
        public static void DecompressGZipFile(string gzipFileName, string targetDir)
        {
            byte[] buf = new byte[4096];

            using (var fs = File.OpenRead(gzipFileName))
            using (var gzipStream = new GZipInputStream(fs)) {

                string fnOut = Path.Combine(targetDir, Path.GetFileNameWithoutExtension(gzipFileName));

                using (var fsOut = File.Create(fnOut)) {
                    StreamUtils.Copy(gzipStream, fsOut, buf);
                }
            }
        }
        public static void ExtractTGZ(string gzArchiveName, string destFolder)
        {
            var flag = gzArchiveName.Split(Path.DirectorySeparatorChar).Last().Split('.').First() + ".bin";
            if (File.Exists(Path.Combine(destFolder, flag))) return;

            Console.WriteLine($"Extracting.");
            var task = Task.Run(() => {
                using (var inStream = File.OpenRead(gzArchiveName)) {
                    using (var gzipStream = new GZipInputStream(inStream)) {
#pragma warning disable CS0618 // Type or member is obsolete
                        using (TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream))
#pragma warning restore CS0618 // Type or member is obsolete
                            tarArchive.ExtractContents(destFolder);
                    }
                }
            });

            while (!task.IsCompleted) {
                Thread.Sleep(200);
                Console.Write(".");
            }

            File.Create(Path.Combine(destFolder, flag));
            Console.WriteLine("");
            Console.WriteLine("Extraction completed.");
        }

    }
}
