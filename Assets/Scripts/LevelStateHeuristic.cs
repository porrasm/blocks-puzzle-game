using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class LevelStateHeuristic {

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

    public static double Value(LevelState state) {
        Dictionary<Colors.BlockColor, PiecePair> pairs = GetPairs(state);
        double distance = 0;

        foreach (PiecePair pair in pairs.Values) {
            distance += pair.Distance;
        }

        return distance;
    }
    private static Dictionary<Colors.BlockColor, PiecePair> GetPairs(LevelState state) {
        Dictionary<Colors.BlockColor, PiecePair> pairs = new Dictionary<Colors.BlockColor, PiecePair>();

        state.IteratePieces((piece) => {
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
        });

        return pairs;
    }
}

