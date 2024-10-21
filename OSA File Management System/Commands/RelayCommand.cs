using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;



namespace OSA_File_Management_System.Commands
{
    internal class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private readonly Action<object?> DoWorkWithParameter; // Action with parameter
        private readonly Action DoWorkWithoutParameter; // Action without parameter

        // Constructor for commands that do not take parameters
        public RelayCommand(Action work)
        {
            DoWorkWithoutParameter = work;
        }

        // Constructor for commands that take parameters
        public RelayCommand(Action<object> work)
        {
            DoWorkWithParameter = work;
        }

        public bool CanExecute(object? parameter)
        {
            return true; // You can implement logic to enable/disable commands
        }

        public void Execute(object? parameter)
        {
            // Check if there's a parameterized method and call the appropriate one
            if (DoWorkWithParameter != null)
            {
                DoWorkWithParameter(parameter); // Call the parameterized method
            }
            else
            {
                DoWorkWithoutParameter(); // Call the non-parameterized method
            }
        }
    }
}
