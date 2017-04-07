using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;
namespace DiscordExampleBot.Modules.Public
{
    [Name("Public Commands")]
    public class PublicModule : ModuleBase
    {
        //fields
        public ModuleInfo _mi;
        public DiscordSocketClient _cl;
        public IEnumerable<ModuleInfo> _mis;
        //constructor
        public PublicModule(CommandService serv, DiscordSocketClient client)
        {
            ModuleInfo mi = serv.Modules.First();
            var mis = serv.Modules;
            DiscordSocketClient cl = client;
            _mi = mi;
            _cl = cl;
            _mis = mis;
        }
        //Commands
        [Command("invite"), Summary("Returns the OAuth2 Invite URL of the bot")]
            public async Task Invite()
        {
            Console.WriteLine("Creating Invite...");
            var application = await Context.Client.GetApplicationInfoAsync();
            await ReplyAsync(
                $"A user with `MANAGE_SERVER` can invite me to your server here: <https://discordapp.com/oauth2/authorize?client_id={application.Id}&scope=bot&permissions=8>");
        }

        [Command("hello"), Summary("Says hello!")]
            public async Task Hello()
        {
            Console.WriteLine("Saying Hello...");
            var name = Context.User.Username;
            await ReplyAsync(
                $"Hello {name}!");
        }

        [Command("pinfo", RunMode = RunMode.Async), Summary("Gets info about mentioned player.")]
            public async Task GetInfo([Remainder] string none = "")
        {
            ulong mention = 0;
            var ids = Context.Message.MentionedUserIds;
            var first = ids.FirstOrDefault();
            if (first != 0)
            {
                mention = first;
            }
            else
            {
                mention = Context.User.Id;
            }
            IGuildUser gusr = await Context.Guild.GetUserAsync(mention);
            string roles = "";
            foreach (ulong id in gusr.RoleIds)
            {
                 roles += Context.Guild.GetRole(id).Name + ", ";
            }
            roles = roles.TrimEnd(new char[] {',', ' '});
            await Context.Channel.SendMessageAsync($"", false, new EmbedBuilder()
                .WithColor(UppermostRole(gusr as SocketGuildUser).Color)
                .WithThumbnailUrl($"{gusr.GetAvatarUrl(ImageFormat.Png, 128)}")
                .AddField(new EmbedFieldBuilder().WithIsInline(true).WithName("Username").WithValue($"{gusr.Username}#{gusr.Discriminator}"))
                .AddField(new EmbedFieldBuilder().WithIsInline(true).WithName("Server Join Date").WithValue($"Joined server at {gusr.JoinedAt.GetValueOrDefault().DateTime}."))
                .AddField(new EmbedFieldBuilder().WithIsInline(true).WithName("Discord Join Date").WithValue($"Joined discord at {gusr.CreatedAt.DateTime}."))
                .AddField(new EmbedFieldBuilder().WithIsInline(true).WithName("Roles").WithValue($"{roles}"))

                );

        }
        public static SocketRole UppermostRole(SocketGuildUser user)
        {
            var sorted = user.Roles
            .Where(role => role.IsHoisted)
            .OrderBy(role => role.Position);
            return sorted.LastOrDefault() ?? user.Guild.EveryoneRole;
        }
        [Command("help"), Summary("Shows available commands.")]
            public async Task Help()
        {
            var marray = _mis.ToArray();
            string[] ListCommands = new string[marray.Length];
            for (int i = 0; i < marray.Length; i++)
            {
                var mod = marray[i];
                var tempstring = "";
                tempstring += $"\r {Format.Bold("["+mod.Name+"]")} \r";
                foreach(CommandInfo com in mod.Commands)
                {        
                    tempstring += "• " + com.Aliases.First() + "\r";
                }
                ListCommands[i] = tempstring;
            }

            var application = await Context.Client.GetApplicationInfoAsync();
            var myembed = new EmbedBuilder()
                .WithTitle($"{Format.Bold("__Ro Commands__")}").WithFooter(new EmbedFooterBuilder().WithText($"{application.Name} by {application.Owner.Username}").WithIconUrl(application.Owner.GetAvatarUrl()));
            for(int i = 0; i < ListCommands.Length; i++)
            {
                string mystring = ListCommands[i];
                string[] mystrings = mystring.Split(']');
                myembed.AddField(new EmbedFieldBuilder().WithValue(mystrings[1].TrimStart('*') + "").WithName("\n" + mystrings[0] + "]**").WithIsInline(true));
                //myembed.AddField(new EmbedFieldBuilder().WithValue("⠀").WithName("⠀").WithIsInline(true));
                //myembed.AddField(new EmbedFieldBuilder().WithValue("⠀").WithName("⠀").WithIsInline(true)); //for spacing to compensate for icon
                // (i == ListCommands.Length - 1) { continue; }
                //myembed.AddField(new EmbedFieldBuilder().WithValue("⠀").WithName("⠀").WithIsInline(true));
            }
            await ReplyAsync("", false, myembed.WithThumbnailUrl(application.IconUrl).WithColor(new Color(255, 0, 0)));
        }

		/**
		 * This command litterally craps itself for afro
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
        */

        [Command("support"), Summary("Returns a link to Ro's support server")]
            public async Task GetServer()
        {
            await ReplyAsync($"Do you need help with Ro? Visit the support server: https://discord.gg/4QcF7fx");
        }

        [Command("info"), Summary("Detailed info about the bot.")]
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


        //Methods
        private static string GetUptime()
            => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");

        private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();
    }
}
