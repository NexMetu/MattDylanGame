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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using PGNapoleonics.HexUtilities.Common;

/// <summary>A fast efficient serial implementation of <b>Bidirectional ALT</b> (<b>A*</b> with <b>L</b>andmarks and <b>T</b>riangle-inequality heuristic) <b>path-finding</b>
/// on a <see cref="Hexgrid"/> map.</summary>
namespace PGNapoleonics.HexUtilities.Pathfinding {
  /// <summary>C# (serial) implementation of NPBA* path-finding algorithm by Pijls &amp; Post (Adapted).</summary>
  /// <remarks>Adapted to hex-grids, and to weight the most direct path favourably for better (visual) 
  /// behaviour on a hexgrid.</remarks>
  public class BidirectionalPathfinder {
    /// <summary>Returns an <c>IPath</c> for the optimal path from coordinates <c>start</c> to <c>goal</c>.</summary>
    /// <param name="start">Coordinates for the <c>last</c> step on the desired path.</param>
    /// <param name="goal">Coordinates for the <c>first</c> step on the desired path.</param>
    /// <param name="board">An object satisfying the interface <c>INavigableBoardFwd</c>.</param>
    /// <remarks>Note that <c>heuristic</c> <b>must</b> be monotonic in order for the algorithm to perform 
    /// properly.</remarks>
    /// See also: <a href="http://www.cs.trincoll.edu/~ram/cpsc352/notes/astar.html">A* Algorithm Notes</a>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",   
      "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public static IDirectedPath FindDirectedPathFwd(
      IHex start,  IHex goal,  IDirectedNavigableBoard board
    ) {
      if (board==null) throw new ArgumentNullException("board"); 
      return FindDirectedPath(start, goal, board.DirectedStepCost, board.Landmarks).PathFwd;
    }

    /// <summary>As <c>FindDirectedPathFwd</c>, except with the steps stacked in reverse.</summary>
    /// <remarks>
    /// The path steps are ordered in reverse as the forward half-path has been stacked 
    /// onto the reverse half-path during post-processing, instead of the reverse.
    /// </remarks>
    /// <see cref="FindDirectedPathFwd"/>
    public static IDirectedPath FindDirectedPathRev(
      IHex start,  IHex goal,  IDirectedNavigableBoard board
    ) {
      if (board==null) throw new ArgumentNullException("board"); 
      return FindDirectedPath(start, goal, board.DirectedStepCost, board.Landmarks).PathRev;
    }

    /// <summary>Returns an <c>IPath</c> for the optimal path from coordinates <c>start</c> to <c>goal</c>.</summary>
    /// <param name="start">Coordinates for the <c>last</c> step on the desired path.</param>
    /// <param name="goal">Coordinates for the <c>first</c> step on the desired path.</param>
    /// <param name="stepCost">Cost to extend path by hex at <c>coords</c> from hex at direction <c>hexside</c>.</param>
    /// <param name="landmarks"><c>LandmarkCollection</c> of landmarks available for heuristic calculation.</param>
    static PathHalves FindDirectedPath (
      IHex start,  IHex goal,
      Func<IHex, Hexside, int> stepCost,
      LandmarkCollection       landmarks
    ) {
      if (start==null)      throw new ArgumentNullException("start"); 
      if (goal==null)       throw new ArgumentNullException("goal"); 
      if (stepCost==null)   throw new ArgumentNullException("stepCost"); 
      if (landmarks==null)  throw new ArgumentNullException("landmarks");

      TraceFlags.FindPathDetail.Trace(true, 
        "Fwd: Find path from {0} to {1}:", start.Coords, goal.Coords);

      var closed        = new HashSet<HexCoords>();
      var pathHalves    = new PathHalves();

      var pathfinderFwd = new PathfinderFwd(start, goal, stepCost, landmarks,
                          closed, pathHalves.SetBestSoFar, ()=>pathHalves.BestSoFar);
      var pathfinderRev = new PathfinderRev(start, goal, stepCost, landmarks,
                          closed, pathHalves.SetBestSoFar, ()=>pathHalves.BestSoFar);

      pathfinderFwd.Partner = pathfinderRev;
      var pathfinder        = pathfinderRev.Partner
                            = pathfinderFwd;
      while (! pathfinder.IsFinished())  pathfinder = pathfinder.Partner; 

      return pathHalves;
    }

    /// <summary>TODO</summary>
    /// <param name="vectorGoal"></param>
    /// <param name="vectorHex"></param>
    /// <returns></returns>
    static int  Preference(IntVector2D vectorGoal, IntVector2D vectorHex) {
      return (0xFFFF & Math.Abs(vectorGoal ^ vectorHex ));
    }

