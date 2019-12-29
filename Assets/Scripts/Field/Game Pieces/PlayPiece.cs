using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPiece : GamePiece {

    public Colors.BlockColor Color { get; set; }

    public PlayPiece() {
        Type = PieceType.PlayPiece;
    }

    public override object Clone() {
        PlayPiece piece = new PlayPiece();
        piece.Type = Type;
        piece.X = X;
        piece.Y = Y;
        piece.Enabled = Enabled;
        piece.ID = ID;
        piece.Color = Color;

        return piece;
    }

    public override void UpdateMove(LevelState state) {
        if (state.Move == Move.Up) {
            MovePiece(X, Y + 1, state);
        } else if (state.Move == Move.Down) {
            MovePiece(X, Y - 1, state);
        } else if (state.Move == Move.Right) {
            MovePiece(X + 1, Y, state);
        } else if (state.Move == Move.Left) {
            MovePiece(X - 1, Y, state);
        }
    }

    private void MovePiece(int x, int y, LevelState state) {

        var field = state.Field;

        if (InvalidIndex(x, y, field.GetLength(0), field.GetLength(1))) {
            Logger.Log("Retrun, index out of bounds");
            return;
        }

        if (state.CellHasPiece(x, y, PieceType.DefaultPiece)) {
            Logger.Log("Retrun, field has piece default peice");
            Logger.Log("in " + x + ", " + y);
            return;
        }

        X = x;
        Y = y;
    }
    private bool InvalidIndex(int x, int y, int widht, int height) {
        return x < 0 || x >= widht || y < 0 || y >= height;
    }

    public override bool Equals(object obj) {
        if (obj.GetType() != GetType()) {
            return false;
        }

        PlayPiece other = (PlayPiece)obj;

        return other.ID == ID;
    }
}
