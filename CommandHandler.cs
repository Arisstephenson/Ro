using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;

namespace DiscordExampleBot
{
    public class CommandHandler
    {
        private CommandService commands;
        private DiscordSocketClient client;
        private IDependencyMap map;

        public async Task Install(IDependencyMap _map)
        {
            // Create Command Service, inject it into Dependency Map
            client = _map.Get<DiscordSocketClient>();
            commands = new CommandService();
            //_map.Add(commands);
            map = _map;
            commands.Log += (x => {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"[{DateTime.Now.GetDateTimeFormats()[110]}]");
                switch ((int)x.Severity)
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case 2:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;
                    case 3:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case 4:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                    case 5:
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                } //sets color according to severity
                Console.WriteLine(x.Message);
                return Task.CompletedTask;
            });
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
            client.MessageReceived += HandleCommand;
        }

        public async Task HandleCommand(SocketMessage parameterMessage)
        {
            // Don't handle the command if it is a system message
            var message = parameterMessage as SocketUserMessage;
            if (message == null) return;

            // Mark where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message has a valid prefix, adjust argPos 
            if (!(message.HasMentionPrefix(client.CurrentUser, ref argPos) || message.HasCharPrefix('>', ref argPos))) return;

            // Create a Command Context
            var context = new CommandContext(client, message);
            // Execute the Command, store the result
            var result = await commands.ExecuteAsync(context, argPos, map);

            //Log Commands
            var gusr = (Discord.IGuildUser)message.Author;
            File.AppendAllText("commands.log", Environment.NewLine +
                $"[{DateTime.Now.GetDateTimeFormats()[110]}]{message.Content}" +
                $"(by {message.Author.Username}#{message.Author.Discriminator}" +
                $"(in {gusr.Guild.Name}))");
            // If the command failed, notify the user
            if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                await message.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");
        }
    }
}
