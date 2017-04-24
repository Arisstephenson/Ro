using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.IO;

namespace DiscordExampleBot.Modules.Public
{
    [Name("Fun Commands 🎲")]
    public class FunModule : ModuleBase
    {
        //fields
        public Data dat;
        public ModuleInfo _mi;
        public DiscordSocketClient _cl;
        //constructor
        public FunModule(CommandService serv, DiscordSocketClient client)
        {
            ModuleInfo mi = serv.Modules.First();
            DiscordSocketClient cl = client;
            _mi = mi;
            _cl = cl;
        }
        //commands
        [Command("Emojify", RunMode = RunMode.Async), Summary("Emojify's a message")]
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
        /**
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
    **/
        [Command("Genderate")]
        public async Task Genderate([Remainder] string discard)
        {
            var temp = "";
            Random rand = new Random();
            string[] prefixes = File.ReadAllLines("GenderData/prefix.txt");
            string[] suffixes = File.ReadAllLines("GenderData/suffix.txt");
            string[] symbols = File.ReadAllLines("GenderData/symbol.txt");
            if (Context.Message.MentionedUserIds.Count > 0)
            {
                var id = Context.Message.MentionedUserIds.First();
                Random idrand = new Random((int)id);
                var usr = await Context.Guild.GetUserAsync(id);
                temp = $"{usr.Username} is a {prefixes[idrand.Next(prefixes.Length - 1)]} {suffixes[idrand.Next(suffixes.Length - 1)]} (symbol: {symbols[idrand.Next(symbols.Length - 1)]})";
                await ReplyAsync(temp);
                return;
            }
            temp = $"{prefixes[rand.Next(prefixes.Length - 1)]} {suffixes[rand.Next(suffixes.Length - 1)]} (symbol: {symbols[rand.Next(symbols.Length - 1)]})";
            await ReplyAsync(temp);
            //TODO: create an array of gender prefixes and suffixes: "Trisexual Ape"
        }
        [Command("collect"), Summary("Collects points for your server.")]
        public async Task Collect([Remainder] string discard = "")
        {
            if (File.Exists($"data/points/{Context.Guild.Id}") == false)
            {
                File.CreateText($"data/points/{Context.Guild.Id}");
                File.WriteAllText($"data/points/{Context.Guild.Id}", "0");
            }
            var points = File.ReadAllText($"data/points/{Context.Guild.Id}");
            var pointz = Convert.ToInt32(points);
            pointz++;
            await ReplyAsync($"Your server now has {pointz}⍟ points!");
            File.WriteAllText($"data/points/{Context.Guild.Id}", pointz.ToString());
        }
        [Command("roll", RunMode = RunMode.Async), Summary("Rolls a die.")]
        public async Task Role()
        {
            Random rand = new Random();
            int roll = rand.Next(1,7);
            switch (roll)
            {
                case 1:
                    await ReplyAsync("```\r╭─────────────╮\r│             │\r│             │\r│     ╭─╮     │\r│     ╰─╯     │\r│             │\r│             │\r╰─────────────╯\rYou rolled a one.```");
                    break;
                case 2:
                    await ReplyAsync("```\r╭─────────────╮\r│        ╭─╮  │\r│        ╰─╯  │\r│             │\r│             │\r│ ╭─╮         │\r│ ╰─╯         │\r╰─────────────╯\rYou rolled a two.```");
                    break;
                case 3:
                    await ReplyAsync("```\r╭─────────────╮\r│        ╭─╮  │\r│        ╰─╯  │\r│     ╭─╮     │\r│     ╰─╯     │\r│ ╭─╮         │\r│ ╰─╯         │\r╰─────────────╯\rYou rolled a three.```");
                    break;
                case 4:
                    await ReplyAsync("```\r╭─────────────╮\r│ ╭─╮    ╭─╮  │\r│ ╰─╯    ╰─╯  │\r│             │\r│             │\r│ ╭─╮    ╭─╮  │\r│ ╰─╯    ╰─╯  │\r╰─────────────╯\rYou rolled a four.```");
                    break;
                case 5:
                    await ReplyAsync("```\r╭─────────────╮\r│ ╭─╮    ╭─╮  │\r│ ╰─╯    ╰─╯  │\r│    ╭─╮      │\r│    ╰─╯      │\r│ ╭─╮    ╭─╮  │\r│ ╰─╯    ╰─╯  │\r╰─────────────╯\rYou rolled a five.```");
                    break;
                case 6:
                    await ReplyAsync("```\r╭─────────────╮\r│ ╭─╮    ╭─╮  │\r│ ╰─╯    ╰─╯  │\r│ ╭─╮    ╭─╮  │\r│ ╰─╯    ╰─╯  │\r│ ╭─╮    ╭─╮  │\r│ ╰─╯    ╰─╯  │\r╰─────────────╯\rYou rolled a six.```");
                    break;
                default:
                    await ReplyAsync("Whoops, your die fell off the table!");
                    break;
            }
        }
        [Command("rsi", RunMode = RunMode.Async)]
        public async Task RSI()
        {
            var guilds = _cl.Guilds;
            var gc = guilds.Count;
            var rand = new Random();
            var pick = rand.Next(0, gc);
            var invite = guilds.ToArray()[pick];
            var invs = await invite.GetInvitesAsync();
            while (invs.First() == null)
            {
                pick = rand.Next(0, gc);
                invite = guilds.ToArray()[pick];
                invs = await invite.GetInvitesAsync();
            }
            await ReplyAsync($"**We found an invite to {invite.Name}!**\r{invs.First().Url}");
        }
    }
}
