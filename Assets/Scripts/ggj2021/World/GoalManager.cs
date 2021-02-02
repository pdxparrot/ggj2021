using System.Collections.Generic;

using pdxpartyparrot.Core.Collections;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    public sealed class GoalManager : SingletonBehavior<GoalManager>
    {
        private readonly HashSet<Goal> _goals = new HashSet<Goal>();

        public void RegisterGoal(Goal goal)
        {
            _goals.Add(goal);
        }

        public void UnRegisterGoal(Goal goal)
        {
            _goals.Remove(goal);
        }

        public Goal GetNearestGoal(Transform transform)
        {
            return _goals.NearestManhattan(transform.position, out _);
        }
    }
}
