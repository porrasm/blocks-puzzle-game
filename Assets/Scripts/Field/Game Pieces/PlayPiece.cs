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

    public override void UpdateMove(Level.MoveState state) {

        int width = state.State.Field.GetLength(0);
        int height = state.State.Field.GetLength(1);

        if (state.Move == Move.Up && Y + 1 < height) {
            Y++;
        } else if (state.Move == Move.Down && Y - 1 >= 0) {
            Y--;
        } else if (state.Move == Move.Right && X + 1 < width) {
            X++;
        } else if (state.Move == Move.Left && X - 1 >= 0) {
            X--;
        }

        return;
        switch (state.Move) {
            case Move.Up:
                Y++;
                break;
            case Move.Down:
                Y--;
                break;
            case Move.Right:
                X++;
                break;
            case Move.Left:
                X--;
                break;
        }
    }

    public override bool Equals(object obj) {
        if (obj.GetType() != GetType()) {
            return false;
        }

        PlayPiece other = (PlayPiece)obj;

        return other.ID == ID;
    }
}
