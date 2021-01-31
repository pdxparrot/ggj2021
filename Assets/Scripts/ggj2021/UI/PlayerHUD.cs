using pdxpartyparrot.Game.UI;

using TMPro;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.UI
{
    public sealed class PlayerHUD : HUD
    {
        [SerializeField]
        private TextMeshProUGUI _score;

        [SerializeField]
        private TextMeshProUGUI _goal;

        public void UpdateScore(int score)
        {
            _score.text = score.ToString();
        }

        public void SetGoal(int goal)
        {
            _goal.text = goal.ToString();
        }
    }
}
