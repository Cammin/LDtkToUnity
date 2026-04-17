using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomPropertyDrawer(typeof(LDtkIntPattern))]
    public class LDtkIntPatternDrawer : PropertyDrawer
    {
        // Grid & layout
        private const int GridSize = 9;
        private const float BoxPad = 4f;
        private const float LegendRow = 22f;
        private const float CellDefault = 22f;
        private const float PadDefault = 2f;

        // Special values
        private const int XValue = -1000001; // red + white X, no input
        private const int QValue = 1000001; // white + black ?, no input

        // Colors
        private static readonly Color ZeroFill = new(0.18f, 0.18f, 0.18f, 1f);
        private static readonly Color ZeroBorder = new(0.82f, 0.82f, 0.82f, 1f);
        private static readonly Color CrossRed = new(0.85f, 0.15f, 0.15f, 1f);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var dataProp = property.FindPropertyRelative("data");

            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.IndentedRect(position);

            var header = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(header, label, EditorStyles.boldLabel);

            if (dataProp is null || !dataProp.isArray || dataProp.propertyType == SerializedPropertyType.String)
            {
                DrawHelpBox(position, "IntPattern expects a serialized int[] named 'data'.", MessageType.Warning);
                EditorGUI.EndProperty();
                return;
            }

            int length = dataProp.arraySize;
            if (length == 0)
            {
                DrawHelpBox(position, "Array is empty.", MessageType.Info);
                EditorGUI.EndProperty();
                return;
            }

            int dataN = Mathf.Clamp(Mathf.RoundToInt(Mathf.Sqrt(length)), 1, GridSize);
            bool isSquare = dataN * dataN == length;
            float y = header.yMax + EditorGUIUtility.standardVerticalSpacing;

            var legend = new Rect(position.x, y, position.width, LegendRow);
            DrawLegend(legend, CollectLegendValues(dataProp, dataN));
            y = legend.yMax + EditorGUIUtility.standardVerticalSpacing;

            if (!isSquare)
            {
                var warn = new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight * 2f);
                EditorGUI.HelpBox(warn,
                    $"Length {length} is not a perfect square. Rendering {dataN}×{dataN} centered in a {GridSize}×{GridSize} grid.",
                    MessageType.Info);
                y = warn.yMax + EditorGUIUtility.standardVerticalSpacing;
            }

            float gridHeight = GridSize * (CellDefault + PadDefault) + PadDefault + BoxPad * 2f;
            var box = new Rect(position.x, y, position.width, gridHeight);
            GUI.Box(box, GUIContent.none);

            var inner = new Rect(box.x + BoxPad, box.y + BoxPad, box.width - BoxPad * 2f, box.height - BoxPad * 2f);

            float cell = CellDefault;
            float cellFull = cell + PadDefault;
            if (GridSize * cellFull + PadDefault > inner.width)
            {
                cell = Mathf.Max(14f, (inner.width - PadDefault) / GridSize - PadDefault);
                cellFull = cell + PadDefault;
            }

            int rowOffset = (GridSize - dataN) / 2;
            int colOffset = (GridSize - dataN) / 2;
            int centerIdxFull = (GridSize / 2) * GridSize + (GridSize / 2);

            var textCentered = new GUIStyle(EditorStyles.miniBoldLabel) { alignment = TextAnchor.MiddleCenter };

            var so = property.serializedObject;
            so.Update();
            EditorGUI.BeginChangeCheck();

            for (int r = 0; r < GridSize; r++)
            {
                for (int c = 0; c < GridSize; c++)
                {
                    var cellRect = new Rect(
                        inner.x + c * cellFull + PadDefault,
                        inner.y + r * cellFull + PadDefault,
                        cell, cell);

                    bool inData = r >= rowOffset && r < rowOffset + dataN &&
                                  c >= colOffset && c < colOffset + dataN;

                    if (!inData)
                    {
                        DrawZeroTile(cellRect);
                        if (r * GridSize + c == centerIdxFull) Highlight(cellRect);
                        continue;
                    }

                    int localR = r - rowOffset;
                    int localC = c - colOffset;
                    int dataIdx = localR * dataN + localC;

                    if (dataIdx >= length)
                    {
                        DrawZeroTile(cellRect);
                        continue;
                    }

                    var elem = dataProp.GetArrayElementAtIndex(dataIdx);
                    int v = elem.propertyType == SerializedPropertyType.Integer ? elem.intValue : 0;

                    DrawDataTile(cellRect, v, textCentered, out bool editable);
                    if (editable)
                    {
                        int newVal = EditorGUI.DelayedIntField(cellRect, GUIContent.none, v, textCentered);
                        if (newVal != v) elem.intValue = newVal;
                        if (v < 0) DrawRedX(cellRect, 2f);
                    }

                    DrawBorder(cellRect, v == 0);
                    if (r * GridSize + c == centerIdxFull) Highlight(cellRect);
                }
            }

            if (EditorGUI.EndChangeCheck())
                so.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var dataProp = property.FindPropertyRelative("data");

            float h = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (dataProp is { isArray: true } && dataProp.propertyType != SerializedPropertyType.String &&
                dataProp.arraySize > 0)
            {
                h += LegendRow + EditorGUIUtility.standardVerticalSpacing;
                h += GridSize * (CellDefault + PadDefault) + PadDefault + BoxPad * 2f;
            }
            else
            {
                h += EditorGUIUtility.singleLineHeight * 2f;
            }

            return h;
        }

        private static List<int> CollectLegendValues(SerializedProperty dataProp, int dataN)
        {
            var set = new HashSet<int>();
            int count = Mathf.Min(dataProp.arraySize, dataN * dataN);

            for (int i = 0; i < count; i++)
            {
                var el = dataProp.GetArrayElementAtIndex(i);
                if (el.propertyType != SerializedPropertyType.Integer) continue;

                int v = el.intValue;
                if (v <= 0 || v == QValue) continue; // exclude 0, negatives, and ?
                set.Add(v);
            }

            var list = new List<int>(set);
            list.Sort();
            return list;
        }

        private static void DrawLegend(Rect rect, List<int> values)
        {
            float sw = rect.height - 6f;
            float x = rect.x;
            var style = EditorStyles.miniLabel;

            foreach (int v in values)
            {
                if (x + 80f > rect.xMax) break;

                var swRect = new Rect(x, rect.y + 3, sw, sw);
                var txtRect = new Rect(swRect.xMax + 4, rect.y + 2, 54, rect.height - 2);

                EditorGUI.DrawRect(swRect, ColorForNumber(v));
                EditorGUI.LabelField(txtRect, v.ToString(), style);

                x = txtRect.xMax + 10;
            }

            if (values.Count == 0) EditorGUI.LabelField(rect, "Legend: (none)");
        }

        private static void DrawDataTile(Rect r, int v, GUIStyle centered, out bool editable)
        {
            editable = false;

            if (v == 0)
            {
                DrawZeroTile(r);
                return;
            }

            if (v == XValue)
            {
                EditorGUI.DrawRect(r, CrossRed);
                DrawWhiteX(r, 2f);
                return;
            }

            if (v == QValue)
            {
                EditorGUI.DrawRect(r, Color.white);
                DrawQuestionMark(r);
                return;
            }

            int colorKey = v < 0 ? -v : v;
            EditorGUI.DrawRect(r, ColorForNumber(colorKey));
            editable = true;
        }

        private static void DrawZeroTile(Rect r)
        {
            EditorGUI.DrawRect(r, ZeroFill);
            Handles.color = ZeroBorder;
            DrawRectBorder(r, 1.6f);
        }

        private static void DrawBorder(Rect r, bool isZero)
        {
            Handles.color = isZero
                ? ZeroBorder
                : (EditorGUIUtility.isProSkin ? new Color(1, 1, 1, 0.15f) : new Color(0, 0, 0, 0.2f));
            DrawRectBorder(r, isZero ? 1.6f : 1.2f);
        }

        private static void Highlight(Rect r)
        {
            var inset = 1f;
            var hi = new Rect(r.x + inset, r.y + inset, r.width - 2 * inset, r.height - 2 * inset);
            Handles.color = EditorGUIUtility.isProSkin
                ? new Color(1f, 1f, 0.3f, 0.9f)
                : new Color(0.2f, 0.2f, 0f, 0.9f);
            DrawRectBorder(hi, 2f);
        }

        private static void DrawRectBorder(Rect r, float thickness)
        {
            Handles.DrawAAPolyLine(thickness,
                new Vector3(r.xMin, r.yMin), new Vector3(r.xMax, r.yMin),
                new Vector3(r.xMax, r.yMax), new Vector3(r.xMin, r.yMax),
                new Vector3(r.xMin, r.yMin));
        }

        private static void DrawWhiteX(Rect r, float thickness)
        {
            float m = Mathf.Min(r.width, r.height) * 0.18f;
            Vector3 a = new(r.xMin + m, r.yMin + m);
            Vector3 b = new(r.xMax - m, r.yMax - m);
            Vector3 c = new(r.xMin + m, r.yMax - m);
            Vector3 d = new(r.xMax - m, r.yMin + m);

            Handles.color = Color.white;
            Handles.DrawAAPolyLine(thickness, a, b);
            Handles.DrawAAPolyLine(thickness, c, d);
        }

        private static void DrawRedX(Rect r, float thickness)
        {
            float m = Mathf.Min(r.width, r.height) * 0.18f;
            Vector3 a = new(r.xMin + m, r.yMin + m);
            Vector3 b = new(r.xMax - m, r.yMax - m);
            Vector3 c = new(r.xMin + m, r.yMax - m);
            Vector3 d = new(r.xMax - m, r.yMin + m);

            Handles.color = CrossRed;
            Handles.DrawAAPolyLine(thickness, a, b);
            Handles.DrawAAPolyLine(thickness, c, d);
        }

        private static void DrawQuestionMark(Rect r)
        {
            var style = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.black },
                fontSize = Mathf.Max(10, (int)(r.height * 0.6f))
            };
            GUI.Label(r, "?", style);
        }

        private static Color ColorForNumber(int v)
        {
            float hue = Mathf.Abs(Mathf.Repeat(v * 0.61803398875f, 1f));
            var col = Color.HSVToRGB(hue, 0.6f, 0.95f);
            col.a = 1f;
            return col;
        }

        private static void DrawHelpBox(Rect position, string msg, MessageType type)
        {
            var r = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2f, position.width, 38f);
            EditorGUI.HelpBox(r, msg, type);
        }
    }
}