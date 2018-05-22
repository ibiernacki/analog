using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;

namespace ViewModels.Modules
{
    public class ReactiveCollection<T> : IReactiveCollection<T>
    {
        private readonly List<T> _collectionState = new List<T>();

        private readonly Subject<IList<T>> _added = new Subject<IList<T>>();
        private readonly Subject<IList<T>> _removed = new Subject<IList<T>>();
        private readonly BehaviorSubject<IList<T>> _changed;

        public IObservable<IList<T>> Added => _added;
        public IObservable<IList<T>> Removed => _removed;
        public IObservable<IList<T>> Changed => _changed;
        public void AddRange(IEnumerable<T> items)
        {
            _collectionState.AddRange(items);
            _added.OnNext(items.ToArray());
            _changed.OnNext(_collectionState.ToArray());
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                _collectionState.Remove(item);
            }
            _removed.OnNext(items.ToArray());
            _changed.OnNext(_collectionState.ToArray());
        }

        public ReactiveCollection()
        {
            _changed = new BehaviorSubject<IList<T>>(_collectionState);
        }


        public IEnumerator<T> GetEnumerator()
        {
            return _collectionState.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collectionState.GetEnumerator();
        }

        public void Add(T item)
        {
            _collectionState.Add(item);
            _added.OnNext(new[] { item });
            _changed.OnNext(_collectionState.ToArray());
        }

        public void Clear()
        {
            _collectionState.Clear();
            _removed.OnNext(_collectionState.ToArray());
            _changed.OnNext(_collectionState.ToArray());

        }

        public bool Contains(T item)
        {
            return _collectionState.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _collectionState.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var result = _collectionState.Remove(item);
            if (!result)
                return false;
            _removed.OnNext(new[] { item });
            _changed.OnNext(_collectionState.ToArray());
            return true;
        }

        public int Count => _collectionState.Count;
        public bool IsReadOnly => false;
        public int IndexOf(T item)
        {
            return _collectionState.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _collectionState.Insert(index, item);
            _added.OnNext(new[] { item });
            _changed.OnNext(_collectionState.ToArray());

        }

        public void RemoveAt(int index)
        {
            var element = _collectionState.ElementAt(index);
            _collectionState.RemoveAt(index);
            _removed.OnNext(new[] { element });
            _changed.OnNext(_collectionState.ToArray());
        }

        public T this[int index]
        {
            get => _collectionState[index];
            set => _collectionState[index] = value;
        }

    }
}