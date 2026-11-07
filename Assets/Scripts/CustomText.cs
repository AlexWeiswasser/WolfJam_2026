using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class CustomText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fillTextUGUI;
    [SerializeField] TextMeshProUGUI outlineTextUGUI;
    [SerializeField] TextMeshPro fillText;
    [SerializeField] TextMeshPro outlineText;

    public void SetText(string text)
    {

        if (fillText != null) fillText.text = text;
        if (outlineText != null) outlineText.text = RemoveColorTags(text);
        if (fillTextUGUI != null) fillTextUGUI.text = text;
        if (outlineTextUGUI != null) outlineTextUGUI.text = RemoveColorTags(text);
    }

    public void SetColor(Color color)
    {
        if (fillText != null) fillText.color = color;
        if (fillTextUGUI != null) fillTextUGUI.color = color;
    }

    public void SetColorWithFade(Color color)
    {
        if (fillText != null) fillText.color = color;
        if (fillTextUGUI != null) fillTextUGUI.color = color;
        if (outlineText != null) outlineText.alpha = color.a;
        if (outlineTextUGUI != null) outlineTextUGUI.alpha = color.a;
    }

    public void SetFontSize(float size)
    {
        if (fillText != null) fillText.fontSize = size;
        if (fillTextUGUI != null) fillTextUGUI.fontSize = size;
        if (outlineText != null) outlineText.fontSize = size;
        if (outlineTextUGUI != null) outlineTextUGUI.fontSize = size;
    }

	private static readonly HashSet<string> OutlineAllowedTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
	    "shake", "wave", "funky", "jump", "pivot", "pivotc",
	    "sketchy", "dangle", "shear", "swing", "spread"
    };

	private static readonly Regex TagRegex = new Regex(@"<\s*/?\s*([^\s=>/]+)[^>]*>", RegexOptions.Compiled);

	private string RemoveColorTags(string text)
	{
		return TagRegex.Replace(text, m => OutlineAllowedTags.Contains(m.Groups[1].Value) ? m.Value : string.Empty);
	}
}
