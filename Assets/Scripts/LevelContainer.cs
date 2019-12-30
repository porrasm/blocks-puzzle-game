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

            Logger.Log("State count: " + level.States.Count);

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

    LevelState a;
    LevelState b;

    #region move
    private bool CheckMove() {

        currentMove = Move.None;

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
        if (Input.GetKeyDown(KeyCode.Return)) {
            StartCoroutine(level.Solver.SolveLevel(level.CurrentState));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            Move move = level.Solver.moves[0];
            level.Solver.moves.RemoveAt(0);
            currentMove = move;
            return true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            a = level.CurrentState;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            b = level.CurrentState;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            Logger.Log("Check equals: " + a.Equals(b));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            Logger.Log("State heur: " + LevelStateHeuristic.GetHeuristicsValue(level.CurrentState));
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

            //state.Field[piece.X, piece.Y].Add(piece);
            state.AddPiece(piece);
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
