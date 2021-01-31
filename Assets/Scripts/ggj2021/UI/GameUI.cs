using UnityEngine;

namespace pdxpartyparrot.ggj2021.UI
{
    public sealed class GameUI : Game.UI.GameUI
    {
        [SerializeField]
        private PlayerHUD _playerHUD;

        public PlayerHUD PlayerHUD => _playerHUD;
    }
}
