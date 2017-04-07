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
    [Name("Owner Commands")]
    public class OwnerModule : ModuleBase
    {
        //fields
        public Data dat;
        public ModuleInfo _mi;
        public DiscordSocketClient _cl;
        //constructor
        public OwnerModule(CommandService serv, DiscordSocketClient client)
        {
            ModuleInfo mi = serv.Modules.First();
            DiscordSocketClient cl = client;
            _mi = mi;
            _cl = cl;
        }
        //commands
        [Command("massrole", RunMode = RunMode.Async), Summary("Adds a role to every member of the server."), RequireUserPermission(GuildPermission.Administrator), RequireOwner]
            public async Task Massrole([Remainder] string rolename)
        {
            Dictionary<string, IRole> roledict = Context.Guild.Roles.ToDictionary(x => { return x.Name; }, y => { return y; });
            if (roledict.TryGetValue(rolename, out IRole myrole) == false)
            {
                await ReplyAsync("Role not found!");
                return;
            }
            var _users = await Context.Guild.GetUsersAsync();
            var users = _users.ToArray();
            for (int i = 0; i < 5000; i++)
            {
                var usr = users[i];
                Console.Write($".");
                await Task.Delay(3000).ContinueWith(x => {
                    Setrole(usr, myrole);
                });

            }
            Task Setrole(IGuildUser _usr, IRole _role)
            {
                _usr.AddRoleAsync(_role);
                return Task.CompletedTask;
            }
        }

        [Command("getfile"), Summary("Gets a retrieves a file from bot's host"), RequireOwner]
            public async Task GetFile([Remainder] string filename)
        {
            Stream x = new FileStream(filename, FileMode.Open);
            await Context.Channel.SendFileAsync(x, filename, null, false, null);
        }

        [Command("dir"), Summary("returns files in directory from host"), RequireOwner]
            public async Task GetDir([Remainder] string filename)
        {
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

        [Command("say"), Alias("echo"), RequireOwner, Summary("Echos the provided input.")]
            public async Task Say([Remainder] string input)
        {
            await ReplyAsync(input);
        }

    }
}
