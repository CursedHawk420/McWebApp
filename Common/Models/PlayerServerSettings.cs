using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Models
{
    public class PlayerServerSettings
    {

        public string playerName;
        public string playerUuid;
        public List<string> joinedChannels;
        public string channelOut;
        public bool hasConnectedDiscord;
        public List<string> mutedPlayers;

        public PlayerServerSettings(string playerName, string playerUuid, List<string> joinedChannels, string channelOut, bool hasConnectedDiscord, List<string> mutedPlayers)
        {
            this.playerName = playerName;
            this.playerUuid = playerUuid;
            this.joinedChannels = joinedChannels;
            this.channelOut = channelOut;
            this.hasConnectedDiscord = hasConnectedDiscord;
            this.mutedPlayers = mutedPlayers;
        }

        public PlayerServerSettings() { }
    }
}
