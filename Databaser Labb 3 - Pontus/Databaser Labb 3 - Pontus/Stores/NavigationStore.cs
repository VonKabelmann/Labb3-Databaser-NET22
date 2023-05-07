using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Databaser_Labb_3___Pontus.Stores;

public class NavigationStore
{
    private ObservableObject _currentViewModel;

    public ObservableObject CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    public event Action CurrentViewModelChanged;

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}