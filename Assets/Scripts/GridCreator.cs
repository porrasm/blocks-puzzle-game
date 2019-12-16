using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour {

    #region fields
    private float lineWidth = 0.05f;

    private int width, height;

    private Vector2 scale;

    private Transform hor, ver;

    [SerializeField]
    private GameObject gridLinePrefab;
    #endregion

    private void Start() {

        NullCheck.Check(gridLinePrefab);

        scale = transform.parent.GetChild(1).localScale;

        SetTransforms();

        DrawGrid();
    }
    private void SetTransforms() {
        hor = new GameObject("Horizontal").transform;
        hor.transform.parent = transform;
        hor.transform.localScale = Vector3.one;
        hor.transform.localPosition = Vector3.zero;
        hor.transform.localEulerAngles = Vector3.zero;

        ver = new GameObject("Vertical").transform;
        ver.transform.parent = transform;
        ver.transform.localScale = Vector3.one;
        ver.transform.localPosition = Vector3.zero;
        ver.transform.localEulerAngles = Vector3.zero;
    }

    #region drawing
    public void DrawGrid() {

        width = Level.Current.Width;
        height = Level.Current.Height;

        ClearGrid();
        DrawLines();
    }

    private void ClearGrid() {
        foreach (Transform child in hor) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in ver) {
            Destroy(child.gameObject);
        }
    }

    private void DrawLines() {

        hor.transform.localScale = Vector3.one;
        ver.transform.localScale = Vector3.one;

        Vector3 horScale = new Vector3(scale.x + lineWidth, lineWidth, 1);
        Vector3 verScale = new Vector3(lineWidth, scale.y + lineWidth, 1);

        for (int i = 0; i <= height; i++) {
            GameObject newHorLine = Instantiate(gridLinePrefab);
            newHorLine.transform.parent = hor.transform;
            newHorLine.transform.localPosition = new Vector3(0, OffsetHor(i), 0);
            newHorLine.transform.localScale = horScale;
        }
        for (int i = 0; i <= width; i++) {
            GameObject newVerLine = Instantiate(gridLinePrefab);
            newVerLine.transform.parent = ver.transform;
            newVerLine.transform.localPosition = new Vector3(OffsetVer(i), 0, 0);
            newVerLine.transform.localScale = verScale;
        }
    }

    private float OffsetHor(int index) {
        return -scale.y / 2 + (scale.y / height) * index;
    }
    private float OffsetVer(int index) {
        return -scale.x / 2 + (scale.x / width) * index;
    }
    #endregion
}
