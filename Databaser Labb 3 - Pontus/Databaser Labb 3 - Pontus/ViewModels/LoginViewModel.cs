using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess.Managers;
using DataAccess.Models;
using Databaser_Labb_3___Pontus.Stores;

namespace Databaser_Labb_3___Pontus.ViewModels;

public class LoginViewModel : ObservableObject
{
    private readonly NavigationStore _navigationStore;
    private readonly UserManager _userManager;

    public LoginViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _userManager = new UserManager();
        LoginCommand = new RelayCommand(SetupLoginCommand, CanExecuteRegisterAndLogin);
        RegisterCommand = new RelayCommand(SetupRegisterCommand, CanExecuteRegisterAndLogin);
        ExitCommand = new RelayCommand(() => Environment.Exit(0));
    }

    private string? _userName;

    public string? UserName
    {
        get => _userName;
        set
        {
            SetProperty(ref _userName, value);
            LoginCommand.NotifyCanExecuteChanged();
            RegisterCommand.NotifyCanExecuteChanged();
        }
    }

    private string? _password;

    public string? Password
    {
        get => _password;
        set
        {
            SetProperty(ref _password, value);
            LoginCommand.NotifyCanExecuteChanged();
            RegisterCommand.NotifyCanExecuteChanged();
        }
    }

    public RelayCommand LoginCommand { get; }
    public RelayCommand RegisterCommand { get; }
    public RelayCommand ExitCommand { get; }

    private void SetupLoginCommand()
    {
        var loginUser = _userManager.GetUserByUserName(_userName);
        if (loginUser is null || loginUser.Password != _password)
        {
            MessageBox.Show("Incorrect username or password!", "LOGIN ERROR", MessageBoxButton.OK);
            return;
        }
        _userManager.CurrentUser = loginUser;
        _navigationStore.CurrentViewModel = new MainMenuViewModel(_navigationStore, _userManager);
    }

    private void SetupRegisterCommand()
    {
        if (_userManager.GetUserByUserName(_userName) is not null)
        {
            MessageBox.Show("A user with that username already exists!", "USERNAME ALREADY EXISTS", MessageBoxButton.OK);
            return;
        }

        _userManager.Add(new User(_userName, _password));
        MessageBox.Show("User succesfully registered!", "USER REGISTERED", MessageBoxButton.OK);
        _userName = string.Empty;
        OnPropertyChanged(nameof(UserName));
        _password = string.Empty;
        OnPropertyChanged(nameof(Password));
    }

    private bool CanExecuteRegisterAndLogin()
    {
        return !string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_password);
    }
}