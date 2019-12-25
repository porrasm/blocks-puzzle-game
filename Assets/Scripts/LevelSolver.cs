using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSolver : MonoBehaviour {

    #region fields
    public Level level;

    public List<Move> moves;

    private Queue<LevelState> toVisit;
    private HashSet<LevelState> visited;
    #endregion

    private class StateCell {
        public StateCell prev;
        public MoveState state;
    }

    public void SolveLevel(LevelState level) {
        this.level = new Level(new MoveState(Move.None, level));
        moves = new List<Move>();
        toVisit = new Queue<LevelState>();
        visited = new HashSet<LevelState>();

        visited.Add(level);

        while (toVisit.Count > 0) {

        }
    }

    private void AddNextMoves(LevelState state) {

        

    }
}