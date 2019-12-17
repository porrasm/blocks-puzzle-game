using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FieldPiece : MonoBehaviour {

    private float distance = 0.001f;

    private static float lerpSpeed = 10;

    public int PieceID { get; set; }
    public GamePiece Piece {
        get {
            return Level.Current.GetPiece(PieceID);
        }
    }
    public void UpdateFieldState() {

        if (DistanceToTarget() < distance) {
            transform.localPosition = PiecePos;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, PiecePos, Time.deltaTime * lerpSpeed);
    }

    private Vector2 PiecePos {
        get {
            return new Vector2(Piece.X, Piece.Y);
        }
    }
    private float DistanceToTarget() {
        return Vector2.Distance(transform.localPosition, PiecePos);
    }

    //Testing only
    public abstract GamePiece GetInitialPiece();
}
