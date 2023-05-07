using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess.Managers;
using DataAccess.Models;
using Databaser_Labb_3___Pontus.Stores;

namespace Databaser_Labb_3___Pontus.ViewModels;

public class ShopMenuViewModel : ObservableObject
{
    private readonly NavigationStore _navigationStore;
    private readonly UserManager _userManager;
    private readonly ShopManager _shopManager;
    private readonly ProductManager _productManager;
    private readonly OrderManager _orderManager;

    public ShopMenuViewModel(NavigationStore navigationStore, UserManager userManager, ShopManager shopManager)
    {
        _navigationStore = navigationStore;
        _userManager = userManager;
        _shopManager = shopManager;
        _productManager = new ProductManager();
        _orderManager = new OrderManager();
        ShopName = _shopManager.CurrentShop?.ShopName;
        _amount = 1;
        _productAddMode = true;
        _shoppingCart = new ObservableCollection<ProductContainer>(_userManager.CurrentUser?.ShoppingCart ?? throw new InvalidOperationException());
        _productStock = new ObservableCollection<ProductContainer>(_shopManager.CurrentShop?.ProductStock ?? throw new InvalidOperationException());
        _productList = new ObservableCollection<Product>(_productManager.GetAll());
        RemoveFromCartCommand = new RelayCommand(SetupRemoveFromCartCommand, () => _selectedCartItem is not null);
        AddToCartCommand = new RelayCommand(SetupAddToCartCommand, () => _selectedStockItem is not null);
        AddToStockCommand = new RelayCommand(SetupAddToStockCommand, () => _productAddMode is true && _selectedProduct is not null);
                                                // Eftersom AddToStockCommand sparar direkt till Db baserat på vad som finns i _productStock
                                                // och RemoveFromCartCommand & AddToCartCommand inte sparar till Db förens man bekräftar med CheckoutCommand
                                                // så förbjuds användaren från att trigga AddToStockCommand om användaren triggat en av de andra commandsen.
        CancelCommand = new RelayCommand(SetupCancelCommand);
        CheckoutCommand = new RelayCommand(SetupCheckoutCommand);
    }

    private bool _productAddMode { get; set; }
    public string? ShopName { get; }
    private int _amount;

    public int Amount
    {
        get => _amount;
        set => SetProperty(ref _amount, value);
    }

    private ObservableCollection<ProductContainer> _shoppingCart;

    public ObservableCollection<ProductContainer> ShoppingCart
    {
        get => new ObservableCollection<ProductContainer>(_shoppingCart);
        set => SetProperty(ref _shoppingCart, value);
    }

    private ProductContainer _selectedCartItem;

    public ProductContainer SelectedCartItem
    {
        get => _selectedCartItem;
        set
        {
            SetProperty(ref _selectedCartItem, value);
            RemoveFromCartCommand.NotifyCanExecuteChanged();
        }
    }

    private ObservableCollection<ProductContainer> _productStock;

    public ObservableCollection<ProductContainer> ProductStock
    {
        get => new ObservableCollection<ProductContainer>(_productStock);
        set => SetProperty(ref _productStock, value);
    }

    private ProductContainer _selectedStockItem;

    public ProductContainer SelectedStockItem
    {
        get => _selectedStockItem;
        set
        {
            SetProperty(ref _selectedStockItem, value);
            AddToCartCommand.NotifyCanExecuteChanged();
        }
    }

    private ObservableCollection<Product> _productList;

    public ObservableCollection<Product> ProductList
    {
        get => new ObservableCollection<Product>(_productList);
        set => SetProperty(ref _productList, value);
    }

    private Product _selectedProduct;

    public Product SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            SetProperty(ref _selectedProduct, value); 
            AddToStockCommand.NotifyCanExecuteChanged();
        }
    }

    public RelayCommand RemoveFromCartCommand { get; }
    public RelayCommand AddToCartCommand { get; }
    public RelayCommand AddToStockCommand { get; }
    public RelayCommand CancelCommand { get; }
    public RelayCommand CheckoutCommand { get; }

    private void SetupRemoveFromCartCommand()
    {
        if (_selectedCartItem.Amount < _amount)
        {
            MessageBox.Show("The amount you've chosen is greater than the amount in your cart!", "AMOUNT ERROR",
                MessageBoxButton.OK);
            return;
        }
        _userManager.CurrentUser.UpdateOrDeleteStockItem(new ProductContainer(_selectedCartItem.Product, _amount));
        _shopManager.CurrentShop.UpsertStockItem(new ProductContainer(_selectedCartItem.Product, _amount));

        _productStock = new ObservableCollection<ProductContainer>(_shopManager.CurrentShop?.ProductStock ?? throw new InvalidOperationException());
        _shoppingCart = new ObservableCollection<ProductContainer>(_userManager.CurrentUser?.ShoppingCart ?? throw new InvalidOperationException());

        OnPropertyChanged(nameof(ProductStock));
        OnPropertyChanged(nameof(ShoppingCart));

        _productAddMode = false;
        AddToStockCommand.NotifyCanExecuteChanged();
    }

    private void SetupAddToCartCommand()
    {
        if (_selectedStockItem.Amount < _amount)
        {
            MessageBox.Show("The amount you've chosen is greater than the amount in stock!", "AMOUNT ERROR",
                MessageBoxButton.OK);
            return;
        }
        _shopManager.CurrentShop.UpdateOrDeleteStockItem(new ProductContainer(_selectedStockItem.Product, _amount));
        _userManager.CurrentUser.UpsertStockItem(new ProductContainer(_selectedStockItem.Product, _amount));

        _productStock = new ObservableCollection<ProductContainer>(_shopManager.CurrentShop?.ProductStock ?? throw new InvalidOperationException());
        _shoppingCart = new ObservableCollection<ProductContainer>(_userManager.CurrentUser?.ShoppingCart ?? throw new InvalidOperationException());

        OnPropertyChanged(nameof(ProductStock));
        OnPropertyChanged(nameof(ShoppingCart));

        _productAddMode = false;
        AddToStockCommand.NotifyCanExecuteChanged();
    }

    private void SetupAddToStockCommand()
    {
        _shopManager.CurrentShop.UpsertStockItem(new ProductContainer(_selectedProduct, _amount));
        _shopManager.Replace(_shopManager.CurrentShop.Id, _shopManager.CurrentShop);

        _productStock = new ObservableCollection<ProductContainer>(_shopManager.CurrentShop?.ProductStock ?? throw new InvalidOperationException());
        OnPropertyChanged(nameof(ProductStock));
    }

    private void SetupCancelCommand()
    {
        _navigationStore.CurrentViewModel = new ShopSelectViewModel(_navigationStore, _userManager);
    }

    private void SetupCheckoutCommand()
    {
        _shopManager.Replace(_shopManager.CurrentShop.Id, _shopManager.CurrentShop);
        _orderManager.Add(new Order(_shopManager.CurrentShop.Id, _userManager.CurrentUser.Id, _shoppingCart));

        _userManager.CurrentUser.ShoppingCart = new List<ProductContainer>();

        MessageBox.Show("Order successfully created!", "ORDER SUCCESS", MessageBoxButton.OK);
        _navigationStore.CurrentViewModel = new MainMenuViewModel(_navigationStore, _userManager);
    }

}