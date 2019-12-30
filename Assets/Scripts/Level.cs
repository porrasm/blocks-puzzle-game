using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// REDUNDANT CLASS :(
public class Level {

    #region fields
    private static LevelContainer current;

    private Move CurrentMove { get; set; }
    public List<LevelState> States { get; private set; }
    public LevelState CurrentState {
        get {
            return States[States.Count - 1];
        }
    }

    private Dictionary<int, GamePiece> pieces;

    public bool Finished { get; private set; }

    public LevelSolver Solver { get; private set; }
    #endregion

    public Level(LevelState state) {
        pieces = new Dictionary<int, GamePiece>();
        States = new List<LevelState>();
        Solver = new LevelSolver(); 

        States.Add(state);
        UpdateDictionary();
    }

    public void Backtrack() {
        States.RemoveAt(States.Count - 1);
        UpdateDictionary();
    }
    public void Restart() {
        LevelState initial = States[0];
        States.Clear();
        States.Add(initial);
        UpdateDictionary();
    }

    public GamePiece GetPiece(int id) {
        GamePiece piece;
        if (pieces.TryGetValue(id, out piece)) {
            return piece;
        }
        throw new System.Exception("Piece not found");
    }

    public void ExecuteMove(Move move) {

        LevelState newState = LevelState.ExecuteMove(move, CurrentState);

        if (newState.Status == LevelStatus.Invalid) {
            Logger.Log("Incorrect move");
            Events.FireEvent(EventType.OnInvalidMove);

            // Incorrect move handling, block destroy animations??
            return;
        }

        States.Add(newState);
        UpdateDictionary();

        if (newState.Status == LevelStatus.Finish) {
            Logger.Log("Finished level");
            Finished = true;
            Events.FireEvent(EventType.OnLevelFinish);
        }
    }

    private void UpdateDictionary() {
        pieces = new Dictionary<int, GamePiece>();
        foreach (var pieces in CurrentState.Field) {
            foreach (GamePiece piece in pieces) {
                this.pieces.Add(piece.ID, piece);
            }
        }

        Events.FireEvent(EventType.OnFieldStateChange, CallbackData.Object(CurrentState));
    }

    public static LevelContainer Current {
        get {
            if (current == null) {
                current = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelContainer>();
                NullCheck.Check(current);
            }
            return current;
        }
    }
}
