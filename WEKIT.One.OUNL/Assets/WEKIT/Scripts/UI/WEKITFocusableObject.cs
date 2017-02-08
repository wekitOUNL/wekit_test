using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// highlightable object, which changes color and size, when gazed at.
/// Derives from MonoBehaviour and can be further subclassed to change highlighting behaviour.
/// users can specify a focus color (yellow by default) and a focus resize factor between 0.8 and 2.0 (1.2 by default).
/// if a default color is provided and useDefaultColor is checked, this color will be used, when gaze is lost.
/// Otherwise, the default color is read from the provided prefab this script is attached to.
/// </summary>
public class WEKITFocusableObject : MonoBehaviour, IFocusable
{


    [Tooltip("Set to true, if default color should be used instead of prefab's own color.")]
    public bool useDefaultColor = false;

    [Tooltip("Color used when object is in default state.")]
    public Color DefaultColor = Color.gray;

    [Tooltip("Color used when object is gazed at.")]
    public Color FocusColor = Color.yellow;

    [Tooltip("Factor at which the object should be enlarged at gaze enter.")]
    [Range(0.8f, 2.0f)]
    public float FocusSizeFactor = 1.2f;

    [Tooltip("Indicate if focus should also be highlighted in child components.")]
    public bool includeChildComponents = true;

    protected Color currentColor;


    /// <summary>
    /// initializes this component and sets the currentColor from default color or from the model.
    /// </summary>
    public void Start()
    {
        if (useDefaultColor)
        {
            currentColor = DefaultColor;
        }
        else
        {
            currentColor = this.gameObject.GetComponent<Renderer>().material.color;
            if (includeChildComponents)
            {
                foreach (Renderer rend in this.gameObject.GetComponentsInChildren<Renderer>())
                {
                    currentColor = rend.material.color;
                }
            }
        }
    }

    /// <summary>
    /// called, when the user gazes at the object. Applies FocusSizeFactor and FocusColor
    /// </summary>
    public void OnFocusEnter()
    {
        this.gameObject.transform.localScale *= FocusSizeFactor;
        changeColor(FocusColor);
    }


    /// <summary>
    /// called, when the user's gaze leaves the object. Reduces size by FocusSizeFactor and applies currentColor.
    /// </summary>
    public void OnFocusExit()
    {
        this.gameObject.transform.localScale /= FocusSizeFactor;
        changeColor(DefaultColor);
    }


    /// <summary>
    /// convenience method to apply a color to a renderer and all children's renderers.
    /// </summary>
    /// <param name="color">the color to apply.</param>
    public void changeColor(Color color)
    {
        this.gameObject.GetComponent<Renderer>().material.color = color;
        if (includeChildComponents)
        {
            foreach (Renderer rend in this.gameObject.GetComponentsInChildren<Renderer>())
            {
                rend.material.color = color;
            }
        }
    }


}
