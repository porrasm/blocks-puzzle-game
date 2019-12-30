using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class LevelStateHeuristic {

    #region fields
    private static float diameter = 12.728f;
    private static LevelState state;
    private static GamePiece[] pieces;
    private static PiecePair[] pairs;
    private static double distance;

    private struct PiecePair {
        public PlayPiece play;
        public FinishPiece finish;

        public PiecePair(PlayPiece play, FinishPiece finish) {
            this.play = play;
            this.finish = finish;
        }

        public double Distance {
            get {
                return Vector2.Distance(new Vector2(play.X, play.Y), new Vector2(finish.X, finish.Y));
            }
        }
    }
    #endregion

    #region initialization
    public static double GetHeuristicsValue(LevelState state) {
        LevelStateHeuristic.state = state;
        pieces = state.Pieces();
        SetPairs();

        EstimateHeuristics();

        double val = distance / pairs.Length;
        Reset();
        return val;
    }
    private static void Reset() {
        state = null;
        pairs = null;
        pieces = null;
        distance = 0;
    }

    private static void SetPairs() {
        Dictionary<Colors.BlockColor, PiecePair> pairs = new Dictionary<Colors.BlockColor, PiecePair>();

        foreach (GamePiece piece in pieces) {
            if (piece.Type == PieceType.PlayPiece) {

                var play = (PlayPiece)piece;

                if (pairs.ContainsKey(play.Color)) {
                    var pair = pairs[play.Color];
                    pair.play = play;
                    pairs[play.Color] = pair;
                } else {
                    var pair = new PiecePair();
                    pair.play = play;
                    pairs.Add(play.Color, pair);
                }

            } else if (piece.Type == PieceType.FinishPiece) {

                var finish = (FinishPiece)piece;

                if (pairs.ContainsKey(finish.Color)) {
                    var pair = pairs[finish.Color];
                    pair.finish = finish;
                    pairs[finish.Color] = pair;
                } else {
                    var pair = new PiecePair();
                    pair.finish = finish;
                    pairs.Add(finish.Color, pair);
                }
            }
        }

        LevelStateHeuristic.pairs = pairs.Values.ToArray();
    }
    #endregion

    #region heuristics
    private static void EstimateHeuristics() {
        if (!PiecesAreInCorrectSetup(pairs)) {
            // blocks in wrong setup, all 'max' distances
            IncorrectSetup();
        }

        AddGoalDistances();
    }

    private static void AddGoalDistances() {
        foreach (PiecePair pair in pairs) {
            distance += pair.Distance;
        }
    }

    private static void IncorrectSetup() {
        distance += pairs.Length * diameter;
    }

    private static bool PiecesAreInCorrectSetup(PiecePair[] pairs) {
        if (pairs.Length < 2) {
            return true;
        }
        for (int i = 0; i < pairs.Length - 1; i++) {
            Vector2 playVector = pairs[i + 1].play.PositionVector - pairs[i].play.PositionVector;
            Vector2 finishVector = pairs[i + 1].finish.PositionVector - pairs[i].finish.PositionVector;

            if (playVector != finishVector) {

                //Logger.Log("Play 1 pos: " + pairs[i + 1].play.PositionVector);
                //Logger.Log("Play 2 pos: " + pairs[i].play.PositionVector);
                //Logger.Log("Finish 1 pos: " + pairs[i + 1].finish.PositionVector);
                //Logger.Log("Finish 2 pos: " + pairs[i].finish.PositionVector);

                //Logger.Log(playVector + ", finish: " + finishVector);
                //throw new Exception("false");
                return false;
            }
        }
        return true;
    }
    #endregion
}

