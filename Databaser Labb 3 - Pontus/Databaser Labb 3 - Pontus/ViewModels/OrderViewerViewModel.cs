using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess.Managers;
using DataAccess.Models;
using Databaser_Labb_3___Pontus.Stores;

namespace Databaser_Labb_3___Pontus.ViewModels;

public class OrderViewerViewModel : ObservableObject
{
    private readonly NavigationStore _navigationStore;
    private readonly UserManager _userManager;
    private readonly OrderManager _orderManager;
    private readonly ShopManager _shopManager;

    public OrderViewerViewModel(NavigationStore navigationStore, UserManager userManager)
    {
        _navigationStore = navigationStore;
        _userManager = userManager;
        _orderManager = new OrderManager();
        _shopManager = new ShopManager();
        _customerUsername = string.Empty;
        _shopName = string.Empty;

        _orderList = new ObservableCollection<Order>(_orderManager.GetAll());

        ExitCommand = new RelayCommand(() =>
            _navigationStore.CurrentViewModel = new MainMenuViewModel(_navigationStore, _userManager));
    }

    private string _customerUsername;

    public string CustomerUsername
    {
        get => _customerUsername;
        set => SetProperty(ref _customerUsername, value);
    }

    private string _shopName;

    public string ShopName
    {
        get => _shopName;
        set => SetProperty(ref _shopName, value);
    }
    
    private ObservableCollection<Order> _orderList;

    public ObservableCollection<Order> OrderList
    {
        get => _orderList;
        set => SetProperty(ref _orderList, value);
    }

    private Order _selectedOrder;

    public Order SelectedOrder
    {
        get => _selectedOrder;
        set
        {
            SetProperty(ref _selectedOrder, value);
            UpdateFields();
        }
    }

    private ObservableCollection<ProductContainer> _orderProducts;

    public ObservableCollection<ProductContainer> OrderProducts
    {
        get => _orderProducts; 
        set => SetProperty(ref _orderProducts, value);
    }
    public RelayCommand ExitCommand { get; }

    private void UpdateFields()
    {
        _customerUsername = _userManager.GetUserById(_selectedOrder.UserId)!.UserName;
        OnPropertyChanged(nameof(CustomerUsername));

        _shopName = _shopManager.GetShopById(_selectedOrder.ShopId)!.ShopName;
        OnPropertyChanged(nameof(ShopName));

        _orderProducts = new ObservableCollection<ProductContainer>(_selectedOrder.ListOfProducts);
        OnPropertyChanged(nameof(OrderProducts));
    }
}