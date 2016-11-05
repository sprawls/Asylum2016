using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventListener), true)]
public class EventListenerInspector : Editor
{
    private GUIStyle _titleStyle = null;
    private EventListener _listenerRef = null;
    private List<EventListener.EventFunction> _availableFunctions = new List<EventListener.EventFunction>();
    private static List<Type> _eventBoundTypes = null; 

    //------------------------------------------------------------------
    [UsedImplicitly]
    private void OnEnable()
    {
        if (_eventBoundTypes == null)
        {
            _eventBoundTypes = ReflectionUtils.GetAllChildrenOfType(typeof(IEventBoundFunctions));
        }

        UpdateAvailableFunctions();
    }

    //------------------------------------------------------------------
    public override void OnInspectorGUI()
    {
        SetValues();

        List<EventListener.EventFunction> chosens = _listenerRef.Functions;
        List<int> taggedToBeRemoved = null;

        for (int i = 0; i < chosens.Count; ++i)
        {
            EditorGUILayout.BeginHorizontal();

            //Popup
            int choice = EditorGUILayout.Popup(GetFunctionIndex(chosens[i]), GenerateFunctionsName(_availableFunctions));

            if (choice >= 0)
            {
                chosens[i] = _availableFunctions[choice];
            }
            else
            {
                chosens[i] = null;
            }

            //Button
            if (GUILayout.Button("Remove"))
            {
                if (taggedToBeRemoved == null) taggedToBeRemoved = new List<int>(); //Created if needed to remove unnecessary allocs
                taggedToBeRemoved.Add(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add function"))
        {
            chosens.Add(null); 
        }

        if (taggedToBeRemoved != null)
        {
            //Remove tagged objects
            for (int i = taggedToBeRemoved.Count -1; i >= 0; --i)
                //Inverse so the index don't change after the first removal
            {
                chosens.RemoveAt(taggedToBeRemoved[i]);
            }
        }
    }

    //------------------------------------------------------------------
    private int GetFunctionIndex(EventListener.EventFunction function)
    {
        if (function == null || function.ComponentRef == null)
            return -1;

        for (int i = 0; i < _availableFunctions.Count; ++i)
        {
            if (_availableFunctions[i].Name == function.Name)
            {
                return i;
            } 
        }

        return -1;
    }

    //------------------------------------------------------------------
    private string[] GenerateFunctionsName(List<EventListener.EventFunction> functions)
    {
        string[] names = new string[functions.Count];

        for (int i = 0; i < functions.Count; ++i)
        {
            names[i] = string.Format("[{0}] {1}", functions[i].ComponentRef.GetType(), functions[i].Name);
        }

        return names;
    }

    //------------------------------------------------------------------
    private void SetValues()
    {
        if (_titleStyle == null)
        {
            _titleStyle = new GUIStyle(EditorStyles.boldLabel);
            _titleStyle.fixedHeight = 20;
        }

        if (_listenerRef == null)
        {
            _listenerRef = (EventListener)target;
        }
    }

    //------------------------------------------------------------------
    private void UpdateAvailableFunctions()
    {
        SetValues();

        _availableFunctions.Clear();

        for (int i = 0; i < _eventBoundTypes.Count; ++i)
        {
            Type t = _eventBoundTypes[i];
            if (typeof(MonoBehaviour).IsAssignableFrom(t))
            {
                Component component = _listenerRef.gameObject.GetComponent(t); //Gets only the first class of this type (there shouldn't multiples)
                MethodInfo[] methods = t.GetMethods(); //Get all methods

                if (component != null)
                {
                    for (int j = 0; j < methods.Length; ++j)
                    {
                        MethodInfo info = methods[j];

                        //See if tagged as EventBoundFunction
                        if (info.GetCustomAttributes(typeof(EventBoundFunctionAttribute), false).Length > 0) 
                        {
                            _availableFunctions.Add(new EventListener.EventFunction(info.Name, component));
                        }
                    }
                }
            }
        }
    }
}