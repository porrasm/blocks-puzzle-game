using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LevelState : ICloneable {
    public LinkedList<GamePiece>[,] Field { get; private set; }

    public LevelState(int width, int height) {
        Field = new LinkedList<GamePiece>[width, height];
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                Field[i, j] = new LinkedList<GamePiece>();
            }
        }
    }

    #region state modification
    public bool FixState() {

        int width = Field.GetLength(0);
        int height = Field.GetLength(1);

        LinkedList<GamePiece>[,] newField = new LinkedList<GamePiece>[width, height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                newField[x, y] = new LinkedList<GamePiece>();
            }
        }

        foreach (var pieces in Field) {
            foreach (GamePiece piece in pieces) {
                if (IncorrectPiecePosition(piece)) {
                    return false;
                }
                newField[piece.X, piece.Y].AddLast(piece);
            }
        }

        Field = null;
        Field = newField;

        return true;
    }
    private bool IncorrectPiecePosition(GamePiece piece) {
        if (piece.X < 0 || piece.X >= Field.GetLength(0)) {
            return true;
        }
        if (piece.Y < 0 || piece.Y >= Field.GetLength(1)) {
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
}
