using System;
using CommunityToolkit.Mvvm.Input;

namespace Databaser_Labb_3___Pontus.Commands;

public class ConfirmProductEditCommand : IRelayCommand // TODO: Kanske implementera?
{
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        throw new NotImplementedException();
    }

    public event EventHandler? CanExecuteChanged;
    public void NotifyCanExecuteChanged()
    {
        throw new NotImplementedException();
    }
}