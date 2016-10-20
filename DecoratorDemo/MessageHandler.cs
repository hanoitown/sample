using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorDemo
{
    public interface IHandle<T> { void Handle(T message); }

    public class CompositeMessageHandler<T> : IHandlerDecorator<T>
    {
        private readonly IEnumerable<IHandle<T>> handlers;

        CompositeMessageHandler(IEnumerable<IHandle<T>> handlers)
        {
            this.handlers = handlers;
        }

        public void Handle(T message)
        {
            foreach (var handler in this.handlers)
            {
                handler.Handle(message);
            }
        }
    }
    public interface IHandlerDecorator<T> : IHandle<T> { }
    public class HandlerDecorator<T> : IHandlerDecorator<T>
    {
        public readonly IEnumerable<IHandle<T>> Handlers;

        public HandlerDecorator(IEnumerable<IHandle<T>> handler)
        {
            Handlers = handler;
        }

        public void Handle(T message)
        {
            foreach (var handler in Handlers)
                handler.Handle(message);
        }
    }

    public class IntHandlerOne : IHandle<int>
    {
        public void Handle(int message)
        {
            Console.WriteLine(1.ToString());
        }
    }
    public class IntHandlerTwo : IHandle<int>
    {
        public void Handle(int message) { Console.WriteLine(2.ToString()); }
    }
    public class IntHandlerNine : IHandle<int>
    {
        public void Handle(int message) { Console.WriteLine(9.ToString()); }
    }
    public class StringHandler : IHandle<string>
    {
        public void Handle(string message) { Console.WriteLine("HEHE"); }
    }
}
