using System;
using System.Collections.Generic;
using System.Linq;
using SevenZip.Compression.LZMA;
using System.IO;
using SevenZip;

namespace common
{
    public class LZMA
    {
        public static void Compress(string src, string dst)
        {
            Encoder encoder   = new Encoder();
            FileStream input  = new FileStream(src, FileMode.Open);
            FileStream output = new FileStream(dst, FileMode.Create);
            MemoryStream properties = new MemoryStream();

            encoder.WriteCoderProperties(properties);
            output.Write(BitConverter.GetBytes((int) properties.Length), 0, sizeof(int));
            output.Write(properties.ToArray(), 0, (int) properties.Length);
            output.Write(BitConverter.GetBytes(input.Length), 0, sizeof(long));

            encoder.Code(input, output, input.Length, -1, null);
            output.Close();
            input.Close();
        }

        public static void Decompress(string src, string dst)
        {
            FileStream input  = new FileStream(src, FileMode.Open);
            Decompress(input, dst, new Progress(null));
            input.Close();
        }

        public static void Decompress(Stream input, string dst, ICodeProgress progress)
        {
            Decoder decoder   = new Decoder();
            FileStream output = new FileStream(dst, FileMode.Create);
            byte[] byteLength = new byte[sizeof(int)];
            input.Read(byteLength, 0, sizeof(int));

            int lenProp = BitConverter.ToInt32(byteLength, 0);
            byte[] properties = new byte[lenProp];
            input.Read(properties, 0, lenProp);
            byteLength = new byte[sizeof(long)];
            input.Read(byteLength, 0, sizeof(long));

            long len = BitConverter.ToInt64(byteLength, 0);
            decoder.SetDecoderProperties(properties);
            decoder.Code(input, output, input.Length, len, progress);
            output.Close();
        }

        public static void Decompress(Stream input, string dst, System.Action<bool /* is decompressing */, long /* current size */, long /* total size */> progressCallback)
        {
            //CommonApplication.PerformTaskAsync((o) =>
            //{
            //    long currentSize = 0, totalSize = 0;
            //    try
            //    {
            //        Decompress(input, dst, new Progress((curSize, outSize) => 
            //        {
            //            currentSize = curSize;
            //            totalSize = outSize;
            //            CommonApplication.PerformTaskOnMainThread((_) =>
            //            {
            //                progressCallback(curSize != outSize, curSize, outSize);
            //            });
            //        }));
            //    }
            //    catch (Exception e)
            //    {
            //        Debug.LogError("Decompress error, " + e.Message);
            //        CommonApplication.PerformTaskOnMainThread((___) =>
            //        {
            //            progressCallback(false, currentSize, totalSize);
            //        });
            //    }
            //});
        }

        class Progress : ICodeProgress
        {
            System.Action<long, long> progress;
            long nextInSize = 0;

            public Progress(System.Action<long, long> progress)
            {
                this.progress = progress;
            }

            public void SetProgress(long inSize, long outSize)
            {
                if (progress != null && (inSize >= nextInSize || inSize == outSize))
                {
                    progress(inSize, outSize);
                    nextInSize = inSize + outSize / 100;
                }
            }
        }
    }
}
