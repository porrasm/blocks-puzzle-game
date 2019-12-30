using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePiece : ICloneable {

    #region fields
    private static int idIndex;
    private static int UniqueID {
        get {
            int i = idIndex;
            idIndex++;
            return i;
        }
    }

    public int ID { get; protected set; }
    public PieceType Type { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool Enabled { get; set; } = true;
    public Vector2 PositionVector {
        get {
            return new Vector2(X, Y);
        }
    }
    #endregion

    public GamePiece(bool unique) {
        if (unique) {
            ID = UniqueID;
            Logger.Log("set Unique id: " + ID);
        }
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