using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[CustomEditor(typeof(MovePattern))]
public class MovePatternEditor : Editor
{
    Vector2 toggleSize = new Vector2(30,30);
    bool check = true;
    int startX = 50;
    int startY = 400;
    //int buffer = 15;
    Vector2 start = new Vector2(0, 200);

    public override void OnInspectorGUI()
    {

        MovePattern mp = (MovePattern)target;

        int range = mp.maxRange;
	int MAX_RANGE = 8;
        Vector2 start = mp.start + Vector2.one * (MAX_RANGE + 1) * mp.buffer;
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                bool prevcheck = mp.getMove(x, y);
                Vector2 uiMove = new Vector2(x, -y);
                //GUIStyle extra = (x == 0 && y == 0) ? new GUIStyle():null;
                //if (extra)
                //{
                //    extra.
                //}
                Vector2 extra = (x == 0 && y == 0) ? Vector2.one * 10 : Vector2.zero;
                Matrix4x4 matrix = GUI.matrix;
                Quaternion quat = matrix.rotation;
                quat.z = 45;
                Vector3 pos = matrix.GetPosition();
                Vector3 scale = matrix.lossyScale;
                //matrix.rotation = quat;
                //matrix.SetTRS(pos, quat, scale);
                //GUI.matrix = matrix;
                bool newcheck = GUI.Toggle(
                    new Rect(uiMove * mp.buffer + start, toggleSize),
                    prevcheck,
                    ""
                    );
                //Debug.Log("check: " + prevcheck + " -> " + newcheck);
                mp.setMove(x, y, newcheck);
                //if (true || prevcheck != newcheck)
                //{
                //}
            }
        }
        //if (GUILayout.Button("hello"))
        //{
        //    Debug.Log("button pressed in ui");
        //}
        //GUIView.current.MarkHotRegion(checkRect);

        //GUILayout.BeginArea(new Rect(0, 0, 500, 500)); //(mp.start.x-range*2+1)buffer))
        //GUILayout.be
        //int rows = 8;
        //for (int i = 0; i < rows; i++)
        //{
        //    check = GUILayout.Toggle(check, "test row "+i);
        //}

        GUILayout.Space(start.y + (MAX_RANGE + 2) * mp.buffer);
        //GUIArea area = new GUIArea();
        //check = GUI.Toggle(
        //    new Rect(10, 10, 100, 30),//toggleSize.x, toggleSize.y)//,
        //    check,
        //    "test "
        //    );
        //check = GUI.Toggle(
        //    new Rect(mp.start.x, mp.start.y, 100, 30),//toggleSize.x, toggleSize.y)//,
        //    check,
        //    "test 2"
        //    );
        //GUILayout.EndArea();
        //Debug.Log("Check value: " + check);

        DrawDefaultInspector();
    }
}
