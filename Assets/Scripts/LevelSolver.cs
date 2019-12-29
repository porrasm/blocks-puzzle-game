using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSolver {

    #region fields
    public List<Move> moves;

    private Queue<StateCell> toVisit;
    private HashSet<LevelState> visited;
    #endregion

    private class StateCell {
        public StateCell prev;
        public LevelState state;

        public static StateCell Link(StateCell prev, LevelState state) {
            StateCell cell = new StateCell();
            cell.prev = prev;
            cell.state = state;
            return cell;
        }
    }

    public IEnumerator SolveLevel(LevelState beginState) {

        moves = new List<Move>();
        toVisit = new Queue<StateCell>();
        visited = new HashSet<LevelState>();

        visited.Add(beginState);

        AddNextMoves(StateCell.Link(null, beginState));

        int safe = 10000;

        while (toVisit.Count > 0 && safe > 0) {

            yield return null;

            safe--;

            StateCell current = toVisit.Dequeue();
            
            if (!visited.Contains(current.state)) {

                visited.Add(current.state);

                if (current.state.Status == LevelStatus.Finish) {
                    FinishCell(current);
                    break;
                }

                AddNextMoves(current);
            }
        }

        if (safe <= 0) {
            Logger.LogError("Not found");
        }

        Logger.Log("Found solution with " + moves.Count + " steps");
    }

    private void FinishCell(StateCell cell) {

        StateCell traverse = cell;

        while (traverse != null) {
            moves.Add(traverse.state.Move);
            traverse = traverse.prev;
        }

        moves.Reverse();
    }

    private Move[] Moves() {
        return new Move[4] { Move.Up, Move.Right, Move.Down, Move.Left };
    }

    private void AddNextMoves(StateCell prevState) {
        foreach (Move move in Moves()) {
            LevelState newState = LevelState.ExecuteMove(move, prevState.state);
            toVisit.Enqueue(StateCell.Link(prevState, newState));
        }
    }
}