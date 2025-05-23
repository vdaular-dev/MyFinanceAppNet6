﻿using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SetupPage.TransactionCategory;

public partial class SetupTransactionCategoryOffCanvas : ComponentBase
{
    [Inject]
    private ITransactionCategoryService<TransactionCategoryModel> _transactionCategoryService { get; set; } = default!;

    [Inject]
    private IOffCanvasService _offCanvasService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Parameter]
    public IAppSettings AppSettings { get; set; } = default!;

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    [Parameter]
    public TransactionCategoryModel DataModel { get; set; } = default!;

    private bool _displayErrorMessages { get; set; } = false;
    private bool _isProcessing { get; set; } = false;
    private InputFormAttributes _inputFormAttributes{ get; set; } = new();
    private TransactionCategoryModel _transactionCategoryModel { get; set; } = new();
    private List<ActionTypeModel> _actionTypes { get; set; } = new();

    public SetupTransactionCategoryOffCanvas()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        ActionTypeModel actionTypeModel = new()
        {
            Id = "C",
            Name = "Credit"
        };
        _actionTypes.Add(actionTypeModel);

        actionTypeModel = new()
        {
            Id = "D",
            Name = "Debit"
        };
        _actionTypes.Add(actionTypeModel);

        actionTypeModel = new()
        {
            Id = "T",
            Name = "Transfer"
        };
        _actionTypes.Add(actionTypeModel);

        await Task.CompletedTask;
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _inputFormAttributes.Control = new()
                {
                    {
                        "class", $"form-control rounded{AppSettings.Form}"
                    }
                };

                _inputFormAttributes.PlainText = new()
                {
                    {
                        "class", $"form-control-plaintext"
                    }
                };

                _inputFormAttributes.Select = new()
                {
                    {
                        "class", $"form-select rounded{AppSettings.Form}"
                    }
                };
            }
            catch (Exception ex)
            {
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }
        }

        await Task.CompletedTask;
    }

    public async Task AddRecordOffCanvasAsync()
    {
        _transactionCategoryModel = new();

        await _offCanvasService.AddRecordAsync();
        await Task.CompletedTask;
    }

    public async Task EditRecordOffCanvasAsync(string id)
    {
        try
        {
            _transactionCategoryModel = await _transactionCategoryService.GetRecordById(id);
            if (_transactionCategoryModel is not null)
            {
                await _offCanvasService.EditRecordAsync(id);
            }
            else
            {
                _transactionCategoryModel = new();
                _toastService.ShowToast(Label.AppNoRecordFound, Theme.Danger);
            }
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    public async Task ViewRecordOffCanvasAsync(string id)
    {
        try
        {
            _transactionCategoryModel = await _transactionCategoryService.GetRecordById(id);
            if (_transactionCategoryModel is not null)
            {
                await _offCanvasService.ViewRecordAsync(id);
            }
            else
            {
                _transactionCategoryModel = new();
                _toastService.ShowToast(Label.AppNoRecordFound, Theme.Danger);
            }
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task UpdateFormStateAsync()
    {
        await _offCanvasService.UpdateFormStateAsync();
        await Task.CompletedTask;
    }

    private async Task ArchiveAsync()
    {
        try
        {
            await _offCanvasService.ArchiveRecordAsync();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task HandleValidSubmitAsync()
    {
        try
        {
            _displayErrorMessages = false;
            _isProcessing = true;

            var offCanvasViewType = _offCanvasService.GetOffCanvasViewType();

            if (offCanvasViewType == OffCanvasViewType.Add)
            {
                await _transactionCategoryService.CreateRecord(_transactionCategoryModel);

                _transactionCategoryModel.Id = await _transactionCategoryService.GetLastInsertedId();
                _toastService.ShowToast(Label.AppMenuSetupTransaction+" "+Label.AppAdded, Theme.Success);
            }
            else if (offCanvasViewType == OffCanvasViewType.Edit)
            {
                await _transactionCategoryService.UpdateRecord(_transactionCategoryModel);
                _toastService.ShowToast(Label.AppMenuSetupTransaction+" "+Label.AppUpdated, Theme.Success);
            }
            else if (offCanvasViewType == OffCanvasViewType.Archive)
            {
                await _transactionCategoryService.ArchiveRecord(_transactionCategoryModel);
                _toastService.ShowToast(Label.AppMenuSetupTransaction+" "+Label.AppArchived, Theme.Success);
            }
            _isProcessing = false;

            DataModel = _transactionCategoryModel;

            await OnSubmitSuccess.InvokeAsync();
            await Task.Delay((int)Delay.DataSuccess);
            await CloseOffCanvasAsync();
        }
        catch (Exception ex)
        {
            _isProcessing = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task HandleInvalidSubmitAsync()
    {
        _isProcessing = false;
        _displayErrorMessages = true;
        await Task.CompletedTask;
    }

    private async Task CloseOffCanvasAsync()
    {
        _transactionCategoryModel = new();
        await _offCanvasService.CloseAsync();
        await Task.CompletedTask;
    }
}
