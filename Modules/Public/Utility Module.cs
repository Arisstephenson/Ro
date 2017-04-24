using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Net.Http;
using Discord.Rest;
using System.IO;
using System.Collections.Generic;
namespace DiscordExampleBot.Modules.Public
{
    [Name("Utility Commands ⚙")]
    public class UtilityModule : ModuleBase
    {
        //fields
        public Data dat;
        public ModuleInfo _mi;
        public DiscordSocketClient _cl;
        //constructor
        public UtilityModule(CommandService serv, DiscordSocketClient client)
        {
            ModuleInfo mi = serv.Modules.First();
            DiscordSocketClient cl = client;
            _mi = mi;
            _cl = cl;
        }

        [Command("ballot", RunMode = RunMode.Async), RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task Ballot()
        {
            await Context.Message.DeleteAsync();
            var a = await Context.Channel.GetMessagesAsync(100).Flatten();
            foreach (IMessage x in a)
            {
                IUserMessage msg = (IUserMessage)x;
                await Task.Delay(3000).ContinueWith(async y =>
                {
                    await msg.AddReactionAsync("👍");
                    await msg.AddReactionAsync("👎");
                });
            }
        }

        [Command("embed"), Summary("creates an embed")]
        public async Task CreateEmbed(string title = "", string desc = "", string footer = "")
        {
            await ReplyAsync("", false, new EmbedBuilder().WithTitle(title).WithDescription(desc).WithFooter(new EmbedFooterBuilder().WithText(footer)).Build());
        }
        
        [Command("nick"), Summary("sets the nickname of a user"), RequireUserPermission(GuildPermission.ManageNicknames)]
        public async Task Nick(string discard, [Remainder]string nick)
        {
            var mention = Context.Message.MentionedUserIds.First();
            var user = await Context.Guild.GetUserAsync(mention);
            await user.ModifyAsync(x => {
                x.Nickname = nick;
            });
        }

        [Command("Emotes"), Summary("Lists the server's emotes.")]
        public async Task Emotes()
        {
            var emojis = Context.Guild.Emojis;
            var tempstring = "**Emojis:** ";
            foreach (GuildEmoji ge in emojis)
            {
                tempstring += $"\r • `{ge.Name}` - <:{ge.Name}:{ge.Id}>";
            }
            await ReplyAsync($"{tempstring}");
        }
        /**[Command("lobby", RunMode = RunMode.Async), Summary("Created a temporary lobby")]
        public async Task Lobby()
        {
            var user = Context.User as IGuildUser;
            var socketuser = Context.User as SocketGuildUser;
            if (user.VoiceChannel == null)
            {
                await ReplyAsync("You must be in a voice channel to use this command!");
                return;
            }
            var channel = await Context.Guild.CreateVoiceChannelAsync($"{Context.User.Username}'s Channel");
            await user.ModifyAsync(x => {
                x.Channel = new Optional<IVoiceChannel>(channel);
            });
            await Task.Delay(2000).ContinueWith(x =>
            {
                Context.Discord.UserVoiceStateUpdated += Discord_UserVoiceStateUpdated;

            });
            async Task Discord_UserVoiceStateUpdated(SocketUser arg1, SocketVoiceState arg2, SocketVoiceState arg3)
            {
                if (arg3.VoiceChannel.Id != channel.Id)
                {
                    await channel.DeleteAsync();
                    socketuser.Discord.UserVoiceStateUpdated -= Discord_UserVoiceStateUpdated;
                }
            }
        }**/

        /**
         * Was messing around with httpclient
        [Command("karnageboards")]
        public async Task Search()
        {
            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync("http://karnage.io"))
            using (HttpContent content = response.Content)
            {
                // ... Read the string.
                string result = await content.ReadAsStringAsync();

                // ... Display the result.
                if (result != null)
                {
                    var body = result.Substring(result.IndexOf("leaderTableContainer"), 500);
                    await ReplyAsync($"```html\r{body}```");
                }
            }
        }
    **/
    /**
    [Command("track", RunMode = RunMode.Async), RequireOwner]
    public async Task Track()
        {
            await ReplyAsync("Now tracking joins...");
            //var file = File.ReadAllLines("data\\UserLog.txt");
            var userlist = "";
            Repeat();
            async void Repeat()
            {
                await Task.Delay(1000 * 60).ContinueWith(async x =>
                {
                    var users = await Context.Guild.GetUsersAsync();
                    userlist += $"{ users.Count.ToString()}, ";
                    File.WriteAllText("UserLog.txt", userlist);
                });
                Repeat();
            }
        }
    **/
    }

}
