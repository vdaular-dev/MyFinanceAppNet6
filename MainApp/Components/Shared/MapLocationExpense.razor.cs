﻿using DateTimeLibrary.Models;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Shared;

public partial class MapLocationExpense : ComponentBase
{
    [Parameter]
    public MapMarkerColor Color { get; set; } = MapMarkerColor.Red;

    [Parameter]
    public MapSize Width { get; set; } = MapSize.Width400;

    [Parameter]
    public MapSize Height { get; set; } = MapSize.Height250;

    [Parameter]
    public Position Position { get; set; } = Position.Start;

    [Parameter]
    public PeriodRange PeriodRange { get; set; } = PeriodRange.Month;

    [Parameter]
    public MapScale Scale { get; set; } = MapScale.Desktop;

    [Parameter]
    public DateTimeRange DateRange { get; set; } = new();

    [Inject]
    private IExpenseService<ExpenseModel> _expenseService { get; set; } = default!;

    [Inject]
    private IGoogleService _googleService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Inject]
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    private DateTimeRange _dateTimeRange { get; set; } = new();
    private List<LocationModel> _locations = new();
    private string _dropdownLabel { get; set; } = Label.AppNoDateAssigned;
    private bool _isLoading { get; set; } = true;

    public MapLocationExpense()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRange = DateRange;
        _dropdownLabel = await _dropdownDateRangeService.UpdateLabel(_dateTimeRange);
        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await FetchDataAsync();
            }
            catch (Exception ex)
            {
                _isLoading = false;
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }

            await InvokeAsync(StateHasChanged);
        }

        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _locations = await _expenseService.GetLocationExpenseList(_dateTimeRange);
            _isLoading = false;
        }
        catch (Exception ex)
        {
            _isLoading = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task DropdownDateRangeRefresh(DateTimeRange dateTimeRange)
    {
        _isLoading = true;

        await Task.Delay((int)Delay.DataLoading);

        _dateTimeRange = dateTimeRange;
        _dropdownLabel = await _dropdownDateRangeService.UpdateLabel(dateTimeRange);
        _toastService.ShowToast(Label.AppMessageDateRangeChanged, Theme.Info);

        await FetchDataAsync();
        await Task.CompletedTask;
    }
}
