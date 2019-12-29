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
    #endregion

    public Level(LevelState state) {
        pieces = new Dictionary<int, GamePiece>();
        States = new List<LevelState>();

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
        Level.ExecuteMove(this, move);
    }

    #region move
    private static LevelStatus ExecuteMove(Level level, Move move) {

        Logger.Log("Exectuing move: " + move);

        LevelState newState = level.CurrentState.CopyState(move);

        Logger.Log("New state move: " + newState.Move);

        foreach (var pieces in newState.Field) {
            foreach (GamePiece piece in pieces) {
                Logger.Log("Update move: " + piece);
                piece.UpdateMove(newState);
            }
        }

        LevelStatus status = LevelState.FixState(newState);

        if (status == LevelStatus.Invalid) {
            Logger.Log("Incorrect move");
            Events.FireEvent(EventType.OnInvalidMove);
            return status;
        }

        level.States.Add(newState);
        level.UpdateDictionary();

        if (status == LevelStatus.Finish) {
            Logger.Log("Finished level");
            level.Finished = true;
            Events.FireEvent(EventType.OnLevelFinish);
        }

        return status;
    }

    
    #endregion

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
