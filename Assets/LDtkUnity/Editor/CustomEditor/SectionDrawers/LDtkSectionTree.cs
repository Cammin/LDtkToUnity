using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    /*[ExcludeFromDocs]
    public class LDtkSectionTree : LDtkSectionDrawer
    {
        private LDtkTreeViewWrapper _tree;

        protected override string GuiText => "Hierarchy";
        protected override string GuiTooltip => "This is the hierarchy of the LDtk json data.";
        protected override Texture GuiImage => LDtkIconUtility.GetUnityIcon("UnityEditor.SceneHierarchyWindow", "");
        protected override string ReferenceLink => null;

        public LDtkSectionTree(SerializedObject serializedObject) : base(serializedObject)
        {
        }
        
        public void SetJson(LdtkJson data)
        {
            _tree = new LDtkTreeViewWrapper(data);
        }
        
        public override void Dispose()
        {
            _tree?.Dispose();
        }

        protected override void DrawDropdownContent()
        {
            _tree?.OnGUI();
        }
    }*/
}