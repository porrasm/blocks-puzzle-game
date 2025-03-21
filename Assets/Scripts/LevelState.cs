﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState : ICloneable {

    #region fields
    public List<GamePiece>[,] Field { get; private set; }
    public LevelStatus Status { get; private set; }
    public Move Move { get; private set; }
    public int PieceCount { get; private set; }
    #endregion


    public LevelState(int width, int height) {
        Field = new List<GamePiece>[width, height];
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                Field[i, j] = new List<GamePiece>();
            }
        }
    }

    #region state modification
    public static LevelStatus FixState(LevelState moveState) {

        int width = moveState.Field.GetLength(0);
        int height = moveState.Field.GetLength(1);

        List<GamePiece>[,] newField = new List<GamePiece>[width, height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                newField[x, y] = new List<GamePiece>();
            }
        }

        foreach (var pieces in moveState.Field) {
            foreach (GamePiece piece in pieces) {
                //if (moveState.State.IncorrectPiecePosition(piece)) {
                //    return Status.Invalid;
                //}
                newField[piece.X, piece.Y].Add(piece);
            }
        }

        moveState.Field = null;
        moveState.Field = newField;

        foreach (var panel in moveState.Field) {
            if (IncorrectPanel(panel)) {
                return LevelStatus.Invalid;
            }
        }

        moveState.IteratePieces((piece) => {
            piece.OnFixState(moveState);
        });

        if (moveState.CheckFinish()) {
            return LevelStatus.Finish;
        }

        return LevelStatus.Normal;
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

        return allActive;
    }

    public bool CellHasPiece(int x, int y, PieceType type) {

        foreach (GamePiece piece in Field[x, y]) {
            if (piece.Type == type) {
                return true;
            }
        }

        return false;
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

    private static bool IncorrectPanel(List<GamePiece> panel) {
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

    public LevelState CopyState(Move newMove) {
        LevelState newState = (LevelState)this.Clone();
        newState.Move = newMove;
        return newState;
    }


    #endregion
    #region helpers
    public object Clone() {

        int width = Field.GetLength(0);
        int height = Field.GetLength(1);

        LevelState newState = new LevelState(width, height);

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {

                foreach (GamePiece piece in Field[x, y]) {
                    newState.Field[x, y].Add((GamePiece)piece.Clone());
                }
            }
        }

        newState.PieceCount = PieceCount;

        return newState;
    }

    public static LevelState ExecuteMove(Move move, LevelState oldState) {

        LevelState newState = oldState.CopyState(move);

        foreach (var pieces in newState.Field) {
            foreach (GamePiece piece in pieces) {
                piece.UpdateMove(newState);
            }
        }

        newState.Status = LevelState.FixState(newState);

        return newState;
    }

    public void AddPiece(GamePiece piece) {
        PieceCount++;
        Field[piece.X, piece.Y].Add(piece);
        Logger.Log("piece count: " + PieceCount);
    }

    public GamePiece[] Pieces() {
        GamePiece[] pieces = new GamePiece[PieceCount];
        int index = 0;

        IteratePieces((piece) => {
            pieces[index] = piece;
            index++;
        });
        return pieces;
    }

    public delegate void PieceIterate(GamePiece piece);
    public void IteratePieces(PieceIterate func) {
        foreach (var pieces in Field) {
            foreach (GamePiece piece in pieces) {
                func?.Invoke(piece);
            }
        }
    }

    public override bool Equals(object obj) {

        if (obj.GetType() != GetType()) {
            return false;
        }

        LevelState other = (LevelState)obj;

        int w = Field.GetLength(0);
        int h = Field.GetLength(1);

        for (int x = 0; x < w; x++) {
            for (int y = 0; y < h; y++) {
                if (Field[x, y].Count != other.Field[x, y].Count) {
                    return false;
                }

                for (int i = 0; i < Field[x, y].Count; i++) {
                    if (!Field[x, y][i].Equals(other.Field[x, y][i])) {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public override int GetHashCode() {
        return 1998067999 + EqualityComparer<List<GamePiece>[,]>.Default.GetHashCode(Field);
    }
    #endregion
}
