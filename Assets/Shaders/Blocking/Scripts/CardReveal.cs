using UnityEngine;

public class CardReveal : MonoBehaviour
{
    public GameObject backSide;
    public GameObject frontSide;
    public GameObject Shader;
    private bool revealed = false;
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Reveal()
    {
        if (revealed) return;

        Debug.Log("Reveal pressed");
        revealed = true;

        anim.SetBool("Revealed", true);
    }

    // Called by Animation Event at mid-flip
    public void SwapToFront()
    {
        backSide.SetActive(false);
        frontSide.SetActive(true);
        Shader.SetActive(true);
    }
}
