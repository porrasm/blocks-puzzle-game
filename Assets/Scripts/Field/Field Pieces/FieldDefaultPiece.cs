using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldDefaultPiece : FieldPiece {

    #region fields

    #endregion
    public override GamePiece GetInitialPiece() {
        DefaultPiece p = new DefaultPiece();
        return p;
    }
}