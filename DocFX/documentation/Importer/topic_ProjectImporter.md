# Project Importer

## Inspector
The importer inspector is composed of several sections:   
[**Main**](topic_Section_Main.md), 
[**IntGrids**](topic_Section_IntGrids.md), 
[**Entities**](topic_Section_Entities.md), 
[**Enums**](topic_Section_Enums.md),
and [**Dependencies**](topic_Section_Dependencies.md).  
![Inspector](../../images/img_Unity_ProjectAsset.png)
- After making any changes, click the apply button at the bottom to reimport.
- If any section is hidden, it's because there were no associated definitions in the LDtk project.

## Hierarchy
The importer generates a gameobject hierarchy of the json structure.  
Topic: [**Hierarchy & Components**](../Topics/topic_HierarchyAndComponents.md)

## Sub-Assets
In addition to the generated GameObjects, some other sub-assets are also generated:
- [**Definition Objects**](../Topics/topic_HierarchyAndComponents.md)
- [**Artifact Asset**](../Topics/topic_ArtifactAssets.md)
- [**Json Project**](../Topics/topic_ProjectFile.md)
- [**Table of Contents**](../Topics/topic_TableOfContents.md)
    
![Project Window](../../images/img_unity_ProjectWindow.png)

## Nested Prefabs
Imported LDtk projects can be nested in prefabs.  
![Nested Project](../../images/img_Unity_NestedProject.png)
![Nested Project Variant](../../images/img_Unity_NestedProjectVariant.png)