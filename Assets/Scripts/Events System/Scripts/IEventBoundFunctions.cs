using System;

/// <summary>
/// Must also inherit MonoBehaviour. 
/// Function that are wanted as available must have the attribute [EventBoundFunction] and be public.
/// Careful: changing the name of the function will unlink it's reference in the inspector on all EventListener
/// </summary>
public interface IEventBoundFunctions
{
}

[AttributeUsage(AttributeTargets.Method)]
public class EventBoundFunctionAttribute : Attribute
{
}