using CommunityToolkit.Mvvm.ComponentModel;
using Databaser_Labb_3___Pontus.Stores;

namespace Databaser_Labb_3___Pontus.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly NavigationStore _navigationStore;
    public ObservableObject CurrentViewModel => _navigationStore.CurrentViewModel;

    public MainViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}