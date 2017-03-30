using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;

namespace DiscordExampleBot.Modules.Public
{
    [Name("Utility Commands")]
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

        [Command("embed"), Summary("creates embed with title, desc, footer, separate with $")]
        public async Task CreateEmbed(string title = "", string desc = "", string footer = "")
        {
            await ReplyAsync("", false, new EmbedBuilder().WithTitle(title).WithDescription(desc).WithFooter(new EmbedFooterBuilder().WithText(footer)).Build());
        }
    }

}
