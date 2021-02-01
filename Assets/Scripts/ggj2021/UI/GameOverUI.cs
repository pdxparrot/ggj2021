using pdxpartyparrot.Core.UI;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.UI
{
    [RequireComponent(typeof(UIObject))]
    public sealed class GameOverUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _winUI;

        [SerializeField]
        private GameObject _lossUI;

        #region Unity Lifecycle

        private void OnEnable()
        {
            _winUI.SetActive(GameManager.Instance.IsGameWon);
            _lossUI.SetActive(GameManager.Instance.IsGameLost);
        }

        #endregion
    }
}
