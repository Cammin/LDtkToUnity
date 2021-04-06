Interface

## Description

An interface that contracts a reflection event to fire during the moment after an entity's fields is injected. 
Use this for immediately reacting to the values being provided to an instantiated entity.

The order of reflection events is as follows:  
`Awake`, `OnEnable`, `OnLDtkFieldsInjected`, `Start`.  

**[[LDtkField]]s do not require an implementation of this interface to work. This interface is optional when needed.**

``` 
public class Player : MonoBehaviour, ILDtkFieldInjectedEvent
{
    [LDtkField] public int lives;

    public void OnLDtkFieldsInjected()
    {
        Debug.Log($"I have {lives} lives");
    }
}
```

| Public Methods | |
|---|---|
| [[OnLDtkFieldsInjected]] | Reflection event that is called after an entity's field injection phase.  |