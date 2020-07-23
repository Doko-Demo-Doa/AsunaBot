using Discord.Commands;
using NadekoBot.Common.Attributes;
using NadekoBot.Core.Services;
using NadekoBot.Extensions;
using NadekoBot.Modules.Permissions.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NadekoBot.Modules.Test
{
    public class Test : NadekoTopLevelModule<TestService>
    {
        private readonly IBotCredentials _creds;
        private readonly CommandService _cmds;
        private readonly GlobalPermissionService _perms;
        private readonly IServiceProvider _services;

        public Test(IBotCredentials creds, GlobalPermissionService perms, CommandService cmds,
            IServiceProvider services)
        {
            _creds = creds;
            _cmds = cmds;
            _perms = perms;
            _services = services;
        }

        [NadekoCommand, Usage, Description, Aliases]
        public async Task Demo()
        {
            await ctx.Channel.SendMessageAsync("Demo");
        }
    }

    public class TestService : INService
    {

    }
}
