using System;
using System.Collections;
using System.Collections.Generic;

namespace YaEcs
{
    public partial class Components
    {
        private struct ComponentEnumerator : IEnumerator<KeyValuePair<uint, IComponent>>
        {
            private readonly Components source;
            private IEnumerator<KeyValuePair<Type, Dictionary<uint, IComponent>>> typeEnumerator;
            private IEnumerator<KeyValuePair<uint, IComponent>>? componentEnumerator;

            public ComponentEnumerator(Components components)
            {
                source = components;
                Current = default;
                componentEnumerator = default;
                typeEnumerator = source.storage.GetEnumerator();
                ;
            }

            public bool MoveNext()
            {
                if (componentEnumerator != null && componentEnumerator.MoveNext())
                {
                    Current = componentEnumerator.Current;
                    return true;
                }

                if (typeEnumerator.MoveNext())
                {
                    componentEnumerator = typeEnumerator.Current.Value.GetEnumerator();
                    if (componentEnumerator.MoveNext())
                    {
                        Current = componentEnumerator.Current;
                        return true;
                    }
                }

                return false;
            }

            public void Reset()
            {
                Current = default;
                componentEnumerator = default;
                typeEnumerator.Dispose();
                typeEnumerator = source.storage.GetEnumerator();
            }

            public KeyValuePair<uint, IComponent> Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                typeEnumerator.Dispose();
                componentEnumerator?.Dispose();
            }
        }
    }
}