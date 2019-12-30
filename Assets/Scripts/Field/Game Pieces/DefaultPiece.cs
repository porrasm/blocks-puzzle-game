using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPiece : GamePiece {

    #region fields
    public Colors.BlockColor Color { get; set; }
    public bool Active { get; private set; }
    #endregion

    public DefaultPiece(bool unique) : base(unique) {
        Type = PieceType.DefaultPiece;
    }


    public override object Clone() {
        DefaultPiece piece = new DefaultPiece(false);
        piece.Type = Type;
        piece.X = X;
        piece.Y = Y;
        piece.Enabled = Enabled;
        piece.ID = ID;
        piece.Color = Color;

        return piece;
    }
}