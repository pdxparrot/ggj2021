using System.Collections.Generic;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.UI;
using pdxpartyparrot.ggj2021.World;

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

        [SerializeField]
        private GameObject _compassNeedleContainer;

        private readonly List<CompassNeedle> _compassNeedles = new List<CompassNeedle>();

        public void Reset(int goal)
        {
            if(goal == 0) {
                _compassNeedles.Clear();
                _compassNeedleContainer.transform.Clear();
            }

            _score.text = "0";
            _goal.text = goal.ToString();

            foreach(GameObject slot in _slots) {
                slot.SetActive(false);
            }

            UpdateTimer(1.0f);
        }

        public void AddCompassNeedle(CompassNeedle needlePrefab, Goal goal)
        {
            CompassNeedle compassNeedle = Instantiate(needlePrefab, _compassNeedleContainer.transform);
            compassNeedle.Initialize(goal);

            _compassNeedles.Add(compassNeedle);
        }

        public void UpdateTimer(float pctRemaining)
        {
            Vector3 rot = _timer.eulerAngles;
            rot.z = (1.0f - pctRemaining) * -180.0f;
            _timer.eulerAngles = rot;
        }

        public void UpdateGoalCompass(Vector3 playerPosition)
        {
            foreach(CompassNeedle needle in _compassNeedles) {
                needle.UpdateAngle(playerPosition);
            }
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
