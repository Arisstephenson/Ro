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

namespace Ro
{
	public class StaffModule : ModuleBase
	{
		//  public string data;
		public ModuleInfo _mi;
		public DiscordSocketClient _cl;
		public StaffModule(CommandService serv, DiscordSocketClient client)
		{
			ModuleInfo mi = serv.Modules.First();
			DiscordSocketClient cl = client;
			_mi = mi;
			_cl = cl;
		}

		[Command("mute")]
		public async Task Mute()
		{
			var mutedUserId = Context.Message.MentionedUserIds.First();
			var guild = Context.Guild;
			IGuildUser user = await guild.GetUserAsync(mutedUserId);
			var muteRole = guild.GetRole(291958399770427392);

			await user.AddRoleAsync(muteRole);
		}
		[Command("unmute")]
		public async Task Unmute()
		{
			var mutedUserId = Context.Message.MentionedUserIds.First();
			var guild = Context.Guild;
			IGuildUser user = await guild.GetUserAsync(mutedUserId);
			var muteRole = guild.GetRole(291958399770427392);

			await user.RemoveRoleAsync(muteRole);
		}
	}
}
