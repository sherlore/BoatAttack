using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ColorEventBinder : BaseColorBinder
{
    public UnityEvent<Color> colorEvent;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetColor(Color color)
    {
        colorEvent.Invoke( color );
    }
}
