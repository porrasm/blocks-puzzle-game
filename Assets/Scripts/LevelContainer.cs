using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContainer : MonoBehaviour {

    #region fields
    public int Width { get; private set; } = 9;
    public int Height { get; private set; } = 9;

    private static LevelContainer current;
    private Move currentMove;
    private Level level;

    #endregion

    private void Start() {
        level = new Level(InitialState());

        Events.FireEvent(EventType.OnLevelStart);
    }

    private void Update() {
        if (CheckMove()) {
            Events.FireEvent(EventType.OnMoveStart, CallbackData.Object(currentMove));
            level.ExecuteMove(currentMove);
            Events.FireEvent(EventType.OnMoveEnd, CallbackData.Object(currentMove));
        }
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            if (level.States.Count > 1) {
                level.Backtrack();
            }
        }
    }

    public GamePiece GetPiece(int id) {
        return level.GetPiece(id);
    }

    #region move
    private bool CheckMove() {

        if (level.Finished) {
            return false;
        }

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
        if (Input.GetKeyDown(KeyCode.R)) {
            level.Restart();
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
    #endregion

    // Testing only
    private LevelState InitialState() {

        FieldPiece[] pieces = GameObject.FindObjectsOfType<FieldPiece>();

        LevelState state = new LevelState(Width, Height);

        for (int i = 0; i < pieces.Length; i++) {

            GamePiece piece = pieces[i].GetInitialPiece();

            piece.X = (int)pieces[i].transform.localPosition.x;
            piece.Y = (int)pieces[i].transform.localPosition.y;

            pieces[i].PieceID = piece.ID;

            state.Field[piece.X, piece.Y].Add(piece);
        }

        return state;
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
