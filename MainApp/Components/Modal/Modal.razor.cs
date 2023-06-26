﻿using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Modal;

public partial class Modal : ComponentBase
{
    // TODO: Add service for the modal

    [Inject]
    private IAppSettingsService _appSettingsService { get; set; } = default!;

    [Parameter]
    public RenderFragment? Title { get; set; }

    [Parameter]
    public RenderFragment? Body { get; set; }

    [Parameter]
    public RenderFragment? Footer { get; set; }

    [Parameter]
    public Size Size { get; set; } = Size.Md;

    [Parameter]
    public bool IsCloseButtonVisible { get; set; } = true;

    private ModalDisplay _modalStyleDisplay { get; set; } = ModalDisplay.none;
    private string _modalClass { get; set; } = string.Empty;
    private string _radius { get; set; } = Radius.Default;
    private bool _showBackdrop { get; set; } = false;
    private Guid _modalId { get; set; } = Guid.Empty;

    public Modal()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _radius = await _appSettingsService.GetModalShape();
            await InvokeAsync(StateHasChanged);
        }

        await Task.CompletedTask;
    }

    public async Task Open(Guid target)
    {
        _modalId = target;
        _modalStyleDisplay = ModalDisplay.block;
        await Task.Delay((int)Delay.ModalOpen);
        _modalClass = "show";
        _showBackdrop = true;

        StateHasChanged();
        await Task.CompletedTask;
    }

    public async Task Close(Guid target)
    {
        _modalId = target;
        _modalClass = string.Empty;
        await Task.Delay((int)Delay.ModalClose);
        _modalStyleDisplay = ModalDisplay.none;
        _showBackdrop = false;

        StateHasChanged();
        await Task.CompletedTask;
    }
}
