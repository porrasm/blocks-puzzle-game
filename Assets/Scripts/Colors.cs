using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Colors {

    #region fields

    #endregion

    public enum BlockColor {
        Red,
        Green
    }

    public static Color32 GetBlockColor(BlockColor color) {
        switch (color) {
            case BlockColor.Red:
                return new Color32(221, 68, 68, 255);
            case BlockColor.Green:
                return new Color32(89, 183, 89, 255);
        }

        return Color.black;
    }
    public static Color32 GetFinishColor(BlockColor color) {
        switch (color) {
            case BlockColor.Red:
                return new Color32(210, 116, 116, 255);
            case BlockColor.Green:
                return new Color32(128, 195, 128, 255);
        }

        return Color.black;
    }
}