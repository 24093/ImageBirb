using System.Windows.Input;

namespace ImageBirb.ViewModels
{
    internal static class CommandExtensions
    {
        /// <summary>
        /// Executes a command only if it can be executed (CanExecute).
        /// </summary>
        public static void Exec(this ICommand command, object parameter)
        {
            if (command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }
    }

    internal class AsyncRelayCommand
    {

    }
}