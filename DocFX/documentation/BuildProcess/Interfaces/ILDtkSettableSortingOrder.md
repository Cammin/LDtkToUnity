An interface that contracts a function intended to set an entity's sorting order.  
This passes in an `int` that represents the sorting order to set.  
The sorting order value is automatically determined by the layer generation; use the interface for simply setting a renderer's sorting order if applicable. ex. Renderer, SpriteRenderer, SortingGroup, etc.
```
SpriteRenderer renderer;
public void OnLDtkSetSortingOrder(int sortingOrder)
{
    renderer.sortingOrder = sortingOrder;
}
```