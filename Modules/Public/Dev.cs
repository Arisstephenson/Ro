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
    [Name("Developing Commands")]
    public class Dev : ModuleBase
    {
        int test = "test".IndexOf("a");
        //fields
        public ModuleInfo _mi;
        public DiscordSocketClient _cl;
        public IEnumerable<ModuleInfo> _mis;
        //constructor
        public Dev(CommandService serv, DiscordSocketClient client)
        {
            ModuleInfo mi = serv.Modules.First();
            var mis = serv.Modules;
            DiscordSocketClient cl = client;
            _mi = mi;
            _cl = cl;
            _mis = mis;
        }

        [Command("ban"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Ban(string mention, [Remainder] string reason = "No reason specified.")
        {
            var user = await Context.Guild.GetUserAsync(Context.Message.MentionedUserIds.First());
            var dm = await user.CreateDMChannelAsync();
            await dm.SendMessageAsync($"You were banned from {Context.Guild.Name} for the following reason:\r{Format.Bold(reason)}");
            await Context.Guild.AddBanAsync(user);
        }
        [Command("kick"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Kick(string mention, [Remainder] string reason = "No reason specified.")
        {
            var user = await Context.Guild.GetUserAsync(Context.Message.MentionedUserIds.First());
            var dm = await user.CreateDMChannelAsync();
            await dm.SendMessageAsync($"You were kicked from {Context.Guild.Name} for the following reason:\r{Format.Bold(reason)}");
            await user.KickAsync();
        }
        [Command("info")]
        public async Task Owner([Remainder] string inf)
        {
            if (inf == "server")
            {
                var guild = Context.Guild;
                var owner = await guild.GetUserAsync(guild.OwnerId);
                var members = await guild.GetUsersAsync();
                var voicechannels = await guild.GetVoiceChannelsAsync();
                var textchannels = await guild.GetTextChannelsAsync();
                var roles = guild.Roles;
                var orderedroles = roles.OrderBy(x => x.Id);
                var rolementions = orderedroles.Select(x =>
                {
                    return x.Mention;
                });
                var textmentions = textchannels.Select(x =>
                {
                    return x.Mention;
                });
                var defaultchannel = await guild.GetTextChannelAsync(guild.DefaultChannelId);
                var embed = new EmbedBuilder()
                    .WithTitle(Format.Underline(Format.Code("Server Info")))
                    .WithColor(new Color(0.9f, 0.9f, 0.1f))
                    .WithThumbnailUrl(guild.IconUrl == null ? Context.Message.Author.GetAvatarUrl() : guild.IconUrl)
                    .WithCurrentTimestamp()
                    .WithUrl($"https://discordapp.com/channels/{guild.Id}/{guild.DefaultChannelId}")
                    .AddInlineField("Name", guild.Name)
                    .AddInlineField("Owner", owner.Mention)
                    .AddInlineField("Verification Level", guild.VerificationLevel)
                    .AddInlineField("Member Count", members.Count())
                    .AddInlineField("Creation Date", guild.CreatedAt)
                    .AddInlineField("Voice Region", await Context.Client.GetVoiceRegionAsync(guild.VoiceRegionId))
                    .AddInlineField("Voice Channels", String.Join(", ", voicechannels))
                    .AddInlineField("Text Channels", String.Join(", ", textmentions))
                    .AddInlineField("Roles", string.Join(", ", rolementions));
                var message = await ReplyAsync("", false, embed);
                await message.AddReactionAsync("➡");
                
            }
            else
            {
                await ReplyAsync("Info not found! Use >info help to see usage.");
            }
        }
        /**
        public async Task OnReac(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            {
                if (arg3.UserId == _cl.CurrentUser.Id) return;
                var msg = await arg1.GetOrDownloadAsync();
                await msg.RemoveAllReactionsAsync();
                await msg.ModifyAsync(k =>
                {
                    k.Embed = new EmbedBuilder().WithTitle("test").Build();
                });
                await msg.AddReactionAsync("⬅");
                _cl.ReactionAdded += OnReac2;
                _cl.ReactionAdded -= OnReac;
            }
        }
        public async Task OnReac2(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            if (arg3.UserId == _cl.CurrentUser.Id) return;
            var msg = await arg1.GetOrDownloadAsync();
            await msg.RemoveAllReactionsAsync();
            await msg.ModifyAsync(k =>
            {
                k.Embed = new EmbedBuilder().WithTitle("back").Build();
            });
            await msg.AddReactionAsync("➡");
            _cl.ReactionAdded += OnReac;
            _cl.ReactionAdded -= OnReac2;

        }
        **/
    }
}