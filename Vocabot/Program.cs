using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq; 

public class Program
{
    private DiscordSocketClient _client;
    private HttpClient _httpClient; 

    public static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        var config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        _client = new DiscordSocketClient(config);
        _httpClient = new HttpClient(); 

        _client.Log += Log;
        _client.MessageReceived += MessageReceivedAsync;

        // 2. Conectar (COLOQUE SEU TOKEN AQUI)
        string token = "SEU_TOKEN_DO_DISCORD_AQUI";

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private async Task MessageReceivedAsync(SocketMessage message)
    {
        if (message.Author.IsBot) return;

        // Comando: !voca [nome da música]
        if (message.Content.StartsWith("!voca "))
        {
            string busca = message.Content.Substring(6);
            await message.Channel.SendMessageAsync($"🔍 Procurando por **{busca}** no VocaDB...");

            string resultado = await BuscarMusicaNoVocaDB(busca);

            await message.Channel.SendMessageAsync(resultado);
        }
    }
    private async Task<string> BuscarMusicaNoVocaDB(string termoBusca)
    {
        try
        {
            string url = $"https://vocadb.net/api/songs?query={termoBusca}&maxResults=1&sort=Popularity&lang=Default";

            HttpResponseMessage resposta = await _httpClient.GetAsync(url);

            string jsonResposta = await resposta.Content.ReadAsStringAsync();

            var dados = JObject.Parse(jsonResposta);
            var items = dados["items"];

            if (!items.HasValues) return "❌ Não encontrei nada com esse nome.";

            var musica = items[0];
            string nomeMusica = musica["name"].ToString();
            string artista = musica["artistString"].ToString();
            int id = (int)musica["id"];

            return $"🎵 **Encontrei!**\n" +
                   $"**Nome:** {nomeMusica}\n" +
                   $"**Artistas:** {artista}\n" +
                   $"**Link:** https://vocadb.net/S/{id}";
        }
        catch (Exception ex)
        {
            return $"💥 Deu erro na API: {ex.Message}";
        }
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}