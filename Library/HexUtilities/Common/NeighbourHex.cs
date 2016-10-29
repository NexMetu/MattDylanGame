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
using System.Diagnostics;
using System.Globalization;

namespace PGNapoleonics.HexUtilities.Common {
  /// <summary>TODO</summary>
  [DebuggerDisplay("NeighbourHex: {Hex.Coords} exits to {HexsideEntry}")]
  public struct NeighbourHex : IEquatable<NeighbourHex> {
    /// <summary>TODO</summary>
    public IHex    Hex          { get; private set; }
    /// <summary>TODO</summary>
    public Hexside HexsideEntry { get {return HexsideIndex;} }
    /// <summary>TODO</summary>
    public Hexside HexsideExit  { get {return HexsideIndex.Reversed();} }
    /// <summary>TODO</summary>
    public Hexside HexsideIndex { get; private set; }

    /// <summary>TODO</summary>
    public NeighbourHex(IHex hex) : this(hex, null) {}
    /// <summary>TODO</summary>
    public NeighbourHex(IHex hex, HexsideFlags hexside)  : this(hex,hexside.IndexOf()) {}
    /// <summary>TODO</summary>
    public NeighbourHex(IHex hex, Hexside? hexsideIndex) : this() {
      Hex          = hex;
      HexsideIndex = hexsideIndex ?? 0;
    }

    /// <inheritdoc/>
    public override string ToString() { 
      return string.Format(CultureInfo.InvariantCulture,
        "NeighbourHex: {0} exits to {1}", Hex.Coords, HexsideEntry);
    }

    #region Value Equality - on Hex field only
    /// <inheritdoc/>
    public override bool Equals(object obj)                  {
      return (obj is NeighbourHex)  &&  Hex.Coords.Equals(((NeighbourHex)obj).Hex.Coords); 
    }
    /// <inheritdoc/>
    public override int GetHashCode()                        { return Hex.Coords.GetHashCode(); }
    bool IEquatable<NeighbourHex>.Equals(NeighbourHex rhs)   { return this == rhs; }

    /// <summary>TODO</summary>
    public static bool operator != (NeighbourHex lhs, NeighbourHex rhs) { return ! (lhs==rhs); }
    /// <summary>TODO</summary>
    public static bool operator == (NeighbourHex lhs, NeighbourHex rhs) {
      return lhs.Hex.Coords.Equals(rhs.Hex.Coords);
    }
    #endregion
  }
}
