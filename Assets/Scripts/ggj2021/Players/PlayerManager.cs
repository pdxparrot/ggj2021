using pdxpartyparrot.Game.Players;
using pdxpartyparrot.ggj2021.Data.Players;

namespace pdxpartyparrot.ggj2021.Players
{
    public sealed class PlayerManager : PlayerManager<PlayerManager>
    {
        public PlayerData GamePlayerData => (PlayerData)PlayerData;
    }
}
