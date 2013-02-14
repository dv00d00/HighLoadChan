namespace HighLoadChan.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using HighLoadChan.Core;

    public class SingleMachineMessanger : IMessanger
    {
        private readonly Dictionary<Type, List<dynamic>> commandHandlers = new Dictionary<Type, List<dynamic>>();
        private readonly Dictionary<Type, List<dynamic>> eventHandlers = new Dictionary<Type, List<dynamic>>();

        public SingleMachineMessanger()
        {
        }

        public void RegisterCommandHandler<T>(ICommandHandler<T> commandHandler) where T : ICommand
        {
            this.AddSubscription(this.commandHandlers, typeof(T), commandHandler);
        }

        public void RegisterEventHandler<T>(IEventHandler<T> eventHandler) where T : IEvent
        {
            this.AddSubscription(this.eventHandlers, typeof(T), eventHandler);
        }

        public void SendCommand<T>(T command) where T : ICommand
        {
            var handlers = this.commandHandlers[typeof(T)].OfType<ICommandHandler<T>>().ToList();
            handlers.ForEach(it => it.HandleCommand(command));
        }

        public void SendEvent<T>(T @event) where T : IEvent
        {
            var handlers = this.eventHandlers[typeof(T)].OfType<IEventHandler<T>>().ToList();
            handlers.ForEach(it => it.HandleEvent(@event));
        }

        private void AddSubscription(Dictionary<Type, List<dynamic>> subscriptions, Type key, dynamic value)
        {
            if (!subscriptions.ContainsKey(key))
            {
                subscriptions[key] = new List<dynamic>();
            }

            subscriptions[key].Add(value);
        }
    }

    class SingleMachineMessangerImpl : SingleMachineMessanger
    {
    }
}