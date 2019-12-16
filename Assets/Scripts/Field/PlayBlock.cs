using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBlock : FieldPiece {

    public BlockColor Color { get; private set; }

    public override GamePiece GetInitialPiece() {
       return new PlayPiece();
    }

    private void Update() {
        UpdateFieldState();
    }
}
