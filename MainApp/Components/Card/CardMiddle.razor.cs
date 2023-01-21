﻿using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Card;

public partial class CardMiddle
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string Styles { get; set; }

    public CardMiddle()
    {
        Styles = string.Empty;
    }
}
