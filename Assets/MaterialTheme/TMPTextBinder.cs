using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TMPTextBinder : BaseTypographyBinder
{
    [SerializeField] TMP_Text m_text;

    protected override void Awake()
    {
        if (m_text == null) m_text = GetComponent<TMP_Text>();

        base.Awake();
    }

    public override void SetTypographyAttr(MaterialTypographyAttr typographyAttr)
    {
        if (m_text == null)
        {
            return;
        }
        m_text.font = typographyAttr.fontAsset;
        m_text.fontSize = typographyAttr.fontSize;
    }
}
