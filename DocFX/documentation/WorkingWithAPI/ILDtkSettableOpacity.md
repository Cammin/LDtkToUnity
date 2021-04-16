# `ILDtkSettableOpacity`

An interface that contracts a function intended to set an entity's alpha colour.  
This passes in a `float` ranging from 1 to 0, using the alpha value from the LDtk layer's opacity value that the entity was stored in.  
```
SpriteRenderer renderer;
public void OnLDtkSetOpacity(float alpha)
{
    Color newColor = renderer.color;
    newColor.a = newAlpha;
    renderer.color = newColor;
}
```