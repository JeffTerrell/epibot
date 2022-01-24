using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;

namespace EpiBot
{
  public class CommandHandler
  {
    private readonly DiscordSocketClient _discord;
    private readonly CommandService _commands;
    private readonly IConfigurationRoot _config;
    private readonly IServiceProvider _provider;

    // DiscordSocketClient, CommandService, IConfigurationRoot, and IServiceProvider are injected automatically from the IServiceProvider
    public CommandHandler(
        DiscordSocketClient discord,
        CommandService commands,
        IConfigurationRoot config,
        IServiceProvider provider)
    {
      _discord = discord;
      _commands = commands;
      _config = config;
      _provider = provider;

      _discord.MessageReceived += OnMessageReceivedAsync;
    }

    private async Task OnMessageReceivedAsync(SocketMessage s)
    {
      //Check to see if real command from user/bot and not self
      var msg = s as SocketUserMessage;
      if (msg == null) return;
      if (msg.Author.Id == _discord.CurrentUser.Id) return;
      //Creating and executing the command
      var context = new SocketCommandContext(_discord, msg);
      int argPos = 0;
      string prefix = "!";
      if (msg.HasStringPrefix(prefix, ref argPos) || msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
      {
        var result = await _commands.ExecuteAsync(context, argPos, _provider);

        if (!result.IsSuccess)
        {
          await context.Channel.SendMessageAsync(result.ToString());
        }
      }
    }
  }
}