using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level {

    #region fields
    private static LevelContainer current;

    private Move CurrentMove { get; set; }
    public List<MoveState> States { get; private set; }
    public MoveState CurrentState {
        get {
            return States[States.Count - 1];
        }
    }

    private Dictionary<int, GamePiece> pieces;

    public bool Finished { get; private set; }
    #endregion

    public Level(MoveState state) {
        pieces = new Dictionary<int, GamePiece>();
        States = new List<MoveState>();

        States.Add(state);
        UpdateDictionary();
    }

    public void Backtrack() {
        States.RemoveAt(States.Count - 1);
        UpdateDictionary();
    }
    public void Restart() {
        MoveState initial = States[0];
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
    private static LevelState.Status ExecuteMove(Level level, Move move) {

        MoveState newState = new MoveState(move, (LevelState)level.CurrentState.State.Clone());

        foreach (var pieces in newState.State.Field) {
            foreach (GamePiece piece in pieces) {
                piece.UpdateMove(newState);
            }
        }

        LevelState.Status status = LevelState.FixState(newState);

        if (status == LevelState.Status.Invalid) {
            Logger.Log("Incorrect move");
            Events.FireEvent(EventType.OnInvalidMove);
            return status;
        }

        level.States.Add(newState);
        level.UpdateDictionary();

        if (status == LevelState.Status.Finish) {
            Logger.Log("Finished level");
            level.Finished = true;
            Events.FireEvent(EventType.OnLevelFinish);
        }

        return status;
    }
    #endregion

    private void UpdateDictionary() {
        pieces = new Dictionary<int, GamePiece>();
        foreach (var pieces in CurrentState.State.Field) {
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
