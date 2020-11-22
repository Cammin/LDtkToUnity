using System;
using System.Collections.Generic;
using System.Linq;
using LDtkUnity.Runtime.FieldInjection;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

#endif

namespace Samples.Scripts.YourTypical2DPlatformer
{
    public class ExampleMob : MonoBehaviour
    {
        [LDtkField] public RaycastHit[] loot;
        [LDtkField] public Vector2Int[] patrol;

        private void OnDrawGizmos()
        {
            List<Vector3> convertedRoute = Array.ConvertAll(patrol, input => new Vector3(input.x, input.y, 0)).ToList();

            //round the starting position to the bottom left of the current tile
            Vector3 pos = transform.position;
            int x = Mathf.FloorToInt(pos.x);
            int y = Mathf.FloorToInt(pos.y);
            convertedRoute.Insert(0, new Vector3(x, y, 0));

            //add half a unit to the points to look like the level itself
            for (int index = 0; index < convertedRoute.Count; index++)
            {
                convertedRoute[index] += (Vector3)(Vector2.one / 2);
            }

#if UNITY_EDITOR
            Handles.color = Color.red;
            Handles.DrawLines(convertedRoute.ToArray());
            #endif
        }
    }
}