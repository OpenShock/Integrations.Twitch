using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

class Bot
{
    TwitchClient client;

    public Bot()
    {
        ConnectionCredentials credentials = new ConnectionCredentials("twitch_username", "access_token");
        var clientOptions = new ClientOptions
        {
            MessagesAllowedInPeriod = 750,
            ThrottlingPeriod = TimeSpan.FromSeconds(30)
        };
        WebSocketClient customClient = new WebSocketClient(clientOptions);
        client = new TwitchClient(customClient);
        client.Initialize(credentials, "channel");

        client.AddChatCommandIdentifier('!');

        client.OnLog += Client_OnLog;
        client.OnConnected += Client_OnConnected;
        client.OnChatCommandReceived += CommandHandler;

        client.Connect();
    }

    private void Client_OnLog(object? sender, OnLogArgs e)
    {
        Console.WriteLine($"{e.DateTime}: {e.BotUsername} - {e.Data}");
    }

    private void Client_OnConnected(object? sender, OnConnectedArgs e)
    {
        Console.WriteLine($"Connected to {e.AutoJoinChannel}");
    }

    private void CommandHandleShock(OnChatCommandReceivedArgs e, List<string> arguments)
    {
        client.SendMessage(e.Command.ChatMessage.Channel, "Shock command received!");
    }

    private void CommandHandleVibrate(OnChatCommandReceivedArgs e, List<string> arguments)
    {
        client.SendMessage(e.Command.ChatMessage.Channel, "Vibrate command received!");
    }

    private void CommandHandleBeep(OnChatCommandReceivedArgs e, List<string> arguments)
    {
        client.SendMessage(e.Command.ChatMessage.Channel, "Beep command received!");
    }

    private void CommandHandler(object? sender, OnChatCommandReceivedArgs e)
    {
        var cmd = e.Command.CommandText.ToLower();
        var args = e.Command.ArgumentsAsList;

        switch (cmd)
        {
            case "shock":
                CommandHandleShock(e, args);
                break;
            case "vibrate":
                CommandHandleVibrate(e, args);
                break;
            case "beep":
                CommandHandleBeep(e, args);
                break;
            default:
                return;
        }
    }
}