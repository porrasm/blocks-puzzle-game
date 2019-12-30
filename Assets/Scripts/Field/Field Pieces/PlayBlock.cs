using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBlock : FieldPiece {

    [SerializeField]
    public Colors.BlockColor Color;

    private void Start() {
        transform.GetChild(0).GetComponent<Renderer>().material.color = Colors.GetBlockColor(Color);
    }

    public override GamePiece GetInitialPiece() {
        var p = new PlayPiece(true);
        p.Color = Color;
        return p;
    }

    private void Update() {
        UpdateFieldState();
    }
}
