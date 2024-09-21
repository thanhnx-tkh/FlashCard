using System.Collections;
using UnityEngine;

public class Movable : MonoBehaviour
{
    private Vector2 from, to;
    private float howfar;
    [SerializeField]
    private float speed = 2;
    private bool idle = true;
    public bool Idle
    {
        get { return idle; }
    }
    protected Vector2 positonStart;
    private RectTransform rectTransform; // RectTransform của UI element

    // Coroutine di chuyển UI từ vị trí hiện tại đến vị trí mới
    protected virtual void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // Lấy RectTransform của UI
        positonStart = rectTransform.anchoredPosition; // Lưu vị trí bắt đầu
    }

    public IEnumerator MoveToPostion(Vector2 targetPostion)
    {
        if (speed <= 0)
        {
            Debug.LogWarning("Speed must be a positive number");
        }
        
        from = rectTransform.anchoredPosition; // Lấy vị trí hiện tại
        to = targetPostion; // Vị trí đích
        howfar = 0;
        idle = false;

        do
        {
            howfar += speed * Time.deltaTime;
            if (howfar > 1)
            {
                howfar = 1;
            }

            // Di chuyển UI bằng cách cập nhật anchoredPosition
            rectTransform.anchoredPosition = Vector2.LerpUnclamped(from, to, howfar);
            yield return null;
        }
        while (howfar != 1);

        idle = true;

        // Sau khi di chuyển đến vị trí đích, quay lại vị trí ban đầu
    }
}
