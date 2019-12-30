using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPiece : GamePiece {

    #region fields
    public Colors.BlockColor Color { get; set; }
    public bool Active { get; private set; }
    #endregion

    public FinishPiece(bool unique) : base(unique) {
        Type = PieceType.FinishPiece;
    }


    public override object Clone() {
        FinishPiece piece = new FinishPiece(false);
        piece.Type = Type;
        piece.X = X;
        piece.Y = Y;
        piece.Enabled = Enabled;
        piece.ID = ID;
        piece.Color = Color;

        return piece;
    }

    public override void OnFixState(LevelState state) {
        base.UpdateMove(state);

        foreach (GamePiece piece in state.Field[X, Y]) {
            if (piece.Type == PieceType.PlayPiece) {
                PlayPiece playPiece = (PlayPiece)piece;
                if (playPiece.Color == Color) {
                    Active = true;
                    return;
                }
            }
        }

        Active = false;
    }
}