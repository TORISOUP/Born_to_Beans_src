using System.Linq;
using UniRx;
using UnityEngine;

namespace GGJ.Utils
{
    public static class FPSCounter
    {
        private const int BufferSize = 10;
        public static IReadOnlyReactiveProperty<float> Current { get; private set; }

        static FPSCounter()
        {
            Current = Observable.EveryUpdate()
                .Select(_ => Time.deltaTime)
                .Buffer(BufferSize, 1)
                .Select(x => 1.0f / x.Average())
                .ToReadOnlyReactiveProperty();
        }
    }
}
