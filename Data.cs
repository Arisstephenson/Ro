using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordExampleBot
{
    public class Data
    {
        public void SetGuildData(ulong guildID, DataType data, string whatToWrite, DiscordSocketClient _cl)
        {
            DirectoryInfo dir;
            FileStream fi;
            if (Directory.Exists($"C:\\ArissBot\\Data\\{guildID}"))
            {
                dir = new DirectoryInfo($"C:\\ArissBot\\Data\\{guildID}");
            }
            else
            {
                dir = Directory.CreateDirectory($"C:\\ArissBot\\Data\\{guildID}");
                Console.WriteLine($"Creating Directory {guildID} ({_cl.GetGuild(guildID).Name})...");
            }
            fi = new FileInfo($"{dir.FullName}\\{data}").Open(FileMode.Append);
            Console.WriteLine($"Writing to File {data}...");
            StreamWriter sw = new StreamWriter(fi);
            sw.Write(whatToWrite);
            sw.Flush();
            sw.Dispose();
            return;
        }
        public string RetrieveGuildData(ulong guildID, DataType data, DiscordSocketClient _cl)
        {
            DirectoryInfo dir;
            FileStream fi;
            if (Directory.Exists($"C:\\ArissBot\\Data\\{guildID}"))
            {
                dir = new DirectoryInfo($"C:\\ArissBot\\Data\\{guildID}");
            }
            else
            {
                dir = Directory.CreateDirectory($"C:\\ArissBot\\Data\\{guildID}");
                Console.WriteLine($"Creating Directory {guildID} ({_cl.GetGuild(guildID).Name})...");
            }
            fi = new FileInfo($"{dir.FullName}\\{data}").Open(FileMode.OpenOrCreate);
            Console.WriteLine($"Opening File {data}...");
            StreamReader sr = new StreamReader(fi);
            string tempstring = sr.ReadToEnd();
            sr.Dispose();
            return tempstring;
        }
    }
}
