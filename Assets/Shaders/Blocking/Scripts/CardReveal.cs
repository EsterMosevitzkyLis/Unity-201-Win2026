using UnityEngine;

public class CardReveal : MonoBehaviour
{
    [Header("Parents")]
    public GameObject backParent;
    public GameObject frontParent;
    public GameObject auraParent; // rarity glow behind card

    private Animator anim;
    private bool revealed = false;

    void Awake()
    {
        anim = GetComponent<Animator>();

        // Initial state
        frontParent.SetActive(false);
        auraParent.SetActive(false);
    }

    // Hover start
    void OnMouseEnter()
    {
        if (revealed) return;

        // Animator will handle scale, shake, and faint glow
        anim.SetBool("HoverBack", true);
    }

    // Hover end
    void OnMouseExit()
    {
        if (revealed) return;

        anim.SetBool("HoverBack", false);
    }

    // Click
    void OnMouseDown()
    {
        if (revealed) return;

        revealed = true;

        // stop hover animation
        anim.SetBool("HoverBack", false);

        // trigger flip/dissolve animation
        anim.SetTrigger("RevealCard");
    }

    // Called via Animation Event at mid-flip to swap sides
    public void SwapToFront()
    {
        backParent.SetActive(false);
        frontParent.SetActive(true);

        // Aura/rarity glow can also be activated via Animator or here
        auraParent.SetActive(true);
    }
}