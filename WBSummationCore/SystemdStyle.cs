using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WBSummationCore
{
    class SystemdStyle
    {
        private string _message;

        public SystemdStyle(string message)
        {
            _message = message;
        }

        public void showMessage()
        {
            Console.Write("[    ] ");
            Console.Write(_message);
        }

        public void okMessage()
        {
            clearMessage();
            Console.Write("[ ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("OK");
            Console.ResetColor();
            Console.Write(" ] ");
            Console.WriteLine(_message);
        }

        public void failMessage()
        {
            clearMessage();
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("FAIL");
            Console.ResetColor();
            Console.Write("] ");
            Console.WriteLine(_message);
        }
        
        private void clearMessage()
        {
            Console.Write(new string('\b', 7 + _message.Length));
        }

        
    }
}
