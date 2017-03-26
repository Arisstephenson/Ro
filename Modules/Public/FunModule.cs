using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DiscordExampleBot.Modules.Public
{
    class FunModule : ModuleBase
    {
        public Data dat;
        public ModuleInfo _mi;
        public DiscordSocketClient _cl;
        public FunModule(CommandService serv, DiscordSocketClient client)
        {
            ModuleInfo mi = serv.Modules.First();
            DiscordSocketClient cl = client;
            _mi = mi;
            _cl = cl;
        }
        [Command("Emojify", RunMode = RunMode.Async)]
        [Summary("Emojify's a message")]
        public async Task Emojify([Remainder] string rem)
        {
            rem = rem.ToLower();
            string estring = string.Empty;
            foreach (char character in rem)
            {
                estring += $"{Em.GetEmoji(character.ToString())} ";
            }
            await ReplyAsync(estring);
        }
        #region Tag Commands
        [Command("tag")]
        [Summary("Returns the chosen tag.")]
        public async Task ReadTag([Remainder] string input)
        {
            Dictionary<string, string> tagdict = new Dictionary<string, string>();
            Console.WriteLine("Retrieving Tag...");
            string vars = dat.RetrieveGuildData(Context.Guild.Id, DataType.Tags, _cl);
            string[] varsplit = vars.Split('↓');
            foreach (string str in varsplit)
            {
                string[] strsplit = str.Split('→');
                tagdict.Add(strsplit.First(), strsplit.Last());
            }
            await ReplyAsync(tagdict[input]);
        }
        [Command("create")]
        [Summary("Creates a tag.")]
        public async Task WriteTag([Remainder] string input)
        {
            Console.WriteLine("Creating Tag...");
            string[] _args = input.Split(' ');
            List<string> inputlist = _args.ToList();
            dat.SetGuildData(Context.Guild.Id, DataType.Tags, inputlist[0] + "→" + $"{ Format.Code(String.Join(" ", inputlist.Skip(1).ToArray()))}" + "↓", _cl);
            await ReplyAsync($"Added tag {inputlist[0]}!");
        }
        [Command("tags")]
        [Summary("Lists the tags for this server")]
        public async Task ListTags()
        {
            Dictionary<string, string> tagdict = new Dictionary<string, string>();
            string vars = dat.RetrieveGuildData(Context.Guild.Id, DataType.Tags, _cl);
            string[] varsplit = vars.Split('↓');
            foreach (string str in varsplit)
            {
                string[] strsplit = str.Split('→');
                tagdict.Add(strsplit.First(), strsplit.Last());
            }
            await ReplyAsync(string.Join(", ", tagdict.Keys));
        }
#endregion
    }
}
