using System;
using LDtkUnity.Enums;
using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.Entities
{
    public class ExampleEnemy : MonoBehaviour
    {
        [SerializeField] private ExamplePointDrawer _drawer;
        
        [LDtkField("type")] public MonsterType _type;
        [LDtkField("hp")] public int _hp;
        [LDtkField("loots")] public ItemType[] _loots;
        [LDtkField("patrol")] public Vector2Int[] _patrol;

        private void Start()
        {
            _drawer.SetPoints(_patrol);
        }
    }
}
