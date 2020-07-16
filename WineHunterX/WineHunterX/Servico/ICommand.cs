using System;
namespace WineHunterX.Servico
{
    public interface ICommand
    {
        void Execute(object arg);
        bool CanExecute(object arg);
        event EventHandler CanExecuteChanged; 
    }
}
