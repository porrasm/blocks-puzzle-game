using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePiece : ICloneable {

    #region fields
    private static int id;

    public int ID { get; protected set; }
    public PieceType Type { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool Enabled { get; set; } = true;
    #endregion

    public GamePiece() {
        ID = id;
        id++;
    }

    public abstract object Clone();
    public virtual void UpdateMove(Level.MoveState state) {

    }
    public virtual void OnFixState(Level.MoveState state) {

    }
}