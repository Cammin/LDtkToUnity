using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Reminder: Responsibility is just for drawing the Header content and and other unique functionality. All of the numerous content is handled in the Reference Drawers
    /// </summary>
    internal abstract class LDtkSectionDataDrawer<T> : LDtkSectionDrawer, ILDtkSectionDataDrawer where T : ILDtkIdentifier
    {
        protected abstract string PropertyName { get; }
        
        protected SerializedProperty ArrayProp;

        private LDtkContentDrawer<T>[] _drawers;

        protected LDtkSectionDataDrawer(LDtkImporterEditor editor, SerializedObject serializedObject) : base(editor, serializedObject)
        {
            
        }

        public override void Init()
        {
            base.Init();
            ArrayProp = SerializedObject.FindProperty(PropertyName);
        }
        
        public void Draw(IEnumerable<ILDtkIdentifier> datas)
        {
            DrawInternal(datas.Cast<T>().ToArray());
        }

        private void DrawInternal(T[] datas)
        {
            HasResizedArrayPropThisUpdate = false;
            int newArraySize = GetSizeOfArray(datas);
            
            //don't draw if there is no data for this project relating to this.
            //in this case, we should also clear the array for this if there was previous data that is simply no longer here.
            if (newArraySize == 0 && !SerializedObject.isEditingMultipleObjects)
            {
                TryRestructureArray(datas);
                return;
            }
            
            LDtkEditorGUIUtility.DrawDivider();
            DrawFoldoutArea();
            
            //don't process any data or resize arrays when we have multi-selections; references will break because of how dynamic the arrays can be.
            if (SerializedObject.isEditingMultipleObjects && !SupportsMultipleSelection)
            {
                EditorGUILayout.HelpBox($"Multi-object editing not supported for {GuiText}.", MessageType.None);
                return;
            }
            
            TryRestructureArray(datas);
            
            DrawValues(datas);
        }

        private void DrawValues(T[] datas)
        {
            List<LDtkContentDrawer<T>> drawers = new List<LDtkContentDrawer<T>>();
            GetDrawers(datas, drawers);
            _drawers = drawers.ToArray();
            
            if (CanDrawDropdown())
            {
                DrawDropdownContent();
            }
        }

        //This takes the data, and compares it with what's currently available already to know if it should insert specific new ones, or retain the old ones as needed.
        private void TryRestructureArray(T[] defs)
        {
            //compare names. if there's a new one that didn't exist in the old array, insert it into that element index
            if (ArrayProp == null)
            {
                return;
            }
            
            string[] keepers = GetAssetKeysFromDefs(defs);

            Profiler.BeginSample("ShouldRestructureArray");
            bool shouldRestructureArray = ShouldRestructureArray(keepers);
            Profiler.EndSample();
            
            if (!shouldRestructureArray)
            {
                return;
            }

            RemoveUnusedData(keepers);
            RemoveDupes();
            AddMissingData(keepers);
            BubbleSortArray(keepers);
            
            HasResizedArrayPropThisUpdate = true;
        }
        
        //for any reason. if array sizes are different, if all the orderings don't match.
        private bool ShouldRestructureArray(string[] assetKeys)
        {
            if (assetKeys.Length != ArrayProp.arraySize)
            {
                return true;
            }

            for (int i = 0; i < assetKeys.Length; i++)
            {
                if (assetKeys[i] != GetKeyForArray(i))
                {
                    return true;
                }
            }
            
            return false;
        }
        
        private void RemoveUnusedData(string[] keepers)
        {
            //remove any serialized data that no longer exists from the json data
            for (int i = ArrayProp.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty elementProp = ArrayProp.GetArrayElementAtIndex(i);
                string keyForElement = GetKeyForElement(elementProp);
                if (keepers != null && keepers.Contains(keyForElement))
                {
                    continue;
                }

                //Debug.Log($"Deleting asset at {i} for key {keyForElement}");
                ArrayProp.DeleteArrayElementAtIndex(i);
            }
        }
        private void RemoveDupes()
        {
            HashSet<string> set = new HashSet<string>();
            for (int i = 0; i < ArrayProp.arraySize; i++)
            {
                string key = GetKeyForArray(i);
                if (set.Contains(key))
                {
                    ArrayProp.DeleteArrayElementAtIndex(i);
                    continue;
                }

                set.Add(key);
            }
        }

        private void AddMissingData(string[] newAssetKeys)
        {
            //add json data that didn't exist in the serialized array 
            SerializedProperty[] elements = ArrayProp.GetArrayElements();
            string[] elementKeys = elements.Select(GetKeyForElement).ToArray();

            foreach (string newAssetKey in newAssetKeys)
            {
                if (elementKeys.Contains(newAssetKey))
                {
                    //we previously already have it, don't add one
                    continue;
                }
                
                //insert new one with the unique new key
                ArrayProp.InsertArrayElementAtIndex(0);
                SerializedProperty insertedProp = ArrayProp.GetArrayElementAtIndex(0);
                
                SerializedProperty insertedKeyProp = GetKeyPropForElement(insertedProp);
                insertedKeyProp.stringValue = newAssetKey;

                SerializedProperty insertedValueProp = GetValuePropForElement(insertedProp);
                insertedValueProp.objectReferenceValue = null;

                //Debug.Log($"Inserted new asset at {0} for key {newAssetKey}");
            }
        }
        
        private void BubbleSortArray(string[] assetKeys)
        {
            if (ArrayProp.arraySize != assetKeys.Length)
            {
                LDtkDebug.LogError("LDtk: Didn't bubble sort, array lengths were not equal");
                return;
            }
            BubbleSortInstance(assetKeys);
            //LogOrderings(assetKeys);
        }

        private void LogOrderings(string[] assetKeys)
        {
            List<string> strings = new List<string>();
            for (int i = 0; i < ArrayProp.arraySize; i++)
            {
                string keyForArray = GetKeyForArray(i);
                strings.Add(keyForArray);
            }
            
            string debugString = string.Join(", ", strings);
            string assetKeyss = string.Join(", ", assetKeys);

            string joined = $"SERIALIZED: {debugString}\nLDTK_DATA: {assetKeyss}";
            
            LDtkDebug.Log(joined);
        }

        private void BubbleSortInstance(string[] assetKeys)
        {
            SerializedProperty[] elements = ArrayProp.GetArrayElements();

            for (int i = 0; i < assetKeys.Length; i++)
            {
                //find the serialized property by name with the same name
                string assetKey = assetKeys[i];

                int indexToMove = -1;
                for (int j = 0; j < elements.Length; j++)
                {
                    SerializedProperty element = elements[j];
                    string keyForElement = GetKeyForElement(element);
                    if (keyForElement == assetKey)
                    {
                        indexToMove = j;
                        //propToMove = element;
                    }
                }

                if (indexToMove == -1)
                {
                    LDtkDebug.LogError("LDtk: Was not able to properly get the matching named element");
                    continue;
                }
                
                if (indexToMove == i)
                {
                    //Debug.Log($"LDtk: {indexToMove} was already in the same index position");
                    //SKIP THIS, was already in same position
                    continue;
                }

                //Debug.Log($"Swapped {indexToMove} --> {i}");
                ArrayProp.MoveArrayElement(indexToMove, i);
            }
        }

        private SerializedProperty GetKeyPropForArray(int arrayIndex)
        {
            SerializedProperty element = ArrayProp.GetArrayElementAtIndex(arrayIndex);
            return GetKeyPropForElement(element);
        }
        private SerializedProperty GetKeyPropForElement(SerializedProperty element)
        {
            return element.FindPropertyRelative(LDtkAsset<Object>.PROPERTY_KEY);
        }
        private SerializedProperty GetValuePropForElement(SerializedProperty element)
        {
            return element.FindPropertyRelative(LDtkAsset<Object>.PROPERTY_ASSET);
        }
        
        private string GetKeyForArray(int arrayIndex)
        {
            SerializedProperty keyProp = GetKeyPropForArray(arrayIndex);
            return keyProp.stringValue;
        }
        private string GetKeyForElement(SerializedProperty element)
        {
            SerializedProperty keyProp = GetKeyPropForElement(element);
            return keyProp.stringValue;
        }

        protected abstract void GetDrawers(T[] defs, List<LDtkContentDrawer<T>> drawers);
        protected virtual int GetSizeOfArray(T[] datas)
        {
            return datas.Length;
        }
        protected virtual string[] GetAssetKeysFromDefs(T[] defs)
        {
            return defs.Select(p => p.Identifier).ToArray();
        }

        protected override void DrawDropdownContent()
        {
            foreach (LDtkContentDrawer<T> drawer in _drawers)
            {
                drawer?.Draw();
            }
        }
    }
}