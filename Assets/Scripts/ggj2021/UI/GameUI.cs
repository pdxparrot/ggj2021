using UnityEngine;

namespace pdxpartyparrot.ggj2021.UI
{
    public sealed class GameUI : Game.UI.GameUI
    {
        [SerializeField]
        private PlayerHUD _playerHUD;

        public PlayerHUD PlayerHUD => _playerHUD;

        [SerializeField]
        private GameObject _gameOverUI;

        public void ShowHUD()
        {
            _playerHUD.gameObject.SetActive(true);
            _gameOverUI.SetActive(false);
        }

        public void ShowGameOver()
        {
            _playerHUD.gameObject.SetActive(false);
            _gameOverUI.SetActive(true);
        }
    }
}
