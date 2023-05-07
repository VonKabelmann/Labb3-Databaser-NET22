using CommunityToolkit.Mvvm.ComponentModel;
using DataAccess.Managers;
using Databaser_Labb_3___Pontus.Stores;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess.Models;

namespace Databaser_Labb_3___Pontus.ViewModels;

public class ShopSelectViewModel : ObservableObject
{
    private readonly NavigationStore _navigationStore;
    private readonly UserManager _userManager;
    private readonly ShopManager _shopManager;

    public ShopSelectViewModel(NavigationStore navigationStore, UserManager userManager)
    {
        _navigationStore = navigationStore;
        _userManager = userManager;
        _shopManager = new ShopManager();
        _shopList = new ObservableCollection<Shop>(_shopManager.GetAll());
        ConfirmCommand = new RelayCommand(() =>
            {
                _shopManager.CurrentShop = _selectedShop;
                _navigationStore.CurrentViewModel = new ShopMenuViewModel(_navigationStore, _userManager, _shopManager);
            }, 
            () => _selectedShop is not null);
        ExitCommand = new RelayCommand(() =>
            _navigationStore.CurrentViewModel = new MainMenuViewModel(_navigationStore, _userManager));
    }

    private ObservableCollection<Shop> _shopList;

    public ObservableCollection<Shop> ShopList
    {
        get => new ObservableCollection<Shop>(_shopList);
        set => SetProperty(ref _shopList, value);
    }

    private Shop _selectedShop;

    public Shop SelectedShop
    {
        get => _selectedShop;
        set
        {
            SetProperty(ref _selectedShop, value);
            ConfirmCommand.NotifyCanExecuteChanged();
        }
    }

    public RelayCommand ConfirmCommand { get; }

    public RelayCommand ExitCommand { get; }
    
}