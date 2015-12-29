using DotNetRevolution.Core.Base;
using System;

namespace DotNetRevolution.Core.Logging
{
    public class TemporaryConsoleColor : Disposable
    {
        private readonly ConsoleColor _background;
        private readonly ConsoleColor _foreground;
        
        public TemporaryConsoleColor(ConsoleColor foreground)
            : this(Console.BackgroundColor, foreground)
        {
        }

        public TemporaryConsoleColor(ConsoleColor background, ConsoleColor foreground)
        {
            _background = Console.BackgroundColor;
            _foreground = Console.ForegroundColor;

            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            Console.ForegroundColor = _foreground;
            Console.BackgroundColor = _background;
        }
    }
}