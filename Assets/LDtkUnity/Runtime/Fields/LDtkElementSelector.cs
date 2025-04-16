namespace LDtkUnity
{
    internal delegate T LDtkElementSelector<out T>(LDtkFieldElement element, out bool success);
}