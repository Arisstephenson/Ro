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
using Imgur;
using Imgur.API.Authentication;
namespace DiscordExampleBot.Modules.Public
{
    public class PublicModule : ModuleBase
    {
        //  public string data;
        public ModuleInfo _mi;
        public DiscordSocketClient _cl;
        public PublicModule(CommandService serv, DiscordSocketClient client)
        {
            ModuleInfo mi = serv.Modules.First();
            DiscordSocketClient cl = client;
            _mi = mi;
            _cl = cl;
        }

        private string RetrieveGuildData(ulong guildID, DataType data)
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
        private void SetGuildData(ulong guildID, DataType data, string whatToWrite)
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
            sw.Write( /** RetrieveGuildData(guildID, data) + **/ whatToWrite);
            sw.Flush();
            sw.Dispose();
            return;
        }
        static Dictionary<string, string> tags = new Dictionary<string, string>();
        [Command("invite")]
        [Summary("Returns the OAuth2 Invite URL of the bot")]
        public async Task Invite()
        {
            Console.WriteLine("Creating Invite...");
            var application = await Context.Client.GetApplicationInfoAsync();
            await ReplyAsync(
                $"A user with `MANAGE_SERVER` can invite me to your server here: <https://discordapp.com/oauth2/authorize?client_id={application.Id}&scope=bot>");
        }
        [Command("hello")]
        [Summary("Says hello!")]
        public async Task Hello()
        {
            Console.WriteLine("Saying Hello...");
            var name = Context.User.Username;
            await ReplyAsync(
                $"Hello {name}!");
        }
        [Command("pinfo")]
        [Summary("Gets info about mentioned player.")]
        public async Task GetInfo([Remainder] string mention = "0")
        {
            if (mention == "0")
            {
                mention = MentionUtils.MentionUser(Context.User.Id);
            }
            IGuildUser gusr = await Context.Guild.GetUserAsync(MentionUtils.ParseUser(mention));
            string userdata =
                $"{Format.Bold($"@{gusr.Username + "#" + gusr.Discriminator}")}\r" +
                $"Joined server at {gusr.JoinedAt}.\r" +
                $"Joined discord at {gusr.CreatedAt}.\r" +
                $"Roles: ";
            foreach (ulong id in gusr.RoleIds) {
                userdata += Context.Guild.GetRole(id).Name + ", ";
            }
            await Context.Channel.SendMessageAsync($"", false, new EmbedBuilder()
                .WithColor(Context.Guild.GetRole(gusr.RoleIds.Last()).Color)
                .WithDescription(userdata).WithTitle($"{gusr.Nickname}")
                .WithImageUrl($"{gusr.GetAvatarUrl(ImageFormat.Png, 128)}")
                .Build());
        }
        [Command("clear")]
        [Summary("Clears messages.")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task ClearMessages([Remainder] int b)
        {
            await Context.Channel.DeleteMessagesAsync(await Context.Channel.GetMessagesAsync(b + 1).Flatten());
            await ReplyAsync($"{Context.Message.Author.Mention}, successfully cleared {b} messages!");
        }
        [Command("tag")]
        [Summary("Returns the chosen tag.")]
        public async Task ReadTag([Remainder] string input)
        {
            Dictionary<string, string> tagdict = new Dictionary<string, string>();
            Console.WriteLine("Retrieving Tag...");
            /** if (tags.TryGetValue(input, out string tagreturn) == true)
            {
                await ReplyAsync($"{tagreturn}");
            }
            else
            {
                await ReplyAsync($"Tag not found!");
            }**/
            string vars = RetrieveGuildData(Context.Guild.Id, DataType.Tags);
            string[] varsplit = vars.Split('↓');
            foreach (string str in varsplit)
            {
                string[] strsplit = str.Split('→');
                tagdict.Add(strsplit.First(), strsplit.Last());
            }
            await ReplyAsync(tagdict[input]);
        }
        [Command("help")]
        [Summary("Shows available commands.")]
        public async Task Help()
        {
            var __mi = _mi.Commands.OrderBy(x => { return x.Aliases.First().ToCharArray().First(); }).ToArray();
            string milist = string.Empty;
            __mi.All(x => { milist += ($"{x.Aliases.First()}, {x.Summary} \n"); return true; });
            Console.WriteLine("Listing Commands...");
            EmbedBuilder emb = new EmbedBuilder();
            emb.WithTitle(Format.Bold("Commands"))
                .WithDescription($"{milist}")
                .WithColor(new Color(200, 200, 200));
            await ReplyAsync($"", false, emb.Build());
        }
        [Command("tags")]
        [Summary("Lists the tags for this server")]
        public async Task ListTags()
        {
            Dictionary<string, string> tagdict = new Dictionary<string, string>();
            string vars = RetrieveGuildData(Context.Guild.Id, DataType.Tags);
            string[] varsplit = vars.Split('↓');
            foreach (string str in varsplit)
            {
                string[] strsplit = str.Split('→');
                tagdict.Add(strsplit.First(), strsplit.Last());
            }
            await ReplyAsync(string.Join(", ", tagdict.Keys));
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
        [Command("GetFile")]
        [Summary("Gets a retrieves a file from bot's host")]
        [RequireOwner]
        public async Task GetFile([Remainder] string filename)
        {
            Console.WriteLine("Getting File...");
            await Context.Channel.SendFileAsync(filename);
        }
        [Command("dir")]
        [Summary("returns files in directory from host")]
        [RequireOwner]
        public async Task GetDir([Remainder] string filename)
        {
            Console.WriteLine("Getting File...");
            string dirs = "";
            foreach (FileSystemInfo fs in new DirectoryInfo(filename).GetFileSystemInfos())
            {
                if (fs.Extension == string.Empty)
                {
                    dirs += $"<dir> {fs.Name},\r";
                }
                else
                {
                    dirs += $"{fs.Name},\r";
                }
            }
            await ReplyAsync($"{Format.Code(dirs, "bat")}");
        }
        [Command("create")]
        [Summary("Creates a tag.")]
        public async Task WriteTag([Remainder] string input)
        {
            Console.WriteLine("Creating Tag...");
            string[] _args = input.Split(' ');
            List<string> inputlist = _args.ToList();
            SetGuildData(Context.Guild.Id, DataType.Tags, inputlist[0] + "→" + $"{ Format.Code(String.Join(" ", inputlist.Skip(1).ToArray()))}" + "↓");
            await ReplyAsync($"Added tag {inputlist[0]}!");
        }
        [Command("set")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Set([Remainder]string remaining)
        {
            string targetvar = remaining.Split(' ').First();
            string value = remaining.Split(' ').Last();
            if (targetvar == "linkFilter" && value == "1")
            {
                Console.WriteLine("Assigning to delegate...");
                _cl.MessageReceived += (async (x) => {
                    bool hasLink = DetectLinks(x);
                    if (hasLink)
                    {
                        await x.DeleteAsync();
                        IMessage im = await x.Channel.SendMessageAsync($"{Format.Bold("Only one link per message 😡")}");
                        await DeleteMsg(im, 1500);
                        return;
                    }
                    else return;
                });
                bool DetectLinks(SocketMessage sm)
                {
                    Regex reg = new Regex("http", RegexOptions.IgnoreCase);
                    MatchCollection matches = reg.Matches(sm.Content);
                    if (matches.Count > 1)
                    {
                        return true;
                    }
                    else return false;
                }
                async Task DeleteMsg(IMessage msg, int delayInMilliseconds)
                {
                    await Task.Delay(delayInMilliseconds).ContinueWith(async (x) =>
                    {
                        await msg.DeleteAsync();
                    });
                }
                await ReplyAsync($"Updated tag {targetvar}!");
            }
        }
        [Command("say")]
        [Alias("echo")]
        [RequireOwner]
        [Summary("Echos the provided input.")]
        public async Task Say([Remainder] string input)
        {
            Console.WriteLine("Mocking...");
            await ReplyAsync(input);
        }
        [Command("Support")]
        [Summary("Returns a link to Ro's support server")]
        public async Task GetServer()
        {
            await ReplyAsync($"Do you need help with Ro? Visit the support server: https://discord.gg/4QcF7fx");
        }
        [Command("info")]
        [Summary("Detailed info about the bot.")]
        public async Task Info()
        {
            Console.WriteLine("Providing Information...");
            var application = await Context.Client.GetApplicationInfoAsync();
            await ReplyAsync(
                $"{Format.Bold("Info")}\n" +
                $"- Author: {application.Owner.Username} (ID {application.Owner.Id})\n" +
                $"- Library: Discord.Net ({DiscordConfig.Version})\n" +
                $"- Runtime: {RuntimeInformation.FrameworkDescription} {RuntimeInformation.OSArchitecture}\n" +
                $"- Uptime: {GetUptime()}\n" +
                $"- Ping: {(Context.Client as DiscordSocketClient).Latency}ms\n\n" +
                $"{Format.Bold("Stats")}\n" +
                $"- Heap Size: {GetHeapSize()} MB\n" +
                $"- Guilds: {(Context.Client as DiscordSocketClient).Guilds.Count}\n" +
                $"- Channels: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Channels.Count)}" +
                $"- Users: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Users.Count)}"
            );
        }
        [Command("embed")]
        [Summary("creates embed with title, desc, footer, separate with $")]
        public async Task CreateEmbed([Remainder] string rem) {
            string[] splitrem = rem.Split('$');
            await ReplyAsync("", false, new EmbedBuilder().WithTitle(splitrem[0]).WithDescription(splitrem[splitrem.Length-2]).WithFooter(new EmbedFooterBuilder().WithText(splitrem.Last())).Build());
        }
        private static string GetUptime()
            => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
        private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();
    }
}
