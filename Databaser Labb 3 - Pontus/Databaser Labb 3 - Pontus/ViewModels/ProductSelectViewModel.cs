using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess.Managers;
using DataAccess.Models;
using Databaser_Labb_3___Pontus.Stores;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Databaser_Labb_3___Pontus.ViewModels;

public class ProductSelectViewModel : ObservableObject
{
    private readonly NavigationStore _navigationStore;
    private readonly UserManager _userManager;
    private readonly ProductManager _productManager;

    public ProductSelectViewModel(NavigationStore navigationStore, UserManager userManager)
    {
        _navigationStore = navigationStore;
        _userManager = userManager;
        _productManager = new ProductManager();
        _productList = new ObservableCollection<Product>(_productManager.GetAll());

        CreateCommand = new RelayCommand(() =>
        {
            _productManager.CurrentProduct = null;
            _navigationStore.CurrentViewModel =
                new ProductEditorViewModel(_navigationStore, _userManager, _productManager);
        });

        EditCommand = new RelayCommand(() =>
        {
            _productManager.CurrentProduct = _selectedProduct;
            _navigationStore.CurrentViewModel =
                new ProductEditorViewModel(_navigationStore, _userManager, _productManager);
        },
            () => _selectedProduct is not null);

        DeleteCommand = new RelayCommand(SetupDeleteCommand,
            () => _selectedProduct is not null);

        ExitCommand = new RelayCommand(() =>
            _navigationStore.CurrentViewModel = new MainMenuViewModel(_navigationStore, _userManager));
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
            EditCommand.NotifyCanExecuteChanged();
            DeleteCommand.NotifyCanExecuteChanged();
        }
    }

    public RelayCommand CreateCommand { get; }
    public RelayCommand EditCommand { get; }
    public RelayCommand DeleteCommand { get; }
    public RelayCommand ExitCommand { get; }

    private void SetupDeleteCommand() // TODO: Radera även dokument från andra collections (förutom i orders!!!)
    {
        _productManager.Delete(_selectedProduct!.Id);
        _productList.Remove(_selectedProduct);
        OnPropertyChanged(nameof(ProductList));
        EditCommand.NotifyCanExecuteChanged();
        DeleteCommand.NotifyCanExecuteChanged();
    }
}