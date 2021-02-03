using UnityEngine;

namespace pdxpartyparrot.ggj2021.UI
{
    public sealed class GameUI : Game.UI.GameUI
    {
        [SerializeField]
        private PlayerHUD _playerHUD;

        public PlayerHUD PlayerHUD => _playerHUD;

        [SerializeField]
        private GameObject _introTutorial;

        [SerializeField]
        private GameObject _gameOver;

        public void Hide()
        {
            _playerHUD.gameObject.SetActive(false);
            _introTutorial.gameObject.SetActive(false);
            _gameOver.gameObject.SetActive(false);
        }

        public void ShowHUD(bool show)
        {
            _playerHUD.gameObject.SetActive(show);
        }
    }
}
