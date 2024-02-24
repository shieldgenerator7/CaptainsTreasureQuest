using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(MovePattern))]
public class MovePatternEditor : Editor
{
    Vector2 toggleSize = new Vector2(10, 10);
    int buffer = 15;
    Vector2 start = new Vector2(0, 10);
    int maxRange = 8;
    //const int MAX_RANGE = 8;

    public override VisualElement CreateInspectorGUI()
    {
        MovePattern mp = (MovePattern)target;
        int mprange = mp.maxRange;
        maxRange = (mprange > 0) ? mprange : maxRange;
        return base.CreateInspectorGUI();
    }

    public override void OnInspectorGUI()
    {

        MovePattern mp = (MovePattern)target;

        maxRange = (int)GUILayout.HorizontalSlider(maxRange, 0, 8);

        Vector2 start = this.start + Vector2.one * (maxRange + 1) * buffer;
        for (int x = -maxRange; x <= maxRange; x++)
        {
            for (int y = -maxRange; y <= maxRange; y++)
            {
                bool prevcheck = mp.getMove(x, y);
                Vector2 uiMove = new Vector2(x, -y);

                bool newcheck = GUI.Toggle(
                    new Rect(uiMove * buffer + start, toggleSize),
                    prevcheck,
                    ""
                    );
                if (newcheck != prevcheck)
                {
                    mp.setMove(x, y, newcheck);
                    EditorUtility.SetDirty(target);
                }
            }
        }

        GUILayout.Space(start.y + (maxRange + 2) * buffer);

        //DrawDefaultInspector();
    }
}
