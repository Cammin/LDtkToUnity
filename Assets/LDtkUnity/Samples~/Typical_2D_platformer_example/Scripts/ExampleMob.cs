using System;
using System.Collections.Generic;
using System.Linq;
using LDtkUnity.Enums;
using LDtkUnity.FieldInjection;
using UnityEditor;
using UnityEngine;

namespace Samples.Typical_2D_platformer_example
{
    public class ExampleMob : MonoBehaviour
    {
        [SerializeField] private ExamplePointDrawer _drawer;
        
        [LDtkField] public Item[] loot;
        [LDtkField] public Vector2Int[] patrol;

        private void Start()
        {
            _drawer.SetPoints(patrol);
        }
    }
}