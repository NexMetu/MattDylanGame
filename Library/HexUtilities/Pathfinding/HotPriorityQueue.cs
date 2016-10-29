﻿#region The MIT License - Copyright (C) 2012-2013 Pieter Geerkens
/////////////////////////////////////////////////////////////////////////////////////////
//                PG Software Solutions Inc. - Hex-Grid Utilities
/////////////////////////////////////////////////////////////////////////////////////////
// The MIT License:
// ----------------
// 
// Copyright (c) 2012-2013 Pieter Geerkens (email: pgeerkens@hotmail.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, 
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to the following 
// conditions:
//     The above copyright notice and this permission notice shall be 
//     included in all copies or substantial portions of the Software.
// 
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//     EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//     OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
//     NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
//     HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
//     WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
//     FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
//     OTHER DEALINGS IN THE SOFTWARE.
/////////////////////////////////////////////////////////////////////////////////////////
#endregion
using System.Collections.Generic;
using System.Linq;

namespace PGNapoleonics.HexUtilities.Pathfinding {
  /// <summary>Heap-On-Top (HOT) Priority Queue implementation with a key of type <c>int</c>.</summary>
  /// <typeparam name="TValue">Type of the queue-item value.</typeparam>
  /// <remarks>
  /// 
  /// </remarks>
  /// <a href="http://en.wikipedia.org/wiki/Heapsort">Wikepedia - Heapsort</a>/>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", 
    "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
  public sealed class HotPriorityQueue<TValue> : IPriorityQueue<int,TValue> {

    int _baseIndex;
    int  _shift;
    IPriorityQueue<int, TValue>                        _queue;
    IDictionary<int, HotPriorityQueueList<int,TValue>> _lists;

    /// <summary>TODO</summary>
    public HotPriorityQueue() : this(0) {}
    /// <summary>TODO</summary>
    public HotPriorityQueue(int shift) { 
      const int initialSize = 2048;
      PoolSize   = initialSize * 7/8;
      _baseIndex = 0;
      _shift     = shift; 
      _queue     = new HotPriorityQueueList<int,TValue>(initialSize).PriorityQueue;
    }

    /// <inheritdoc/>
    public bool Any()    { return _queue.Any()  ||  (_lists!=null && _lists.Any()); }

    /// <inheritdoc/>
    public int  Count    { get { return _queue.Count; } }

    /// <summary>The number of elements which are handled by a straight HeapPriorityQueue.</summary>
    /// <remarks>
    /// When the number of elements exceeds this value, additional lists are created 
    /// to handle the overflow elements of lower priority (higher <c>TKey</c> values.
    /// </remarks>
    public int  PoolSize { get; set; }

    /// <inheritdoc/>
    public void Enqueue(int priority, TValue value) {
      Enqueue(new HexKeyValuePair<int,TValue>(priority,value));
    }
    /// <inheritdoc/>
    public void Enqueue(HexKeyValuePair<int,TValue> item) {
      var index = item.Key >> _shift;
      if (index <= _baseIndex) {
        _queue.Enqueue(item);
      } else if (_lists == null && _queue.Count < PoolSize) {
        _baseIndex = index;
        _queue.Enqueue(item);
      } else {
        if (_lists == null) {
#if UseSortedDictionary
          _lists = new SortedDictionary<ushort, HotPriorityQueueList<PathPriority, TValue>>();
#else
          _lists = new SortedList<int, HotPriorityQueueList<int,TValue>>();
#endif
        }
        HotPriorityQueueList<int,TValue> list;
        if( ! _lists.TryGetValue(index, out list) ) {
          list = new HotPriorityQueueList<int,TValue>();
          _lists.Add(index, list);
        }
        list.Add(item);
      }
    }

    /// <inheritdoc/>
    public bool TryDequeue(out HexKeyValuePair<int,TValue> result) {
      if (_queue.TryDequeue(out result))  {
        return true;
      } else {
        var list   = _lists.First();
        _baseIndex = list.Key;
        _queue     = list.Value.PriorityQueue;
        _lists.Remove(list.Key);

        return _queue.TryDequeue(out result);
      }
    }

    /// <inheritdoc/>
    public bool TryPeek(out HexKeyValuePair<int,TValue> result) {
      if (_queue.TryPeek(out result))  {
        return true;
      //} else if (_lists==null) {
      //  return false;
      } else {
        var list   = _lists.First();
        _baseIndex = list.Key;
        _queue     = list.Value.PriorityQueue;
        _lists.Remove(list.Key);

        return _queue.TryPeek(out result);
      }
    }
  }
}
