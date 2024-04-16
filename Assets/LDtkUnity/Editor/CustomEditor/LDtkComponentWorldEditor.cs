using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkComponentWorld), true)]
    [CanEditMultipleObjects]
    internal sealed class LDtkComponentWorldEditor : UnityEditor.Editor
    {
        private GUIContent _contentButton;
        private LDtkComponentWorld[] _inspectedWorlds;
        private LDtkComponentWorld[] _validWorlds;
        
        private void OnEnable()
        {
            _contentButton = new GUIContent()
            {
                text = "Spawn all Levels",
                tooltip = "Instantiate all levels for this world",
                image = LDtkIconUtility.LoadLevelFileIcon()
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            TryDrawSpawnLevelsButton(); 
            serializedObject.ApplyModifiedProperties();
        }

        private void TryDrawSpawnLevelsButton()
        {
            RebuildInspectedWorlds();

            if (!_validWorlds.IsNullOrEmpty())
            {
                DrawSpawnLevelsButton();
            }
        }

        private void RebuildInspectedWorlds()
        {
            _inspectedWorlds = serializedObject.targetObjects.OfType<LDtkComponentWorld>().ToArray();
            _validWorlds = _inspectedWorlds.Where(p => p.Parent && p.Parent.ExternalLevels).ToArray();
        }

        private void DrawSpawnLevelsButton()
        {
            using (new GUILayout.HorizontalScope())
            {
                using (new EditorGUIUtility.IconSizeScope(Vector2.one * 16))
                {
                    if (GUILayout.Button(_contentButton))
                    {
                        SpawnLevelsInWorlds();
                    }
                }
                GUILayout.FlexibleSpace();
            }
        }

        private void SpawnLevelsInWorlds()
        {
            if (HasAnyWithHierarchy())
            {
                string msg = serializedObject.isEditingMultipleObjects ? 
                    "One or more worlds has children, which will be destroyed and replaced by the new levels.\n\nConfirm?" : 
                    "The selected world has children, which will be destroyed and replaced by the new levels.\n\nConfirm?"; 
                if (!EditorUtility.DisplayDialog("Spawn Levels", msg, "Yes", "Cancel"))
                {
                    return;
                }
            }
            
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Spawn Levels In Worlds");
            
            foreach (LDtkComponentWorld world in _validWorlds)
            {
                SpawnLevelsInWorld(world);
            }
            
            Undo.IncrementCurrentGroup();
        }
        
        private bool HasAnyWithHierarchy()
        {
            if (_validWorlds.IsNullOrEmpty())
            {
                return false;
            }
            
            foreach (LDtkComponentWorld world in _validWorlds)
            {
                if (world.transform.childCount > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void SpawnLevelsInWorld(LDtkComponentWorld worldComponent)
        {
            //load project of world to spawn the appropriate levels
            string projectPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(worldComponent);
            if (projectPath.IsNullOrEmpty())
            {
                LDtkDebug.LogError($"This world \"{worldComponent.Identifier}\" could not find it's source project. Was this prefab unpacked or disconnected from it's source project?");
                return;
            }
            
            LDtkProjectFile projectFile = AssetDatabase.LoadAssetAtPath<LDtkProjectFile>(projectPath);
            if (projectFile == null)
            {
                LDtkDebug.LogError($"This world \"{worldComponent.Identifier}\" could not load it's source project. Is this project imported properly?");
                return;
            }
            
            LdtkJson json = projectFile.FromJson;
            
            World world = json.UnityWorlds.FirstOrDefault(p => p.Identifier == worldComponent.Identifier);
            if (world == null)
            {
                LDtkDebug.LogError($"Couldn't locate the world \"{worldComponent.Identifier}\" in the json to spawn levels from");
                return;
            }
            
            //destroy hierarchy before adding new things
            for (int i = worldComponent.transform.childCount - 1; i >= 0; i--)
            {
                GameObject obj = worldComponent.transform.GetChild(i).gameObject;
                if (PrefabUtility.IsAddedGameObjectOverride(obj))
                {
                    Undo.DestroyObjectImmediate(obj);
                }
            }
            
            //spawn all level objects
            foreach (Level level in world.Levels)
            {
                GameObject levelPrefab = new LDtkRelativeGetterLevelPrefab().GetRelativeAsset(level, projectPath);
                if (levelPrefab == null)
                {
                    continue;
                }
                
                Object levelInstance = PrefabUtility.InstantiatePrefab(levelPrefab, worldComponent.transform);
                Undo.RegisterCreatedObjectUndo(levelInstance, "Create level prefab instance");
            }
        }
    }
}