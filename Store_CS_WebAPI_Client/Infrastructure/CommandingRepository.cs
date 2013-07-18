using System;
using System.Windows.Input;

namespace Store_CS_WebAPI_Client.Infrastructure
{

    /// <summary>
    /// The Command Object for performing Operation for methods with input parameter and out parameter
    /// </summary>
    public class RoutedCommand : ICommand
    {

        private Action<object>  _handlerExecution;
        private Func<object, bool> _canHandleExecution;

        public RoutedCommand(Action<object> h ,Func<object,bool> c=null)
        {
            _handlerExecution = h;
            _canHandleExecution = c;
        }

        public bool CanExecute(object parameter)
        {
            return (_canHandleExecution == null) || _canHandleExecution.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _handlerExecution.Invoke(parameter);
        }
    }
}
