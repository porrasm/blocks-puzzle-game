using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPiece : GamePiece {

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
        Logger.Log("Copied id: " + this.ID + ", to piece: " + piece.ID);
        return piece;
    }

    public override void ExecuteMove(Move move) {
        switch (move) {
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
}
