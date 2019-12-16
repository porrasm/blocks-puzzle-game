using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    #region fields
    private static Level current;

    public int Width { get; private set; } = 9;
    public int Height { get; private set; } = 9;

    private Move currentMove;
    private List<MoveState> states;
    private LevelState CurrentState {
        get {
            return states[states.Count - 1].State;
        }
    }

    private Dictionary<int, GamePiece> pieces;

    public struct MoveState {
        public Move Move;
        public LevelState State;
        public MoveState(Move move, LevelState state) {
            Move = move;
            State = state;
        }
    }
    #endregion

    private void Start() {
        pieces = new Dictionary<int, GamePiece>();
        states = new List<MoveState>();
        states.Add(InitialState());
        UpdateDictionary();
    }

    private void Update() {
        if (CheckMove()) {
            ExecuteMove();
        }
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            if (states.Count > 1) {
                states.RemoveAt(states.Count - 1);
                UpdateDictionary();
            }
        }
    }

    public GamePiece GetPiece(int id) {
        GamePiece piece;
        if (pieces.TryGetValue(id, out piece)) {
            return piece;
        }
        throw new System.Exception("Piece not found");
    }

    #region move
    private bool CheckMove() {

        int x = 0;
        int y = 0;

        if (Input.GetKeyDown(KeyCode.W)) {
            y++;
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            y--;
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            x++;
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            x--;
        }

        if (x == 0 && y == 0) {
            currentMove = Move.None;
            return false;
        }

        if (x == 1) {
            currentMove = Move.Right;
        } else if (x == -1) {
            currentMove = Move.Left;
        } else if (y == 1) {
            currentMove = Move.Up;
        } else if (y == -1) {
            currentMove = Move.Down;
        }
        return true;
    }

    private void ExecuteMove() {

        MoveState newState = new MoveState(currentMove, (LevelState)CurrentState.Clone());

        foreach (var pieces in newState.State.Field) {
            foreach (GamePiece piece in pieces) {
                Logger.Log("exevuting piece");
                Logger.Log("piec old x " + piece.X);
                piece.ExecuteMove(newState.Move);
                Logger.Log("piec new x " + piece.X);
            }
        }

        if (!newState.State.FixState()) {
            Logger.Log("Incorrect move");
            return;
        }

        states.Add(newState);
        UpdateDictionary();
    }
    #endregion

    // Testing only
    private MoveState InitialState() {

        FieldPiece[] pieces = GameObject.FindObjectsOfType<FieldPiece>();

        LevelState state = new LevelState(Width, Height);

        for (int i = 0; i < pieces.Length; i++) {

            GamePiece piece = pieces[i].GetInitialPiece();

            piece.X = (int)pieces[i].transform.localPosition.x;
            piece.Y = (int)pieces[i].transform.localPosition.y;

            pieces[i].PieceID = piece.ID;

            state.Field[piece.X, piece.Y].AddLast(piece);
        }

        return new MoveState(Move.None, state);
    }
    private void UpdateDictionary() {
        pieces = new Dictionary<int, GamePiece>();
        foreach (var pieces in CurrentState.Field) {
            foreach (GamePiece piece in pieces) {
                this.pieces.Add(piece.ID, piece);
                Logger.Log("Adding piece: " + piece.ID + ", " + piece);
            }
        }
    }

    public static Level Current {
        get {
            if (current == null) {
                current = GameObject.FindGameObjectWithTag("Level").GetComponent<Level>();
                NullCheck.Check(current);
            }
            return current;
        }
    }
}
