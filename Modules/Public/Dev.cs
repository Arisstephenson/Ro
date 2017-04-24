/**using System;
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
//    }
//}