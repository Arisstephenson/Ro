using System;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace DiscordExampleBot
{
    enum DataType
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
            client.Log += (async x => {
                await Console.Error.WriteLineAsync(x.Message);
            });
            var token = "insert token here";

            // Login and connect to Discord.
            Console.WriteLine("Logging in...");
            await client.LoginAsync(TokenType.Bot, token).ContinueWith(x =>
           {
               client.SetStatusAsync(UserStatus.Online);
               client.SetGameAsync("Type >help!", "", StreamType.Twitch);
           });
            await client.StartAsync();
            
            var map = new DependencyMap();
            Console.WriteLine("Adding Client to map...");
            map.Add(client);

            handler = new CommandHandler();
            Console.WriteLine("Installing map...");
            await handler.Install(map);

            // Block this program until it is closed.
            client.JoinedGuild += (async x => {
                await x.DefaultChannel.SendMessageAsync($"Hello I am Ro 😄.\rI am currently in early development so please report any problems to @Ariss#4202.\rTHANKS and have fun with me :).");
                Console.WriteLine($"I have joined the server {x.Name} :P");
                return;
            });
            Console.WriteLine("Ready!");
            await GetInput();
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
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
                }
            }
            return;
        }
    }
}
