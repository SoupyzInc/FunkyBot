//System
using System.Threading.Tasks;
using System.Text.RegularExpressions;

//Discod.NET
using Discord;
using Discord.Commands;


[Group("help")]
public class HelpModule : ModuleBase<SocketCommandContext>
{
    readonly string soupyzLLCImg = "https://cdn.discordapp.com/attachments/734935840282771598/741382917653004431/S_Monochrome.png";
    readonly Discord.Color white = new Discord.Color(0xFEFEFE);

    [Command()]
    public async Task Help()
    {
        var builder = new EmbedBuilder
        {
            Author = new EmbedAuthorBuilder
            {
                Name = "SoupyzLLC Documentation",
            },

            ThumbnailUrl = soupyzLLCImg,

            Description = "`.help team`\n`.help r6`\n`.help mute`\n`.help unmute`",

            Footer = new EmbedFooterBuilder
            {
                Text = "Requested by " + Context.User,
                IconUrl = Context.User.GetAvatarUrl()
            },
        };

        builder.WithColor(white)
                .Build();

        await Context.Channel.SendMessageAsync(embed: builder.Build());
    }

    [Command("team")]
    public async Task HelpTeam()
    {
        var builder = new EmbedBuilder
        {
            Author = new EmbedAuthorBuilder
            {
                Name = ".team Documentation",
            },

            ThumbnailUrl = soupyzLLCImg,

            Description = "`.team @User1 @User2`\nCreates two teams from people in your voice channel (excluding bots).\n*User(s) can be excluded from the team draft by mentioning them.",

            Footer = new EmbedFooterBuilder
            {
                Text = "Requested by " + Context.User,
                IconUrl = Context.User.GetAvatarUrl()
            },
        };

        builder.WithColor(white)
                .Build();

        await Context.Channel.SendMessageAsync(embed: builder.Build());
    }

    [Command("r6")]
    public async Task HelpRSix()
    {
        var builder = new EmbedBuilder
        {
            Author = new EmbedAuthorBuilder
            {
                Name = ".r6 Documentation",
            },

            ThumbnailUrl = soupyzLLCImg,

            Description = "`.r6 PlayerUplayName Platform`\nSearches [r6tracker](https://r6.tracker.network/) for the specified player's stats.",

            Footer = new EmbedFooterBuilder
            {
                Text = "Requested by " + Context.User,
                IconUrl = Context.User.GetAvatarUrl()
            },
        };

        builder.WithColor(white)
                .Build();

        await Context.Channel.SendMessageAsync(embed: builder.Build());
    }

    [Command("mute")]
    public async Task HelpMute()
    {
        var builder = new EmbedBuilder
        {
            Author = new EmbedAuthorBuilder
            {
                Name = ".mute Documentation",
            },

            ThumbnailUrl = soupyzLLCImg,

            Description = "`.mute`\nServer mutes everyone in your voice channel.",

            Footer = new EmbedFooterBuilder
            {
                Text = "Requested by " + Context.User,
                IconUrl = Context.User.GetAvatarUrl()
            },
        };

        builder.WithColor(white)
                .Build();

        await Context.Channel.SendMessageAsync(embed: builder.Build());
    }

    [Command("unmute")]
    public async Task HelpUnmute()
    {
        var builder = new EmbedBuilder
        {
            Author = new EmbedAuthorBuilder
            {
                Name = ".unmute Documentation",
            },

            ThumbnailUrl = soupyzLLCImg,

            Description = "`.unmute @User1 @User2`\nUnserver mutes everyone in your voice channel.\n*User(s) can be excluded from the unmute by mentioning them.",

            Footer = new EmbedFooterBuilder
            {
                Text = "Requested by " + Context.User,
                IconUrl = Context.User.GetAvatarUrl()
            },
        };

        builder.WithColor(white)
                .Build();

        await Context.Channel.SendMessageAsync(embed: builder.Build());
    }
}
