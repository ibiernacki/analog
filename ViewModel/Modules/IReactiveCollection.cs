using System;
using System.Collections.Generic;

namespace ViewModels.Modules
{
    public interface IReactiveCollection<T> : IList<T>
    {
        IObservable<IList<T>> Added { get; }
        IObservable<IList<T>> Removed { get; }
        IObservable<IList<T>> Changed { get; }

        void AddRange(IEnumerable<T> items);
        void RemoveRange(IEnumerable<T> items);
    }
}