    BidirectionalPathfinder   Partner  { get; set; }

    DirectedPath GetPartnerPath(HexCoords coords) {
      DirectedPath path;
      _open.TryGetValue(coords,out path);
      return path;
    }

    int  Estimate(IHex here, IHex there, int totalCost) {
      var estimate   = _heuristic(there.Coords) + totalCost;
      var preference = Preference(_start.Coords.Canon - there.Coords.Canon,
                                  _start.Coords.Canon - _goal.Coords.Canon );
      return (estimate << 16) + preference;
    }
    int  FrontierMinimum() { 
      HexKeyValuePair<int,DirectedPath> item;
      return (_queue.TryPeek(out item) ? item.Key >> 16 : (Int32)Int16.MaxValue); 
    }
    int  Heuristic(HexCoords coords) { return _heuristic(coords);  }

    bool IsFinished() {
      HexKeyValuePair<int,DirectedPath> item;
      if ( ! _queue.TryDequeue(out item)) return true;

      var path   = item.Value;
      var coords = path.PathStep.Hex.Coords;

      _open.Remove(coords);
      if( ! _closed.Contains(coords) ) {

        TraceFlags.FindPathDequeue.Trace(
          "{0} Dequeue Path at {1} w/ cost={2,4} at {3,-9}; estimate={4,4}:{5,4}.", 
          _search, coords, path.TotalCost, path.HexsideExit, item.Key >> 16, item.Key & 0xFFFF);

        if (item.Key>>16 < _getBestSoFar()
        &&  path.TotalCost + Partner.FrontierMinimum() - Partner.Heuristic(coords) 
                         < _getBestSoFar()
        ) {
          var hexsides = HexsideExtensions.HexsideList;
          for (var index = 0; index < hexsides.Count; index++) {
            ExpandHex(path, hexsides[index]);
          }
        }
        _closed.Add(coords);
      }
      return ! _queue.Any();
    }
    void ExpandHex(DirectedPath path, Hexside hexside) {
      var here  = path.PathStep.Hex;
      var there = here.Neighbour(hexside);
      if (there != null  &&  ! _closed.Contains(there.Coords) ) {
        var cost = _stepCost(here, hexside, there);
        if( (cost > 0)
        ) {
          if( (path.TotalCost+cost < _getBestSoFar()  ||  ! _open.ContainsKey(there.Coords))
          ) {
            var totalCost = cost + path.TotalCost;
            var key       = Estimate(here, there, totalCost);
            var newPath   = _addStep(path, there, hexside, cost);

            TraceFlags.FindPathEnqueue.Trace("   Enqueue {0}: estimate={1,4}:{2,4}",
                                          there.Coords, key>>16, key & 0xFFFF);

            DirectedPath oldPath;
            if ( ! _open.TryGetValue(there.Coords, out oldPath))  {
              _open.Add(there.Coords, newPath);
              _queue.Enqueue(key, newPath);
            } else if (newPath.TotalCost < oldPath.TotalCost) {
              _open.Remove(there.Coords);
              _open.Add(there.Coords, newPath);
              _queue.Enqueue(key, newPath);
            }

            _setBestSoFar(newPath, Partner.GetPartnerPath(there.Coords));
          }
        }
      }
    }

    #region Fields
    HashSet<HexCoords>                 _closed;
    Func<int>                          _getBestSoFar;
    IHex                               _goal;
    Func<HexCoords,int>                _heuristic;
    Landmark                           _landmark;
    Dictionary<HexCoords,DirectedPath> _open;
    HotPriorityQueue<DirectedPath>     _queue;
    string                             _search;
    IHex                               _start;
    IntVector2D                        _vectorGoal;

    internal  Func<DirectedPath,IHex,Hexside,int,DirectedPath> _addStep;
    internal  Action<DirectedPath,DirectedPath>                _setBestSoFar;
    internal  Func<IHex, Hexside, IHex, int>                   _stepCost;
    #endregion

    private sealed class PathHalves {
      public PathHalves() { BestSoFar   = (Int32)Int16.MaxValue; }

      public int          BestSoFar { get; private set; }
      public int          KeySoFar  { get; private set; }
      public DirectedPath PathFwd   { get { return MergePaths(_pathFwd, _pathRev); } }
      public DirectedPath PathRev   { get { return MergePaths(_pathRev, _pathFwd); } }

