using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FieldPiece : MonoBehaviour {
    public int PieceID { get; set; }
    public GamePiece Piece {
        get {
            return Level.Current.GetPiece(PieceID);
        }
    }
    public void UpdateFieldState() {
        transform.localPosition = new Vector2(Piece.X, Piece.Y);
    }

    //Testing only
    public abstract GamePiece GetInitialPiece();
}
