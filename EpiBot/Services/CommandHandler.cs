using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;

namespace EpiBot.Services
{
  public class CommandHandler
  {
    private readonly DiscordSocketClient _discord;
    private readonly CommandService _commands;
    private readonly InteractionService _interactions;
    private readonly IConfigurationRoot _config;
    private readonly IServiceProvider _provider;

    // DiscordSocketClient, CommandService, IConfigurationRoot, and IServiceProvider are injected automatically from the IServiceProvider
    public CommandHandler(
        DiscordSocketClient discord,
        CommandService commands,
        InteractionService interactions,
        IConfigurationRoot config,
        IServiceProvider provider)
    {
      _discord = discord;
      _commands = commands;
      _interactions = interactions;
      _config = config;
      _provider = provider;

      // attach method for handling message commands
      _discord.MessageReceived += OnMessageReceivedAsync;

      // attach method for handling interaction commands
      _discord.InteractionCreated += HandleInteractionAsync;
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

    private async Task HandleInteractionAsync(SocketInteraction arg)
    {
      try
      {
        var context = new SocketInteractionContext(_discord, arg);
        await _interactions.ExecuteCommandAsync(context, _provider);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        // the below deletes the original response of the interaction, if the interaction fails
        if (arg.Type == Discord.InteractionType.ApplicationCommand)
        {
          await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
        }
      }
    }
  }
}