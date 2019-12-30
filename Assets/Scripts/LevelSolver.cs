using Priority_Queue;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class LevelSolver {

    #region fields
    public List<Move> moves;

    private SimplePriorityQueue<StateCell> toVisit;
    private HashSet<LevelState> visited;
    #endregion

    private class StateCell {
        public StateCell prev;
        public LevelState state;
        public int moves;

        public double Heuristic {
            get {
                return moves + LevelStateHeuristic.Value(state);
            }
        }

        public static StateCell Link(StateCell prev, LevelState state, int moves) {
            StateCell cell = new StateCell();
            cell.prev = prev;
            cell.state = state;
            cell.moves = moves;
            return cell;
        }
    }
    public IEnumerator SolveLevel(LevelState beginState) {

        moves = new List<Move>();
        toVisit = new SimplePriorityQueue<StateCell>();
        visited = new HashSet<LevelState>();

        visited.Add(beginState);

        AddNextMoves(StateCell.Link(null, beginState, 0));

        Stopwatch s = new Stopwatch();
        s.Start();

        while (toVisit.Count > 0) {

            StateCell current = toVisit.Dequeue();

            if (s.ElapsedMilliseconds > 1000) {
                Logger.Log("Visited: " + visited.Count + ", in queue: " + toVisit.Count + ", steps: " + current.moves);
                s.Reset();
                s.Start();
                yield return null;
            }

            if (!visited.Contains(current.state)) {

                visited.Add(current.state);

                if (current.state.Status == LevelStatus.Finish) {
                    FinishCell(current);
                    break;
                }

                AddNextMoves(current);
            }
        }

        Logger.Log("Found solution with " + moves.Count + " steps and visited " + visited.Count);
    }

    private void FinishCell(StateCell cell) {

        StateCell traverse = cell;

        while (traverse != null) {
            if (traverse.state.Move == Move.None) {
                break;
            }
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
            StateCell newCell = StateCell.Link(prevState, newState, prevState.moves + 1);
            if (newCell.state.Status == LevelStatus.Finish) {
                Logger.Log("Premature finish found");
            }
            toVisit.Enqueue(newCell, (float)newCell.Heuristic);
        }
    }
}