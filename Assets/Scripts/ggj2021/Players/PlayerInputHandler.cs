using pdxpartyparrot.Game.Players.Input;
using pdxpartyparrot.ggj2021.Data.Players;

using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2021.Players
{
    public sealed class PlayerInputHandler : SideScollerPlayerInputHandler
    {
        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerInputData is PlayerInputData);
            Assert.IsTrue(Player is Player);
        }

        #endregion
    }
}
