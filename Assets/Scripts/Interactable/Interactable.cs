using System.Deployment.Internal;
using UnityEngine;
using JetBrains.Annotations;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    private float _outlineValue;
    private bool _highlightActive = false;
    private OutlineController[] _outlineControllers;

	private static bool _NEVER_ILLUMINATED = true;

    public virtual bool IsInteractable { get { return true; } }

    protected abstract void OnTrigger();

    [UsedImplicitly]
    private void Start()
    {
        _outlineControllers = GetComponentsInChildren<OutlineController>();
        
        ClearTargeted();
    }

    public void Trigger()
    {
        OnTrigger();
		TutorialMachine.Instance.HideMessage(TutorialMachine.EMessageType.ClickToInteract);
    }

    public void SetTargeted()
    {
        _highlightActive = true;
        SetOutline(true);

		if (_NEVER_ILLUMINATED) {
			TutorialMachine.Instance.ShowMessage(TutorialMachine.EMessageType.ClickToInteract);
			_NEVER_ILLUMINATED = false;
		}
    }

    public void ClearTargeted()
    {
        _highlightActive = false;
        SetOutline(false);
    }

    private void SetOutline(bool show)
    {
        for (int i = 0; i < _outlineControllers.Length; ++i)
        {
            if (show)
            {
                _outlineControllers[i].Show();
            }
            else
            {
                _outlineControllers[i].Hide();
            }
        }
    }
}