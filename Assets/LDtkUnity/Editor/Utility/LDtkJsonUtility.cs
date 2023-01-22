using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Utf8Json;
using Utf8Json.Internal;

namespace LDtkUnity.Editor
{
    internal static class LDtkJsonUtility
    {
        public static byte[] GetBytesFromStream(Stream stream)
        {
            byte[] buffer = MemoryPool.GetBuffer();
            int newSize = FillFromStream(stream, ref buffer);
            if (new JsonReader(buffer).GetCurrentJsonToken() == Utf8Json.JsonToken.Number)
            {
                buffer = BinaryUtil.FastCloneWithResize(buffer, newSize);
            }

            return buffer;
        }
        private static class MemoryPool
        {
            [ThreadStatic]
            private static byte[] buffer;

            public static byte[] GetBuffer()
            {
                if (buffer == null)
                    buffer = new byte[65536];
                return buffer;
            }
        }
        private static int FillFromStream(Stream input, ref byte[] buffer)
        {
            int offset = 0;
            int num;
            while ((num = input.Read(buffer, offset, checked (buffer.Length - offset))) > 0)
            {
                checked { offset += num; }
                if (offset == buffer.Length)
                    BinaryUtil.FastResize(ref buffer, checked (offset * 2));
            }
            return offset;
        }
    }
}