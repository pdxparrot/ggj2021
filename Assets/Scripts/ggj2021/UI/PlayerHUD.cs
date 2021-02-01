using pdxpartyparrot.Game.UI;

using TMPro;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.UI
{
    public sealed class PlayerHUD : HUD
    {
        [SerializeField]
        private Transform _timer;

        [SerializeField]
        private TextMeshProUGUI _score;

        [SerializeField]
        private TextMeshProUGUI _goal;

        [SerializeField]
        private GameObject[] _slots;

        public void Reset(int goal)
        {
            _score.text = "0";
            _goal.text = goal.ToString();

            foreach(GameObject slot in _slots) {
                slot.SetActive(false);
            }

            UpdateTimer(1.0f);
        }

        public void UpdateTimer(float pctRemaining)
        {
            Vector3 rot = _timer.eulerAngles;
            rot.z = (1.0f - pctRemaining) * -180.0f;
            _timer.eulerAngles = rot;
        }

        public void UpdateScore(int score)
        {
            _score.text = score.ToString();
        }

        public void AddSlot()
        {
            for(int i = 0; i < _slots.Length; ++i) {
                if(!_slots[i].activeInHierarchy) {
                    _slots[i].SetActive(true);
                    break;
                }
            }
        }

        public void RemoveSlot()
        {
            for(int i = _slots.Length - 1; i >= 0; --i) {
                if(_slots[i].activeInHierarchy) {
                    _slots[i].SetActive(false);
                    break;
                }
            }
        }
    }
}
