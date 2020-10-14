//System
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

//Discord.Net
using Discord;
using Discord.Commands;
using Discord.WebSocket;

public class AmongUsMoudle : ModuleBase<SocketCommandContext>
{
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

    [Command("unmute", RunMode = RunMode.Async)]
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

            /*
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

            await ReplyAsync(embed: builder.Build());
            */
        }
    }

    [Command("mute", RunMode = RunMode.Async)]
    public async Task NewMute()
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
        finally
        {
            await ReplyAsync(embed: ManageMute(Context, true));

            /*
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

            await ReplyAsync(embed: builder.Build());
            */
        }
    }
}
