using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePiece : ICloneable {

    #region fields
    private static int idIndex;

    public int ID { get; protected set; }
    public PieceType Type { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool Enabled { get; set; } = true;
    #endregion

    public GamePiece() {
        ID = idIndex;
        idIndex++;
    }

    public abstract object Clone();
    public virtual void UpdateMove(LevelState state) {

    }
    public virtual void OnFixState(LevelState state) {

    }

    public override bool Equals(object obj) {

        GamePiece other = obj as GamePiece;

        if (other == null) {
            return false;
        }

        return ID == other.ID;
    }

    public override int GetHashCode() {
        return 1213502048 + ID.GetHashCode();
    }
}