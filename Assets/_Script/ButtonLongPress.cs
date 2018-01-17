using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonLongPress : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public UnityEvent onLongPress = new UnityEvent();
    public float holdTime = 0.5f;
    public bool isRepeat = false;
    public float repeatRate = 0.2f;
    public RectTransform rt;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isRepeat)
            InvokeRepeating("OnLongPress", holdTime, repeatRate);
        else
            Invoke("OnLongPress", holdTime);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        CancelInvoke("OnLongPress");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        CancelInvoke("OnLongPress");
    }
    protected override void OnDisable()
    {
        CancelInvoke("OnLongPress");
    }
    protected override void OnDestroy() {
        CancelInvoke("OnLongPress");
    }
    void OnLongPress()
    {
        onLongPress.Invoke();
    }
}
