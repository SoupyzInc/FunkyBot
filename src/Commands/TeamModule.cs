//System
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

//Discod.Net
using Discord;
using Discord.Commands;
using Discord.WebSocket;

public class TeamModule : ModuleBase<SocketCommandContext>
{
    [Command("Team")]
    public async Task Team(params IUser[] usersToExclude)
    {
        DiscordSocketClient targetClient = Context.Client; //Gets the reference to the dude who said the message
        IVoiceChannel userChannel = (Context.User as IVoiceState).VoiceChannel; //Gives reference to the users voice channel

        //User Not In VC Error Handler
        try
        {
            var voiceUsers = Context.Guild.GetVoiceChannel(userChannel.Id).Users; //Attempts to grab all people in vc
        }
        catch
        {
            //Error Handler Message
            var builderTest = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = "Join a voice channel to use the team command.",
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
            var voiceUsers = Context.Guild.GetVoiceChannel(userChannel.Id).Users; //Grabs all the people in the vc
            List<SocketGuildUser> usersInChannel = voiceUsers.ToList(); //Puts them in a list

            List<SocketGuildUser> usersToParse = new List<SocketGuildUser>();
            List<SocketGuildUser> usersToRemove = new List<SocketGuildUser>();

            for (int i = 0; i < usersInChannel.Count; i++)
            {
                usersToParse.Add(Context.Guild.GetUser(usersInChannel[i].Id)); //Converts the all the user's IUsers to SocketGuildUsers
            }
            for (int i = 0; i < usersInChannel.Count; i++) //Removes Bots
            {
                if (usersInChannel[i].IsBot)
                {
                    usersToRemove.Add(usersInChannel[i]);
                }
            }

            for (int i = 0; i < usersToExclude.Length; i++)
            {
                usersToRemove.Add(Context.Guild.GetUser(usersToExclude[i].Id)); //Converts all the unwanted user's IUsers to SocketGuildUsers
            }

            List<SocketGuildUser> usersToTeam = new List<SocketGuildUser>();
            usersToTeam = usersToParse.Except(usersToRemove).ToList(); //Removes unwanted users

            //Team Creation
            Random rnd = new Random();
            List<SocketGuildUser> sortedUsers = usersToTeam.OrderBy(x => rnd.Next()).ToList(); //Randomly sorts users

            List<SocketGuildUser> blueTeam = new List<SocketGuildUser>(); //Generate a blue team
            List<SocketGuildUser> redTeam = new List<SocketGuildUser>(); //Generate a red team

            List<string> blueTeamUsers = new List<string>(); //Make a list of all blue team players
            List<string> redTeamUsers = new List<string>(); //Make a list of all red team players

            for (int i = 0; i < (sortedUsers.Count / 2); i++) //Add half the players to red team
            {
                redTeam.Add(sortedUsers[i]);
                redTeamUsers.Add(sortedUsers[i].Id.ToString()); //Adds the red team players to the red list
            }

            blueTeam = sortedUsers.Except(redTeam).ToList(); //Add the other half to blue team

            for (int i = 0; i < blueTeam.Count; i++)
            {
                blueTeamUsers.Add(blueTeam[i].Id.ToString()); //Adds the blue team players to the blue list
            }

            string blueText = string.Join(">, <@", blueTeamUsers.ToArray()); //Combines blue string array into a long string
            string redText = string.Join(">, <@", redTeamUsers.ToArray()); //Combines red string array into a long string

            string redMessage = "";
            string blueMessage = "";

            if (!string.IsNullOrWhiteSpace(redText))
            {
                redMessage = "<@" + redText + "> ";
            }

            if (!string.IsNullOrWhiteSpace(blueText))
            {
                blueMessage = "<@" + blueText + "> ";
            }

            //Message Handling
            Discord.Color lightBlue = new Discord.Color(0xd4e9f1);
            var builder = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = "Teams",
                },

                Description = "**:red_square: | **" + redMessage + "\n" +
                                "**:blue_square: | **" + blueMessage,

                Footer = new EmbedFooterBuilder
                {
                    Text = "This function is powered by FunkyBot™.",
                    IconUrl = "https://cdn.discordapp.com/attachments/736361743139209307/742053144678105258/Funky_Bot.png"
                },
            };

            builder.WithColor(lightBlue)
                    .Build();

            await Context.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
} 
