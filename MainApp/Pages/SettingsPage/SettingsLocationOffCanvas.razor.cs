﻿using System.Collections.Generic;
using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Models;

namespace MainApp.Pages.SettingsPage;

public partial class SettingsLocationOffCanvas : ComponentBase
{
    [Inject]
    private IGoogleService _googleService { get; set; } = default!;

    [Inject]
    private IOffCanvasService _offCanvasService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    private LocationModel _locationModel { get; set; } = new();
    private UserLocationModel _userLocationModel { get; set; } = new();
    private List<LocationModel> _locationlist { get; set; } = new();

    private bool _isProcessing { get; set; } = false;
    private bool _isVerifying { get; set; } = false;
    private bool _formIsInvalid { get; set; } = false;
    private bool _userLocationIsInvalid { get; set; } = false;

    public SettingsLocationOffCanvas()
    {
    }

    public async Task OpenAsync()
    {
        await ResetDefaults();
        await _offCanvasService.AddRecordAsync();
        await Task.CompletedTask;
    }

    private async Task CloseOffCanvasAsync()
    {
        await _offCanvasService.CloseAsync();
        await Task.CompletedTask;
    }

    private async Task ResetDefaults()
    {
        _locationModel = new();
        _userLocationModel = new();
        _locationlist = new();
        _formIsInvalid = false;
        _userLocationIsInvalid = false;
        _isProcessing = false;

        await Task.CompletedTask;
    }

    private async Task VerifyAddress()
    {
        try
        {
            _formIsInvalid = false;

            if (string.IsNullOrWhiteSpace(_locationModel.Address))
            {
                _formIsInvalid = true;
                return;
            }

            _isVerifying = true;

            Response<List<LocationModel>> response = await _googleService.GetGeocodingAddressAsync(_locationModel.Address);

            if (response.Success)
            {
                _locationlist = response.Data;
            }
            else
            {
                _locationlist = new();
                _toastService.ShowToast(response.ErrorMessage, Theme.Danger);
            }

            _isVerifying = false;

        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task SelectAddress(LocationModel location)
    {
        _userLocationModel.Location = location;
        await Task.CompletedTask;
    }

    private async Task SaveAddress()
    {
        try
        {

            _isProcessing = true;

            if (_userLocationModel.Location.Latitude == 0 && _userLocationModel.Location.Longitude == 0)
            {
                _userLocationIsInvalid = true;
                _isProcessing = false;

                return;
            }

            _toastService.ShowToast("Location added!", Theme.Info);
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }
}
