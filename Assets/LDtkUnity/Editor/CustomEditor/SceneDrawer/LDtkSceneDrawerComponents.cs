using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public abstract class LDtkSceneDrawerComponents
    {
        public abstract void Draw();

        protected static bool CanDrawItem(Component p)
        {
            if (p == null)
            {
                return false;
            }

            if (!p.gameObject.activeInHierarchy)
            {
                return false;
            }

            SceneVisibilityManager vis = SceneVisibilityManager.instance;
            if (vis.IsPickingDisabled(p.transform.gameObject))
            {
                return false;
            }
            
            return true;
        }
    }
}