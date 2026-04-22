using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileSlot)), CanEditMultipleObjects]

public class TileSlotEditor : Editor
{
    private GUIStyle _centeredStyle;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        
        _centeredStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 16,
            margin = new RectOffset(0, 0, 5, 5),
        };
        float oneButtonWidth = (EditorGUIUtility.currentViewWidth - 22);
        float twoButtonWidth = (EditorGUIUtility.currentViewWidth - 25) / 2;
        float threeButtonWidth = (EditorGUIUtility.currentViewWidth - 25) / 3;
        
        
        GUILayout.Label("Position and Rotation", _centeredStyle);
        
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Rotate Left", GUILayout.Width(twoButtonWidth)))
        {
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).RotateTile(-1);
            }
        }
        
        if (GUILayout.Button("Rotate Right", GUILayout.Width(twoButtonWidth)))
        {

            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).RotateTile(1);
            }
        }
        
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("- .1f on the Y", GUILayout.Width(twoButtonWidth)))
        {
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).AdjustY(-1);
            }
        }
        
        if (GUILayout.Button("+ .1f on the Y", GUILayout.Width(twoButtonWidth)))
        {

            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).AdjustY(1);
            }
        }
        
        GUILayout.EndHorizontal();
        
        GUILayout.Label("Tile Options", _centeredStyle);
        
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Field", GUILayout.Width(twoButtonWidth)))
        {
            GameObject newTile = FindAnyObjectByType<TileSetHolder>().tileField;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }
        
        if (GUILayout.Button("Road", GUILayout.Width(twoButtonWidth)))
        {
            GameObject newTile = FindAnyObjectByType<TileSetHolder>().tileRoad;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }
        
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Sideway", GUILayout.Width(oneButtonWidth)))
        {
            GameObject newTile = FindAnyObjectByType<TileSetHolder>().tileSideway;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }
        
        GUILayout.EndHorizontal();
        
        GUILayout.Label("Corner Options", _centeredStyle);
        
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Inner corner", GUILayout.Width(twoButtonWidth)))
        {
            GameObject newTile = FindAnyObjectByType<TileSetHolder>().tileInnerCorner;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }
        
        if (GUILayout.Button("Outer corner", GUILayout.Width(twoButtonWidth)))
        {
            GameObject newTile = FindAnyObjectByType<TileSetHolder>().tileOuterCorner;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }
        
        GUILayout.EndHorizontal();
        
        GUILayout.Label("Bridges and Hills", _centeredStyle);
        
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Hill 1", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindAnyObjectByType<TileSetHolder>().tileHill_1;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }
        
        if (GUILayout.Button("Hill 2", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindAnyObjectByType<TileSetHolder>().tileHill_2;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }
        
        if (GUILayout.Button("Hill 3", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindAnyObjectByType<TileSetHolder>().tileHill_3;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }
        
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Bridge Field", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindAnyObjectByType<TileSetHolder>().tileBridgeField;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }
        
        if (GUILayout.Button("Bridge Road", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindAnyObjectByType<TileSetHolder>().tileBridgeRoad;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }
        
        if (GUILayout.Button("Bridge Sideway", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindAnyObjectByType<TileSetHolder>().tileBridgeSideway;
            foreach (var targetTile in targets)
            {
                ((TileSlot)targetTile).SwitchTile(newTile);
            }
        }
        
        GUILayout.EndHorizontal();
    }
}
