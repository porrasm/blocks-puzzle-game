using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFinish : FieldPiece {

    #region fields
    [SerializeField]
    public Colors.BlockColor Color;
    #endregion

    private void Start() {
        transform.GetChild(0).GetComponent<Renderer>().material.color = Colors.GetFinishColor(Color);
    }

    public override GamePiece GetInitialPiece() {
        var p = new FinishPiece();
        p.Color = Color;
        return p;
    }

}
