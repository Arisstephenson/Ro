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
    [Name("Moomoo Commands")]
    public class Moodule : ModuleBase
    {
        int test = "test".IndexOf("a");
        //fields
        public ModuleInfo _mi;
        public DiscordSocketClient _cl;
        public IEnumerable<ModuleInfo> _mis;
        //constructor
        public Moodule(CommandService serv, DiscordSocketClient client)
        {
            ModuleInfo mi = serv.Modules.First();
            var mis = serv.Modules;
            DiscordSocketClient cl = client;
            _mi = mi;
            _cl = cl;
            _mis = mis;
        }

        [Command("rule")]
        public async Task GetRule([Remainder] string a)
        {
            string[] rules = new string[] {
                "1. Harassment & Disrespect - Do not harass or disrespect another user.",
                "2. Impersonation - Do not pretend to be somebody else. This includes staff members, bots and other users.",
                "3. Message Content - Do not post any type of offensive content. This includes scary, sexual, and any other type of graphic content. Keep the chat clean and PG.",
                "4. Spam - Don't post meaningless messages. We also consider sending messages in all caps, repeating your messages, and posting random messages to gain exp to be spam.",
                "5. Bots - Don't misuse or abuse the bots and their commands.",
                "6. Evasion - Do not bypass, or attempt to bypass any type of punishment. This includes using an alternate account to get around a ban or mute.",
                "7. Topics - Keep the channels on topic. Post suggestions in #suggestions, bot commands in #bot-commands, etc.",
                "8. Lying - Do not lie to a staff member.",
                "9. English - Talk English in all chats, expect the ones that are meant for other languages like: #russian and #portuguese.",
                "10. Advertising - Do not advertise any other Discord server or website. You may post things as long as you are not doing just as an advertisement (videos, images, and music are allowed).",
                "11. Language - Do not swear, attempt to swear, or bypass a language filter.",
                "12. Do not repeatedly ask for a staff position. Staff positions are given to users who are trusted by the administrators.",
                "13. Disobeying Staff - Do not argue with staff members. If you have a complaint about a staff member, privately contact an administrator.",
                "14. Name Content - Do not name yourself anything inappropriate or not able to be typed with a standard US keyboard (the staff need to be able to mention you.)",
                "15. Disruption - Do not do anything to cause disruptions in the chat"
            };
            foreach (string rule in rules)
            {
                var c = rule.IndexOf(a, 3);
                if (c > 0)
                {
                    await ReplyAsync($"```{rule}```");
                    return;
                }
            }
            var b = Convert.ToUInt32("a");
            switch ( b )
            {
                case 1:
                    await ReplyAsync("```1. Harassment & Disrespect - Do not harass or disrespect another user.```");
                    break;

                case 2:
                    await ReplyAsync("```2. Impersonation - Do not pretend to be somebody else. This includes staff members, bots and other users.```");
                    break;

                case 3:
                    await ReplyAsync("```3. Message Content - Do not post any type of offensive content. This includes scary, sexual, and any other type of graphic content. Keep the chat clean and PG.```");
                    break;

                case 4:
                    await ReplyAsync("```4. Spam - Don't post meaningless messages. We also consider sending messages in all caps, repeating your messages, and posting random messages to gain exp to be spam. ```");
                    break;

                case 5:
                    await ReplyAsync("```5. Bots - Don't misuse or abuse the bots and their commands.```");
                    break;

                case 6:
                    await ReplyAsync("```6. Evasion - Do not bypass, or attempt to bypass any type of punishment. This includes using an alternate account to get around a ban or mute.```");
                    break;

                case 7:
                    await ReplyAsync("```7. Topics - Keep the channels on topic. Post suggestions in #suggestions, bot commands in #bot-commands, etc.```");
                    break;

                case 8:
                    await ReplyAsync("```8. Lying - Do not lie to a staff member.```");
                    break;

                case 9:
                    await ReplyAsync("```9. English - Talk English in all chats, expect the ones that are meant for other languages like: #russian and #portuguese.```");
                    break;

                case 10:
                    await ReplyAsync("```10. Advertising - Do not advertise any other Discord server or website. You may post things as long as you are not doing just as an advertisement (videos, images, and music are allowed).```");
                    break;

                case 11:
                    await ReplyAsync("```11. Language - Do not swear, attempt to swear, or bypass a language filter.```");
                    break;

                case 12:
                    await ReplyAsync("```12. Do not repeatedly ask for a staff position. Staff positions are given to users who are trusted by the administrators.```");
                    break;

                case 13:
                    await ReplyAsync("```13. Disobeying Staff - Do not argue with staff members. If you have a complaint about a staff member, privately contact an administrator.```");
                    break;

                case 14:
                    await ReplyAsync("```14. Name Content - Do not name yourself anything inappropriate or not able to be typed with a standard US keyboard (the staff need to be able to mention you.)```");
                    break;

                case 15:
                    await ReplyAsync("```15. Disruption - Do not do anything to cause disruptions in the chat```");
                    break;
                default:
                    await ReplyAsync("That's not a rule!");
                    break;
            }
        }

    }
}