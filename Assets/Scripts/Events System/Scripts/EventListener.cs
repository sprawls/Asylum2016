using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public abstract class EventListener : MonoBehaviour
{
    [System.Serializable]
    public class EventFunction
    {
        public string Name { get { return _name; } }
        public Component ComponentRef { get { return _componentRef; } }

        [SerializeField, HideInInspector]
        private string _name;

        [SerializeField, HideInInspector]
        private Component _componentRef;

        private MethodInfo _method;

        public EventFunction(string name, Component component)
        {
            _name = name;
            _componentRef = component; 
        }

        public void Init()
        {
            _method = ComponentRef.GetType().GetMethod(_name);
        }

        public void Invoke()
        {
            if (_componentRef == null || _method == null)
                return;


            _method.Invoke(ComponentRef, null);
        }
    }
    
    [SerializeField]
    private bool _throwOnce = false;

    private bool _alreadyThrown = false;

    [HideInInspector]
    public List<EventFunction> Functions = new List<EventFunction>();

    //=====================================================================================================

    protected virtual string Description { get { return ""; } }

    //=====================================================================================================
    //------------------------------------------------------------------------------------
    protected virtual void Awake()
    {
        for (int i = 0; i < Functions.Count; ++i)
        {
            if (Functions[i] == null)
                continue;

            Functions[i].Init();
        }
    }

    //=====================================================================================================
    //------------------------------------------------------------------------------------
    protected void Trigger()
    {
        if (_throwOnce && _alreadyThrown)
            return;

        for (int i = 0; i < Functions.Count; ++i)
        {
            if (Functions[i] == null)
                continue;

            Functions[i].Invoke();
        }

        _alreadyThrown = true;
    }
}