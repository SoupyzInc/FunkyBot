using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

/// <summary>
/// Contains all necessary methods to moderate Among Us games.
/// </summary>
public class AmongUsMoudle : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Checks for a message calling the unmute operation
    /// </summary>
    /// <param name="deadPersonsGiven">dead players who should remain muted.</param>
    /// <remarks>Umutes all players in the voice channel who are not pinged in the command.</remarks>
    [Command("unmute", RunMode = RunMode.Async)] //Allows for the method to run on a thread different from the gateway
    public async Task Unmute(params IUser[] deadPersonsGiven)
    {
        IVoiceChannel userChannel = (Context.User as IVoiceState).VoiceChannel; //Gives reference to the users voice channel

        //User Not In VC Error Handler
        try
        {
            var voiceUsers = Context.Guild.GetVoiceChannel(userChannel.Id).Users; //Attempts to grab all people in the vc
        }
        catch
        {
            //Error Handler Message
            var builderTest = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = "Join a voice channel to use the Unmute command.",
                },

                Footer = new EmbedFooterBuilder
                {
                    Text = "Requested by " + Context.User,
                    IconUrl = Context.User.GetAvatarUrl()
                },
            };

            builderTest.WithColor(0xFEFEFE)
                    .Build();

            await Context.Channel.SendMessageAsync(embed: builderTest.Build());

            return;
        }
        finally
        {
            await ReplyAsync(embed: ManageMute(Context, false, deadPersonsGiven));
        }
    }

    /// <summary>
    /// Checks for a messag calling the mute operation.
    /// </summary>
    /// <remarks>Mutes all players in the voice channel.</remarks>
    [Command("mute", RunMode = RunMode.Async)] //Allows for the method to run on a thread different from the gateway
    public async Task NewMute()
    {
        IVoiceChannel userChannel = (Context.User as IVoiceState).VoiceChannel; //Gives reference to the users voice channel

        try //Checks if user is in a voice channel
        {
            var voiceUsers = Context.Guild.GetVoiceChannel(userChannel.Id).Users; //Attempts to grab all people in the vc
        }
        catch //If not, returns an error message
        {
            //Error Handler Message
            var builderTest = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = "Join a voice channel to use the mute command.",
                },

                Footer = new EmbedFooterBuilder
                {
                    Text = "Requested by " + Context.User,
                    IconUrl = Context.User.GetAvatarUrl()
                },
            };

            builderTest.WithColor(0xFEFEFE)
                    .Build();

            await Context.Channel.SendMessageAsync(embed: builderTest.Build());
        }
        finally //If so, 
        {
            await ReplyAsync(embed: ManageMute(Context, true));
        }
    }

    /// <summary>
    /// Determines who in the voice channel should be muted or not and performs the mute ooperation.
    /// </summary>
    /// <param name="Context">the context of the users in the voice channel.</param>
    /// <param name="doMute">whether to mute or unmute the voice channel.</param>
    /// <param name="deadPeopleGiven">dead users who should remain muted and exluced from the mute operation.</param>
    /// <returns>And embeded message to be printed confirming the mute operation.</returns>
    public Embed ManageMute(SocketCommandContext Context, bool doMute, params IUser[] deadPeopleGiven)
    {
        IVoiceChannel userChannel = (Context.User as IVoiceState).VoiceChannel; //Gives reference to the users voice channel
        var voiceUsers = Context.Guild.GetVoiceChannel(userChannel.Id).Users; //grabs all the people in the vc
        List<SocketGuildUser> usersInChannel = voiceUsers.ToList(); //Puts them in a list

        List<SocketGuildUser> usersToMute = new List<SocketGuildUser>();
        List<SocketGuildUser> usersToRemove = new List<SocketGuildUser>();
        List<SocketGuildUser> deadPeopleToRemove = new List<SocketGuildUser>();

        for (int i = 0; i < usersInChannel.Count; i++)
        {
            usersToMute.Add(Context.Guild.GetUser(usersInChannel[i].Id)); //Converts the all the user's IUsers to SocketGuildUsers
        }
        for (int i = 0; i < usersInChannel.Count; i++) //Removes Bots
        {
            if (usersInChannel[i].IsBot)
            {
                usersToRemove.Add(usersInChannel[i]);
            }
        }

        foreach (var person in deadPeopleGiven)
        {
            deadPeopleToRemove.Add(Context.Guild.GetUser(person.Id));
        }

        usersToMute = usersToMute.Except(usersToRemove).ToList();

        MuteOperation(doMute, usersToMute, deadPeopleToRemove);

        if (doMute == true)
        {
            var builder = new EmbedBuilder
            {
                Description = ":mute: **User(s) have been muted.**",

                Footer = new EmbedFooterBuilder
                {
                    Text = "Requested by " + Context.User,
                    IconUrl = Context.User.GetAvatarUrl()
                },
            };

            builder.WithColor(0xFEFEFE)
                .Build();
            return builder.Build();
        }
        else
        {
            var builder = new EmbedBuilder
            {
                Description = ":speaker: **User(s) have been unmuted.**",

                Footer = new EmbedFooterBuilder
                {
                    Text = "Requested by " + Context.User,
                    IconUrl = Context.User.GetAvatarUrl()
                },
            };

            builder.WithColor(0xFEFEFE)
                .Build();
            return builder.Build();
        }
    }

    /// <summary>
    /// Mutes and unmutes necessary players in the voice channel.
    /// </summary>
    /// <param name="Operation">whether to mute or unmute necessary players</param>
    /// <param name="UsersToMute">a list of users to mute.</param>
    /// <param name="deadPeople">a list of dead players who should remain muted.</param>
    public async void MuteOperation(bool Operation, List<SocketGuildUser> UsersToMute, List<SocketGuildUser> deadPeople)
    {
        if (Operation == true) //Mute
        {
            foreach (var user in UsersToMute)
            {
                await user.ModifyAsync(m => { m.Mute = true; });
            }
        }
        else if (Operation == false) //Unmute
        {
            UsersToMute = UsersToMute.Except(deadPeople).ToList();
            foreach (var user in UsersToMute)
            {
                await user.ModifyAsync(m => { m.Mute = false; });
            }
        }
    }
}
