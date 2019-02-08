using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageBirb.Common
{
    internal static class CommandExtensions
    {
        public static void Exec(this ICommand command, object parameter = null)
        {
            if (command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        public static void Exec<T>(this ICommand command, T parameter)
        {
            if (command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }
    }
}