      public void SetBestSoFar(DirectedPath pathFwd, DirectedPath pathRev) {
        if (pathFwd==null  ||  pathRev==null) return;
        if( pathFwd.TotalCost + pathRev.TotalCost < BestSoFar) {
          _pathFwd  = pathFwd; 
          _pathRev  = pathRev; 
          BestSoFar = _pathFwd.TotalCost + pathRev.TotalCost;
          KeySoFar  = _pathFwd.Key + _pathRev.Key;
          TraceFlags.FindPathDetail.Trace("   SetBestSoFar: pathFwd at {0}; pathRev at {1}; Cost = {2}",
              pathFwd.PathStep.Hex.Coords, _pathRev.PathStep.Hex.Coords, BestSoFar);
        }
      }

      public static DirectedPath MergePaths(DirectedPath fwd, DirectedPath rev) {
        if (rev!=null) {
          while (rev.PathSoFar!=null) {
            var hexside = rev.PathStep.HexsideEntry;
            var cost    = rev.TotalCost - (rev = rev.PathSoFar).TotalCost;
            fwd = fwd.AddStep(rev.PathStep.Hex, hexside, cost, cost);
          }
        }
        return fwd;
      }

      DirectedPath _pathFwd;
      DirectedPath _pathRev;
    }

    BidirectionalPathfinder(IHex start, IHex goal,
      LandmarkCollection landmarks, HashSet<HexCoords> closed, Func<int> getBestSoFar
    ) {
      _start        = start;
      _goal         = goal;
      _getBestSoFar = getBestSoFar;
      _vectorGoal   = goal.Coords.Canon - start.Coords.Canon;
      _open         = new Dictionary<HexCoords, DirectedPath>();
      _closed       = closed;
      _queue        = new HotPriorityQueue<DirectedPath>(16);
      _landmark     = landmarks
              .OrderByDescending(l => l.HexDistance(goal.Coords) - l.HexDistance(start.Coords))
              .FirstOrDefault();
      _heuristic    = c => _landmark.HexDistance(c) - _landmark.HexDistance(start.Coords);

      var path = new DirectedPath(goal);
      _open.Add(goal.Coords, path);
      _queue.Enqueue (0, path);
    }

    /// <summary>A <see cref="DirectedPath"/> from mid-point to goal obtained by searching backwards from goal.</summary>
    private sealed class PathfinderFwd : BidirectionalPathfinder {
      /// <summary>Create a new instance of <see cref="PathfinderFwd"/></summary>
      /// <param name="start"></param>
      /// <param name="goal"></param>
      /// <param name="stepCost"></param>
      /// <param name="landmarks"></param>
      /// <param name="closed"></param>
      /// <param name="setBestSoFar"></param>
      /// <param name="getBestSoFar"></param>
      public PathfinderFwd(IHex start, IHex goal, Func<IHex, Hexside, int> stepCost, 
        LandmarkCollection landmarks, HashSet<HexCoords> closed,
        Action<DirectedPath,DirectedPath> setBestSoFar,   Func<int> getBestSoFar
      ) : base (start, goal, landmarks, closed, getBestSoFar) {
        _addStep      = (path,there,hexside,cost) => path.AddStep(there,hexside,cost);
        _setBestSoFar = (self,partner) => setBestSoFar(self,partner);
        _stepCost     = (here,hexside,there) => stepCost(there, hexside.Reversed());

        _search       = "Fwd";
        TraceFlags.FindPathDetail.Trace( "   {0} Search uses: vectorGoal = {1}; and landmark at {2}", 
          _search, _vectorGoal, _landmark.Coords);
      }
    }

    /// <summary>A <see cref="DirectedPath"/> from start to mid-point obtained by searching forwards from start.</summary>
    private sealed class PathfinderRev : BidirectionalPathfinder {
      /// <summary>Create a new instance of <see cref="PathfinderRev"/>.</summary>
      /// <param name="start"></param>
      /// <param name="goal"></param>
      /// <param name="stepCost"></param>
      /// <param name="landmarks"></param>
      /// <param name="closed"></param>
      /// <param name="setBestSoFar"></param>
      /// <param name="getBestSoFar"></param>
      public PathfinderRev(IHex start, IHex goal, Func<IHex, Hexside, int> stepCost, 
        LandmarkCollection landmarks,  HashSet<HexCoords> closed,
        Action<DirectedPath,DirectedPath> setBestSoFar,   Func<int> getBestSoFar
      ) : base (goal, start, landmarks, closed, getBestSoFar) {
        _addStep      = (path,there,hexside,cost) => path.AddStep(there,hexside.Reversed(),cost);
        _setBestSoFar = (self,partner) => setBestSoFar(partner,self);
        _stepCost     = (here,hexside,there) => stepCost(here, hexside);

        _search       = "Rev";
        TraceFlags.FindPathDetail.Trace( "   {0} Search uses: vectorGoal = {1}; and landmark at {2}", 
          _search, _vectorGoal, _landmark.Coords);
      }
    }
  }
}
