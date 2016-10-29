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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using PGNapoleonics.HexUtilities.Common;
using PGNapoleonics.HexUtilities.Pathfinding;

/// <summary>Brought to you by <b>PG Software Solutions Inc&dot;.</b>, a quality software provider.</summary>
/// <remarks>Our software solutions are more than <b>Pretty Good</b>; ... they're <b>great!</b></remarks>
namespace PGNapoleonics { }

namespace PGNapoleonics.HexUtilities {
  /// <summary>External interface exposed by individual hexes.</summary>
  public interface IHex {
    /// <summary>The <c>IBoard&lt;IHex></c> on which this hex is located.</summary>
    IBoard<IHex> Board          { get; }

    /// <summary>The <c>HexCoords</c> coordinates for this hex on <c>Board</c>.</summary>
    HexCoords    Coords         { get; }

    /// <summary>Elevation of this hex in "steps" above the minimum elevation of the board.</summary>
    int          Elevation      { get; }

    /// <summary>Elevation "Above Sea Level" in <i>game units</i> of the ground in this hex.</summary>
    /// <remarks>Calculated as BaseElevationASL + Elevation * ElevationStep.</remarks>
    int          ElevationASL   { get; }

    /// <summary>Height ASL in <i>game units</i> of observer's eyes for FOV calculations.</summary>
    int          HeightObserver { get; }

    /// <summary>Height ASL in <i>game units</i> of target above ground level to be spotted.</summary>
    int          HeightTarget   { get; }

    /// <summary>Height ASL in <i>game units</i> of any blocking terrian in this hex.</summary>
    int          HeightTerrain  { get; }

    /// <summary>Returns the neighbouring hex across <c>Hexside</c> <c>hexside</c>.</summary>
    IHex Neighbour(Hexside hexside);

    /// <summary>Cost to extend the path with the hex located across the <c>Hexside</c> at <c>direction</c>.</summary>
    int  StepCost(Hexside direction);

    /// <summary>Cost to exit this hex through the <c>Hexside</c> <c>hexsideExit</c>.</summary>
    int  DirectedStepCost(Hexside hexsideExit);
  }

  /// <summary>Abstract implementation of the interface <see Cref="IHex"/>.</summary>
  public abstract class Hex : IHex, IEquatable<Hex> {
    /// <summary>TODO</summary>
    protected Hex(IBoard<IHex> board, HexCoords coords) { 
      Board     = board;
      Coords    = coords; 
#if FALSE
      Shortcuts = new List<PathShortcut>(0);
#endif
    }

    /// <inheritdoc/>
    public          IBoard<IHex> Board           { get; private set; }

    /// <inheritdoc/>
    public          HexCoords    Coords          { get; private set; }

    /// <inheritdoc/>
    public virtual  int          Elevation       { get; protected set; }

    /// <inheritdoc/>
    public abstract int          ElevationASL    { get; }

    /// <inheritdoc/>
    public virtual  int          HeightObserver  { get { return ElevationASL + 1; } }

    /// <inheritdoc/>
    public virtual  int          HeightTarget    { get { return ElevationASL + 1; } }

    /// <inheritdoc/>
    public abstract int          HeightTerrain   { get; }

    /// <inheritdoc/>
    public abstract int  StepCost(Hexside direction);

    /// <inheritdoc/>
    public virtual  int  DirectedStepCost(Hexside hexsideExit) {
      return Board[Coords.GetNeighbour(hexsideExit)].StepCost(hexsideExit);
    }

    /// <inheritdoc/>
    public          IHex Neighbour(Hexside hexside) { return Board[Coords.GetNeighbour(hexside)]; }

    /// <inheritdoc/>
    public IEnumerator<NeighbourHex> GetEnumerator() { return this.GetNeighbourHexes().GetEnumerator(); }

    #region Value Equality
    /// <inheritdoc/>
    public override bool Equals(object obj) {
      var hex = obj as Hex;
      return hex!=null && Coords.Equals(hex.Coords);
    }

    /// <inheritdoc/>
    public override int GetHashCode()       { return Coords.GetHashCode(); }

    /// <inheritdoc/>
    bool IEquatable<Hex>.Equals(Hex rhs)    { return rhs!=null  &&  this.Coords.Equals(rhs.Coords); }
    #endregion
  }

  /// <summary>Extension methods for <see cref="Hex"/>.</summary>
  public static partial class HexExtensions {
    /// <summary>TODO</summary>
    /// <param name="this"></param>
    /// <param name="directions"></param>
    /// <returns></returns>
     public static IEnumerable<NeighbourHex> GetNeighbourHexes(this IHex @this, HexsideFlags directions) {
      return from n in @this.GetNeighbourHexes()
             where directions.HasFlag(n.HexsideEntry.Direction()) && n.Hex.IsOnboard()
             select n;
    }
   /// <summary>All neighbours of this hex, as an <c>IEnumerable&lt;NeighbourHex></c></summary>
    public static IEnumerable<NeighbourHex> GetAllNeighbours(this IHex @this) {
      return HexsideExtensions.HexsideList.Select(i => 
            new NeighbourHex(@this.Board[@this.Coords.GetNeighbour(i)], i));
    }

    /// <summary>All <i>OnBoard</i> neighbours of this hex, as an <c>IEnumerable&lt;NeighbourHex></c></summary>
    public static IEnumerable<NeighbourHex> GetNeighbourHexes(this IHex @this) { 
      return @this.GetAllNeighbours().Where(n => n.Hex!=null);
    }

    /// <inheritdoc/>
    public static IEnumerator GetEnumerator(this IHex @this) { 
      return @this.GetNeighbourHexes().GetEnumerator();
    }

    /// <summary>The <i>Manhattan</i> distance from this hex to that at <c>coords</c>.</summary>
    public static int Range(this IHex @this, IHex target) { 
      if (@this==null) throw new ArgumentNullException("this");
      if (target==null) throw new ArgumentNullException("target");

      return @this.Coords.Range(target.Coords); 
    }

    /// <summary>Returns a least-cost path from this hex to the hex <c>goal.</c></summary>
    public static IDirectedPath GetDirectedPath(this IHex @this, IHex goal) {
      if (@this==null) throw new ArgumentNullException("this");
      return @this.Board.GetDirectedPath(@this,goal);
    }

    /// <summary>Returns whether this hex is "On Board".</summary>
    public static bool IsOnboard(this IHex @this) {
      return @this!=null  &&  @this.Board.IsOnboard(@this.Coords);
    }
  }
}
