using System;
using System.Windows.Input;

namespace SongList2.Commands
{
    internal class RelayCommand : ICommand
    {
        private readonly Action m_execute;
        private readonly Func<bool>? m_canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            m_execute = execute;
            m_canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) 

            => m_canExecute == null || m_canExecute();

        public void Execute(object? parameter) 
            => m_execute();

        public event EventHandler? CanExecuteChanged { add { } remove { } }
    }
}
