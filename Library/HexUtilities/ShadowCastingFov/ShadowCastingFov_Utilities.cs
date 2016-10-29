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

using PGNapoleonics.HexUtilities.Common;

namespace PGNapoleonics.HexUtilities.ShadowCasting {
  internal static partial class ShadowCasting {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "code")]
    static IntVector2D LogAndEnqueue(Action<FovCone> enqueue, int range, IntVector2D top, 
            IntVector2D bottom, RiseRun riseRun, int code
    ) {
      if( top.GT(bottom)) {
        var cone = new FovCone(range+1, top, bottom, riseRun);
        #if TraceFOV
          TraceFlag.FieldOfView.Trace(false, "  EQ: ({0}) code: {1}",cone,code);
        #endif
        enqueue(cone);
        return bottom;
      } else {
        return top;
      }
    }

    private static int GetRange(HexCoords coords) { return HexCoords.EmptyCanon.Range(coords); }

    static int XFromVector(int y, IntVector2D v) {        return (-2 * v.Y + v.X * (3 * y + 1) + (3 * v.Y) - 1) / (3 * v.Y);
    }
    /// <summary>Helper matrix for <c>VectorHexTop</c>.</summary>
    static IntMatrix2D matrixHexTop = new IntMatrix2D(3,0,  0,3, 2,1);
    /// <summary>Helper matrix for <c>VectorHexBottom</c>.</summary>
    static IntMatrix2D matrixHexBottom = new IntMatrix2D(3,0,  0,3, -2,-1);

    /// <summary>IntVector2D for top corner of cell Canon(x,y).</summary>
    /// <remarks>
    /// In first dodecant; The top corner for hex (x,y) is determined 
    /// (from close visual inspection) as:
    ///       (x,y) + 1/3 * (2,1)
    /// which reduces to:
    ///       (x + 2/3, y + 1/3) == 1/3 * (3x + 2, 3y + 1)
    /// </remarks>
    static IntVector2D VectorHexTop(HexCoords hex) { return hex.Canon * matrixHexTop; }
    /// <summary>IntVector2D for bottom corner of cell Canon(x,y).</summary>
    /// <remarks>
    /// In first dodecant; The bottom corner for hex (x,y) is determined 
    /// (from close visual inspection) as:
    ///       (x,y) + 1/3 * (-2,-1)
    /// which reduces to:
    ///       (x - 2/3, y - 1/3) == 1/3 * (3x - 2, 3y - 1)
    /// </remarks>
    static IntVector2D VectorHexBottom(HexCoords hex)  { return hex.Canon * matrixHexBottom;  }

    // These are here (instead of IntVector2D.cs) because they are "upside-down" for regular use.
    static IntVector2D VectorMax(IntVector2D lhs, IntVector2D rhs) {
      return lhs.GT(rhs) ? lhs : rhs; 
    }
    //static IntVector2D VectorMin(IntVector2D lhs, IntVector2D rhs) {
    //  return lhs.LE(rhs) ? lhs : rhs;
    //}
    private static bool GT(this IntVector2D lhs, IntVector2D rhs) {
      return lhs.X*rhs.Y > lhs.Y*rhs.X; 
    }
    private static bool LE(this IntVector2D lhs, IntVector2D rhs) { return ! lhs.GT(rhs); }
  }
}
