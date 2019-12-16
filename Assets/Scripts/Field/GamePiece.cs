using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePiece : ICloneable {

    private static int id;
    public int ID { get; protected set; }
    public PieceType Type { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool Enabled { get; set; } = true;

    public GamePiece() {
        ID = id;
        id++;
        Logger.Log("Created new piece instance");
    }

    public abstract object Clone();
    public virtual void ExecuteMove(Move move) {

    }
}