using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using D3DataParser.DataStruct;
using D3DataParser.Models;
using Newtonsoft.Json;

namespace D3DataParser
{
    class Program
    {
        const int FixedFileHeaderSize = 16;

        static void Main(string[] args)
        {
            Directory.CreateDirectory("Converted");

            var zhCNfiles = Directory.GetFiles("String/zhCN/StringList", "*.stl");
            var enUSfiles = Directory.GetFiles("String/enUS/StringList", "*.stl");

            //NEED TO BE OPTIMIZED
            var zhCN_Lookups = zhCNfiles.ToDictionary(p => Path.GetFileNameWithoutExtension(p), p => ExtractStirngJson(p));
            var enUS_Lookups = enUSfiles.ToDictionary(p => Path.GetFileNameWithoutExtension(p), p => ExtractStirngJson(p));
            foreach (var mItem in enUS_Lookups)
            {
                if (!zhCN_Lookups.TryGetValue(mItem.Key, out var zhCNItem))
                {
                    continue;
                }
                var rstList = new List<LocaleObj>(mItem.Value.Count);
                foreach (var sItem in mItem.Value)
                {
                    if (!zhCNItem.TryGetValue(sItem.Key, out var zhCN_Text))
                    {
                        continue;
                    }
                    var enUS_Text = sItem.Value;
                    var textJson = new LocaleObj { Key = sItem.Key, ENUS = enUS_Text, ZHCN = zhCN_Text };
                    rstList.Add(textJson);
                }
                File.WriteAllText(Path.Combine("Converted", mItem.Key + ".json"), JsonConvert.SerializeObject(rstList, Formatting.Indented));
            }
            Console.WriteLine("DONE!");
        }

        private static Dictionary<string, string> ExtractStirngJson(string stlFile)
        {
            using var fs = File.OpenRead(stlFile);
            using var br = new BinaryReader(fs);
            var header = br.ReadStruct<StlHeader>();

            //number of offsets
            var dataCount = header.TotalDataSize / Marshal.SizeOf(typeof(StlOffset));

            //create array
            var offsets = new StlOffset[dataCount];

            //Diablo Stringlookup
            var lookup = new Dictionary<string, string>(dataCount);

            for (int i = 0; i < dataCount; i++)
            {
                var offset = br.ReadStruct<StlOffset>();
                var currentPos = br.BaseStream.Position;

                br.BaseStream.Seek(offset.KeyOffset + FixedFileHeaderSize, SeekOrigin.Begin);
                var key = Encoding.UTF8.GetString(br.ReadBytes(offset.KeyLength - 1));//remove \0

                br.BaseStream.Seek(offset.StrOffset + FixedFileHeaderSize, SeekOrigin.Begin);
                var str = Encoding.UTF8.GetString(br.ReadBytes(offset.StrLength - 1));//remove \0

                br.BaseStream.Seek(currentPos, SeekOrigin.Begin);

                lookup[key] = str;
            }
            return lookup;
        }
    }
}
