using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ggj2021.NPCs;
using pdxpartyparrot.ggj2021.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    [RequireComponent(typeof(Collider))]
    public sealed class Platform : MonoBehaviour
    {
        [SerializeField]
        private PlatformWaypoint _initialWaypoint;

        [SerializeField]
        private float _speed = 5.0f;

        [SerializeField]
        private Transform _actorContainer;

        [SerializeField]
        [ReadOnly]
        private PlatformWaypoint _nextWaypoint;

        [SerializeReference]
        [ReadOnly]
        private ITimer _cooldown;

        #region Unity Lifecycle

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;

            _cooldown = TimeManager.Instance.AddTimer();

            SetWaypoint(_initialWaypoint);
        }

        private void OnDestroy()
        {
            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_cooldown);
                _cooldown = null;
            }

            for(int i = 0; i < _actorContainer.childCount; ++i) {
                Transform child = _actorContainer.GetChild(i);

                Player player = child.GetComponentInParent<Player>();
                if(null != player) {
                    OnPlayerTriggerExit(player);
                    continue;
                }

                Sheep sheep = child.GetComponentInParent<Sheep>();
                if(null != sheep) {
                    OnSheepTriggerExit(sheep);
                    continue;
                }
            }
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            if(null == _nextWaypoint || _cooldown.IsRunning) {
                return;
            }

            if(PartyParrotManager.Instance.IsPaused || !GameManager.Instance.IsGameReady || GameManager.Instance.IsGameOver) {
                return;
            }

            transform.MoveTowards(_nextWaypoint.transform.position, _speed * dt);

            if(Vector3.Distance(transform.position, _nextWaypoint.transform.position) < float.Epsilon) {
                _cooldown.Start(_nextWaypoint.Cooldown);

                transform.position = _nextWaypoint.transform.position;
                SetWaypoint(_nextWaypoint.NextWaypoint);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.gameObject.GetComponentInParent<Player>();
            if(null != player) {
                OnPlayerTriggerEnter(player);
                return;
            }

            Sheep sheep = other.gameObject.GetComponentInParent<Sheep>();
            if(null != sheep) {
                OnSheepTriggerEnter(sheep);
                return;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Player player = other.gameObject.GetComponentInParent<Player>();
            if(null != player) {
                OnPlayerTriggerExit(player);
                return;
            }

            Sheep sheep = other.gameObject.GetComponentInParent<Sheep>();
            if(null != sheep) {
                OnSheepTriggerExit(sheep);
                return;
            }
        }

        #endregion

        private void SetWaypoint(PlatformWaypoint waypoint)
        {
            _nextWaypoint = waypoint;

            if(null == _nextWaypoint) {
                Debug.Log("No next waypoint, stopping");
                return;
            }
        }

        private void OnPlayerTriggerEnter(Player player)
        {
            player.transform.SetParent(_actorContainer);
        }

        private void OnPlayerTriggerExit(Player player)
        {
            PlayerManager.Instance.ReclaimPlayer(player);
        }

        private void OnSheepTriggerEnter(Sheep sheep)
        {
            sheep.transform.SetParent(_actorContainer);
        }

        private void OnSheepTriggerExit(Sheep sheep)
        {
            sheep.transform.SetParent(GameManager.Instance.BaseLevel.SheepPen);
        }
    }
}
