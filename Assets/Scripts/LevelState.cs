using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LevelState : ICloneable {

    #region fields
    public LinkedList<GamePiece>[,] Field { get; private set; }
    #endregion

    public enum Status {
        Normal = 0,
        Invalid = -1,
        Finish = 1
    }

    public LevelState(int width, int height) {
        Field = new LinkedList<GamePiece>[width, height];
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                Field[i, j] = new LinkedList<GamePiece>();
            }
        }
    }

    #region state modification
    public static Status FixState(Level.MoveState moveState) {

        int width = moveState.State.Field.GetLength(0);
        int height = moveState.State.Field.GetLength(1);

        LinkedList<GamePiece>[,] newField = new LinkedList<GamePiece>[width, height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                newField[x, y] = new LinkedList<GamePiece>();
            }
        }

        foreach (var pieces in moveState.State.Field) {
            foreach (GamePiece piece in pieces) {
                //if (moveState.State.IncorrectPiecePosition(piece)) {
                //    return Status.Invalid;
                //}
                newField[piece.X, piece.Y].AddLast(piece);
            }
        }

        moveState.State.Field = null;
        moveState.State.Field = newField;

        foreach (var panel in moveState.State.Field) {
            if (IncorrectPanel(panel)) {
                return Status.Invalid;
            }
        }

        moveState.State.IteratePieces((piece) => {
            piece.OnFixState(moveState);
        });

        if (moveState.State.CheckFinish()) {
            return Status.Finish;
        }

        return Status.Normal;
    }
    private bool CheckFinish() {

        bool allActive = true;

        IteratePieces((piece) => {
            if (piece.Type == PieceType.FinishPiece) {
                if (!((FinishPiece)piece).Active) {
                    allActive = false;
                }
            }
        });

        Logger.Log("all active: " + allActive);

        return allActive;
    }

    private bool IncorrectPiecePosition(GamePiece piece) {
        //if (piece.X < 0 || piece.X >= Field.GetLength(0)) {
        //    return true;
        //}
        //if (piece.Y < 0 || piece.Y >= Field.GetLength(1)) {
        //    return true;
        //}

        int playBlockCount = 0;

        foreach (GamePiece p in Field[piece.X, piece.Y]) {
            if (p.Type == PieceType.PlayPiece) {
                playBlockCount++;
            }
        }

        if (playBlockCount == 1) {
            return true;
        }

        return false;
    }

    private static bool IncorrectPanel(LinkedList<GamePiece> panel) {
        int playBlockCount = 0;

        foreach (GamePiece p in panel) {
            if (p.Type == PieceType.PlayPiece) {
                playBlockCount++;
            }
        }

        if (playBlockCount > 1) {
            return true;
        }

        return false;
    }

    public object Clone() {

        int width = Field.GetLength(0);
        int height = Field.GetLength(1);

        LevelState newState = new LevelState(width, height);

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {

                foreach (GamePiece piece in Field[x, y]) {
                    newState.Field[x, y].AddLast((GamePiece)piece.Clone());
                }
            }
        }

        return newState;
    }
    #endregion
    #region helpers
    public delegate void PieceIterate(GamePiece piece);
    public void IteratePieces(PieceIterate func) {
        foreach (var pieces in Field) {
            foreach (GamePiece piece in pieces) {
                func?.Invoke(piece);
            }
        }
    }
    #endregion
}
