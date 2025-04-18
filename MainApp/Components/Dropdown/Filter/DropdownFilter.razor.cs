﻿using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Dropdown.Filter;

public partial class DropdownFilter : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    [Parameter]
    public bool IsDisplayLargeNone { get; set; } = false;

    [Parameter]
    public Size ButtonSize { get; set; } = Size.Md;

    [Parameter]
    public string IconStart { get; set; } = string.Empty;

    [Parameter]
    public Theme ButtonColor { get; set; } = Theme.Secondary;

    [Parameter]
    public Theme IconStartColor { get; set; } = Theme.Secondary;

    [Parameter]
    public string IconEnd { get; set; } = "bi-chevron-down";

    [Parameter]
    public string DropdownLabel { get; set; } = Label.AppNoFilterAssigned;

    [Parameter]
    public Position DropdownPosition { get; set; } = Position.Start;

    [Parameter]
    public FilterModel Model { get; set; } = default!;

    public DropdownFilter()
    {
    }

    private async Task ResetFilter()
    {
        await OnSubmitSuccess.InvokeAsync();
        await Task.CompletedTask;
    }
}
