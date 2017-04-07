using System;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace DiscordExampleBot
{
    public enum DataType
    {
        Vars = 'v',
        Levels = 'l',
        Tags = 't',
    }
    static class Em
    {
        public static string GetEmoji(string val)
        {
            if (val == "a") return a;
            if (val == "b") return b;
            if (val == "c") return c;
            if (val == "d") return d;
            if (val == "e") return e;
            if (val == "f") return f;
            if (val == "g") return g;
            if (val == "h") return h;
            if (val == "i") return i;
            if (val == "j") return j;
            if (val == "k") return k;
            if (val == "l") return l;
            if (val == "m") return m;
            if (val == "n") return n;
            if (val == "o") return o;
            if (val == "p") return p;
            if (val == "q") return q;
            if (val == "r") return r;
            if (val == "s") return s;
            if (val == "t") return t;
            if (val == "u") return u;
            if (val == "v") return v;
            if (val == "w") return w;
            if (val == "x") return x;
            if (val == "y") return y;
            if (val == "z") return z;
            else return "";
        }
        public static string a = "🇦";
        public static string b = "🇧";
        public static string c = "🇨";
        public static string d = "🇩";
        public static string e = "🇪";
        public static string f = "🇫";
        public static string g = "🇬";
        public static string h = "🇭";
        public static string i = "🇮";
        public static string j = "🇯";
        public static string k = "🇰";
        public static string l = "🇱";
        public static string m = "🇲";
        public static string n = "🇳";
        public static string o = "🇴";
        public static string p = "🇵";
        public static string q = "🇶";
        public static string r = "🇷";
        public static string s = "🇸";
        public static string t = "🇹";
        public static string u = "🇺";
        public static string v = "🇻";
        public static string w = "🇼";
        public static string x = "🇽";
        public static string y = "🇾";
        public static string z = "🇿";
    }
    public class Program
    {
		
        // string data;
        // Convert our sync main to an async main.
        public static void Main(string[] args) =>
            new Program().Start().GetAwaiter().GetResult();
        private DiscordSocketClient client;
        private CommandHandler handler;

        public async Task Start()
        {
            // Define the DiscordSocketClient
            client = new DiscordSocketClient();
            client.Log += (x => {
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
			var token = File.ReadAllText("token.txt");

            // Login and connect to Discord.
            await client.LoginAsync(TokenType.Bot, token).ContinueWith(x =>
           {
               client.SetStatusAsync(UserStatus.Online);
               client.SetGameAsync("Type >help!", "", StreamType.Twitch);
           });
            await client.StartAsync();
            
            var map = new DependencyMap();
            map.Add(client);

            handler = new CommandHandler();
            await handler.Install(map);

            // Block this program until it is closed.
            client.JoinedGuild += (async x => {
                await x.DefaultChannel.SendMessageAsync($"Hello I am 😄.\rI am currently in early development so please report any problems to @Ariss#4202.\rTHANKS and have fun with me :).");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"I have joined the server {x.Name}.");
                return;
            });
            await GetInput();
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
			return System.Threading.Tasks.Task.CompletedTask;
        }
        async private Task GetInput()
        {
            int a = 1;
            while (a == 1)
            {
                string input = Console.ReadLine();
                if (input == "stop")
                {
                    Console.Write("Setting Status to offline...");
                    await client.SetStatusAsync(UserStatus.Invisible);
                    Console.Write("Stopping...");
                    await client.StopAsync();
                    Console.WriteLine("Logging Out...");
                    await client.LogoutAsync();
                    Console.WriteLine("The bot has been shut down, until next time ;)");
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            return;
        }
    }
}
