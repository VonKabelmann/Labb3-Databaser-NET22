using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess.Managers;
using Databaser_Labb_3___Pontus.Stores;

namespace Databaser_Labb_3___Pontus.ViewModels;

public class MainMenuViewModel : ObservableObject
{
    private readonly NavigationStore _navigationStore;
    private readonly UserManager _userManager;

    public MainMenuViewModel(NavigationStore navigationStore, UserManager userManager)
    {
        _navigationStore = navigationStore;
        _userManager = userManager;
        ShopsCommand = new RelayCommand(() =>
            _navigationStore.CurrentViewModel = new ShopSelectViewModel(_navigationStore, _userManager));
        ProductsCommand = new RelayCommand(() =>
            _navigationStore.CurrentViewModel = new ProductSelectViewModel(_navigationStore, _userManager));
        OrdersCommand = new RelayCommand(() =>
            _navigationStore.CurrentViewModel = new OrderViewerViewModel(_navigationStore, _userManager));
        LogoutCommand =
            new RelayCommand(() => _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore));
    }

    public RelayCommand ShopsCommand { get; }
    public RelayCommand ProductsCommand { get; }
    public RelayCommand OrdersCommand { get; }
    public RelayCommand LogoutCommand { get; }
}