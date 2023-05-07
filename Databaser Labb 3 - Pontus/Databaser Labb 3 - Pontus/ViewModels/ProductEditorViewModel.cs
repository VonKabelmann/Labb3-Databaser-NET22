using System;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess.Managers;
using DataAccess.Models;
using Databaser_Labb_3___Pontus.Stores;

namespace Databaser_Labb_3___Pontus.ViewModels;

public class ProductEditorViewModel : ObservableObject
{
    private readonly NavigationStore _navigationStore;
    private readonly UserManager _userManager;
    private readonly ProductManager _productManager;
    private readonly ShopManager _shopManager;
    public ProductEditorViewModel(NavigationStore navigationStore, UserManager userManager, ProductManager productManager)
    {
        _navigationStore = navigationStore;
        _userManager = userManager;
        _productManager = productManager;
        _shopManager = new ShopManager();

        if (_productManager.CurrentProduct is not null)
        {
            _productName = _productManager.CurrentProduct.ProductName;
            _price = _productManager.CurrentProduct.Price;
        }
        else
        {
            _price = 13.37;
        }

        ConfirmCommand = new RelayCommand(SetupConfirmCommand, CanExecuteConfirm);
        CancelCommand = new RelayCommand(() =>
            _navigationStore.CurrentViewModel = new ProductSelectViewModel(_navigationStore, _userManager));
    }
    private string? _productName;

    public string? ProductName
    {
        get => _productName;
        set
        {
            SetProperty(ref _productName, value);
            ConfirmCommand.NotifyCanExecuteChanged();
        }
    }

    private double _price;

    public double Price
    {
        get => _price;
        set => SetProperty(ref _price, value);
    }
    public RelayCommand ConfirmCommand { get; }
    public RelayCommand CancelCommand { get; }

    private void SetupConfirmCommand()                  // TODO: Redigera i alla collections utom Orders???
    {
        if (_productManager.CurrentProduct is null)     // Så ser vi att vi kör create och inte edit
        {
            if (_productManager.GetAll().Any(p => p.ProductName.Equals(_productName))) // Samma namn existerar redan
            {
                MessageBox.Show("Product with chosen name already exists!", "PRODUCT NAME ERROR", MessageBoxButton.OK);
            }
            else                                        // Lägger till ny
            {
                _productManager.Add(new Product(_productName, _price));
                MessageBox.Show("Product successfully added!");
                _navigationStore.CurrentViewModel = new ProductSelectViewModel(_navigationStore, _userManager);
            }
        }
        else if ((_productManager.GetAll().Any(p => p.ProductName.Equals(_productName))         // Kollar ifall namnet finns, men också ifall den nu finns är det samma ID?
                 && _productManager.GetAll().First(p => p.Id.Equals(_productManager.CurrentProduct.Id)).ProductName == _productName)    // Isåfall låt redigeringen ske
                 || !_productManager.GetAll().Any(p => p.ProductName.Equals(_productName)))
        {
            _productManager.Replace(_productManager.CurrentProduct.Id, new Product(_productName, _price));
            
            var product = _productManager.CurrentProduct;
            product.ProductName = _productName;
            product.Price = _price;
            _shopManager.EditProductInAllShops(product);

            MessageBox.Show("Product successfully edited!", "EDIT SUCCESS", MessageBoxButton.OK);
            _navigationStore.CurrentViewModel = new ProductSelectViewModel(_navigationStore, _userManager);
        }
        else
        {
            MessageBox.Show("Product with chosen name already exists!", "PRODUCT NAME ERROR", MessageBoxButton.OK);
        }
    }
    private bool CanExecuteConfirm()
    {
        return !string.IsNullOrEmpty(_productName);
    }
}