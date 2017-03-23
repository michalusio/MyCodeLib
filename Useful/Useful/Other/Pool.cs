using System.Collections.Generic;

namespace Useful.Other
{
    public class Pool
    {
        private readonly List<int> _openpool;
        private readonly HashSet<int> _takenpool;
        private int _max;
        public int Step;

        public Pool()
        {
            _takenpool = new HashSet<int>();
            _openpool = new List<int>();
            for (var i = 0; i < Step; i++)
                _openpool.Add(i);
            _max = Step;
        }

        public int Take()
        {
            if (_openpool.Count < 10)
            {
                for (var i = _max; i < _max + Step; i++)
                    _openpool.Add(i);
                _max += Step;
            }
            var a = _openpool[0];
            _openpool.RemoveAt(0);
            _takenpool.Add(a);
            return a;
        }

        public void Free(int i)
        {
            if (_takenpool.Contains(i))
            {
                _takenpool.Remove(i);
                var index = 0;
                for (var j = 0; j < _openpool.Count; j++)
                {
                    index = j;
                    if (_openpool[j] > i) break;
                }
                _openpool.Insert(index, i);
            }
        }
    }
}