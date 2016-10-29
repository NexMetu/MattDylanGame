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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

using PGNapoleonics.HexUtilities;

using PGNapoleonics.HexUtilities.Common;
using PGNapoleonics.WinForms;

namespace PGNapoleonics.HexgridPanel {
  /// <summary>TODO</summary>
  public interface IMapDisplay {
    /// <summary>TODO</summary>
    Size   GridSize      { get; }
    /// <summary>Rectangular extent in pixels of the defined mapboard.</summary>
    Size   MapSizePixels { get; }
    /// <summary>TODO</summary>
    string Name          { get; }

    /// <summary>TODO</summary>
    CoordsRectangle GetClipCells(PointF point, SizeF size);
    /// <summary>TODO</summary>
    CoordsRectangle GetClipCells(RectangleF visibleClipBounds);

    /// <summary>Paint the top layer of the display, graphics that changes frequently between refreshes.</summary>
    /// <param name="g">Graphics object for the canvas being painted.</param>
    void  PaintHighlight(Graphics g);
    /// <summary>Paint the base layer of the display, graphics that changes rarely between refreshes.</summary>
    /// <param name="g">Graphics object for the canvas being painted.</param>
    void  PaintMap(Graphics g);
    /// <summary>Paint the intermediate layer of the display, graphics that changes infrequently between refreshes.</summary>
    /// <param name="g">Graphics object for the canvas being painted.</param>
    void  PaintUnits(Graphics g);
  }

  /// <summary>Sub-class implementation of a <b>WinForms</b> Panel with integrated <see cref="Hexgrid"/> support.</summary>
  public partial class HexgridPanel : Panel, ISupportInitialize, IHexgridHost {
    /// <summary>TODO</summary>
    public HexgridPanel() {
      InitializeComponent();
    }
    /// <summary>TODO</summary>
    public HexgridPanel(IContainer container) {
      if (container==null) throw new ArgumentNullException("container");
      container.Add(this);

      InitializeComponent();
    }

    #region ISupportInitialize implementation
    /// <inheritdoc/>>
    public virtual void BeginInit() { 
      MapMargin = new System.Drawing.Size(5,5);
      //SetScaleList(new List<float>() {1.000F}.AsReadOnly());
      Scales = new List<float>() {1.000F}.AsReadOnly();
    }
    /// <inheritdoc/>
    public virtual void EndInit() { 
      this.MakeDoubleBuffered(true);
      HotspotHex = HexCoords.EmptyUser;
    }
    #endregion

    /// <summary>TODO</summary>
    public event EventHandler<HexEventArgs> HotspotHexChange;
    /// <summary>TODO</summary>
    public event EventHandler<HexEventArgs> MouseAltClick;
    /// <summary>TODO</summary>
    public event EventHandler<HexEventArgs> MouseCtlClick;
    /// <summary>TODO</summary>
    public event EventHandler<HexEventArgs> MouseLeftClick;
    /// <summary>TODO</summary>
    public event EventHandler<HexEventArgs> MouseRightClick;
    /// <summary>TODO</summary>
    public event EventHandler<EventArgs>    ScaleChange;

    /// <summary>MapBoard hosting this panel.</summary>
    public IMapDisplay Host          {
      get { return _host; }
      set { _host = value; MapBuffer = null; }
    } IMapDisplay _host;

    /// <summary>TODO</summary>
    public HexCoords   HotspotHex    { get; private set; }

    /// <summary>TODO</summary>
    public bool        IsTransposed  { 
      get { return _isTransposed; }
      set { _isTransposed = value;  
            Hexgrid       = IsTransposed ? new TransposedHexgrid(this) 
                                         : new Hexgrid(this);  
            if (IsHandleCreated) SetScroll();   
          }
    } bool _isTransposed;

    /// <summary>Margin of map in pixels.</summary>
    public Size        MapMargin     { get; private set; }

    /// <inheritdoc/>
    public Size        MapSizePixels { get {return Host.MapSizePixels + MapMargin.Scale(2);} }

    /// <summary>Current scaling factor for map display.</summary>
    public float       MapScale      { get { return Scales[ScaleIndex]; } }

    /// <summary>Returns <code>HexCoords</code> of the hex closest to the center of the current viewport.</summary>
    public HexCoords PanelCenterHex  { 
      get { return GetHexCoords( Location + Size.Round(ClientSize.Scale(0.50F)) ); }
    }

    /// <summary>Index into <code>Scales</code> of current map scale.</summary>
    public virtual int ScaleIndex    { 
      get { return _scaleIndex; }
      set { var newValue = Math.Max(0, Math.Min(Scales.Count-1, value));
            if( _scaleIndex != newValue) {
              _scaleIndex = newValue; 
              OnScaleChange(EventArgs.Empty); 
            }
      } 
    } int _scaleIndex;

    /// <summary>Array of supported map scales  as IList&lt;float&gt;.</summary>
    public ReadOnlyCollection<float>     Scales        { get; set; }
    /// <summary>Set property Scales (array of supported map scales as IList&lt;float&gt;.</summary>
    [Obsolete("Use Property Setter 'Scales' instead.")]
    public void SetScaleList(ReadOnlyCollection<float> scales) { Scales = scales; }

    /// <summary>Set ScrollBar increments and bounds from map dimensions.</summary>
    public virtual void SetScroll() {
      var smallChange              = Size.Ceiling(Host.GridSize.Scale(MapScale));
      HorizontalScroll.SmallChange = smallChange.Width;
      VerticalScroll.SmallChange   = smallChange.Height;

      var largeChange              = Size.Round(ClientSize.Scale(0.75F));
      HorizontalScroll.LargeChange = Math.Max(largeChange.Width,  smallChange.Width);
      VerticalScroll.LargeChange   = Math.Max(largeChange.Height, smallChange.Height);

      var size                     = Hexgrid.Size;
      if (AutoScrollMinSize != size) {
        AutoScrollMinSize          = size;
        HorizontalScroll.Maximum   = Math.Min(1, Math.Max(1, size.Width  - ClientSize.Width));
        VerticalScroll.Maximum     = Math.Min(1, Math.Max(1, size.Height - ClientSize.Height));
        Invalidate();
      }
    }

    #region Grid Coordinates
    ///<inheritdoc/>
    protected Hexgrid    Hexgrid        { get; private set; }
    Size    IHexgridHost.ClientSize     { get { return ClientSize; } }
    /// <summary>TODO</summary>
    public SizeF   GridSizeF      { get { return Host.GridSize.Scale(MapScale); } }
    /// <summary>TODO</summary>
    public Point   ScrollPosition { get { return AutoScrollPosition; } }

    CoordsRectangle GetClipCells(PointF point, SizeF size) {
      return Host.GetClipCells(point, size);
    }

    /// <summary>Returns, as a Rectangle, the IUserCoords for the currently visible extent.</summary>
    public virtual CoordsRectangle     VisibleRectangle {
      get { return GetClipCells( AutoScrollPosition.Scale(-1.0F/MapScale), 
                                      ClientSize.Scale(1.0F/MapScale) );
      }
    }

    /// <summary><c>HexCoords</c> for a selected hex.</summary>
    /// <param name="point">Screen point specifying hex to be identified.</param>
    /// <returns>Coordinates for a hex specified by a screen point.</returns>
    /// <remarks>See "file://Documentation/HexGridAlgorithm.mht"</remarks>
    public HexCoords GetHexCoords(Point point) {
      return Hexgrid.GetHexCoords(point, new Size(AutoScrollPosition));
    }
    /// <summary>Returns ScrollPosition that places given hex in the upper-Left of viewport.</summary>
    /// <param name="coordsNewULHex"><c>HexCoords</c> for new upper-left hex</param>
    /// <returns>Pixel coordinates in Client reference frame.</returns>
    public Point HexCenterPoint(HexCoords coordsNewULHex) {
      return Hexgrid.HexCenterPoint(coordsNewULHex);
    }
    /// <summary>Returns the scroll position to center a specified hex in viewport.</summary>
    /// <param name="coordsNewCenterHex"><c>HexCoords</c> for the hex to be centered in viewport.</param>
    /// <returns>Pixel coordinates in Client reference frame.</returns>
    protected Point ScrollPositionToCenterOnHex(HexCoords coordsNewCenterHex) {
      return Hexgrid.ScrollPositionToCenterOnHex(coordsNewCenterHex);
    }
    #endregion

    #region Painting
    /// <summary>TODO</summary>
    protected override void OnPaintBackground(PaintEventArgs e) { ; }
    /// <summary>TODO</summary>
    protected override void OnPaint(PaintEventArgs e) {
      if (e==null) throw new ArgumentNullException("e");
      base.OnPaint(e);
      if(IsHandleCreated)    PaintPanel(e.Graphics);
    }
    static readonly Matrix TransposeMatrix = new Matrix(0F,1F, 1F,0F, 0F,0F);
    /// <summary>TODO</summary>
    protected virtual void PaintPanel(Graphics g) {
      if (g==null) throw new ArgumentNullException("g");
      var scroll = Hexgrid.ScrollPosition;
      if (DesignMode) { g.FillRectangle(Brushes.Gray, ClientRectangle);  return; }

      g.Clear(Color.White);
      g.DrawRectangle(Pens.Black, ClientRectangle);

      if (IsTransposed) { g.Transform = TransposeMatrix; }
      g.TranslateTransform(scroll.X, scroll.Y);
      g.ScaleTransform(MapScale,MapScale);

      var state = g.Save();
      g.DrawImageUnscaled(MapBuffer, Point.Empty);

      g.Restore(state); state = g.Save();
//      g.TranslateTransform(MapMargin.Width, MapMargin.Height);
      Host.PaintUnits(g);

      g.Restore(state); state = g.Save();
//      g.TranslateTransform(MapMargin.Width, MapMargin.Height);
      Host.PaintHighlight(g);
    }
    #endregion

    #region Double-Buffering
    /// <summary>TODO</summary>
    Bitmap MapBuffer     { 
      get { return _mapBuffer ?? ( _mapBuffer = PaintBuffer()); } 
      set { if (_mapBuffer!=null) _mapBuffer.Dispose(); _mapBuffer = value; }
    } Bitmap _mapBuffer;

    /// <summary>TODO</summary>
    Bitmap PaintBuffer() {
      var size      = MapSizePixels;

      Bitmap buffer     = null;
      Bitmap tempBuffer = null;
      try {
        tempBuffer = new Bitmap(size.Width,size.Height, PixelFormat.Format32bppPArgb);
        using(var g = Graphics.FromImage(tempBuffer)) {
          g.Clear(Color.White);
          g.TranslateTransform(MapMargin.Width, MapMargin.Height);
          Host.PaintMap(g);
        }
        buffer     = tempBuffer;
        tempBuffer = null;
      } finally { if (tempBuffer!=null) tempBuffer.Dispose(); }
      return buffer;
    }
    #endregion

    /// <summary>TODO</summary>
    protected static  bool  IsAltKeyDown   { get {return ModifierKeys.HasFlag(Keys.Alt);} }
    /// <summary>TODO</summary>
    protected static  bool  IsCtlKeyDown   { get {return ModifierKeys.HasFlag(Keys.Control);} }
    /// <summary>TODO</summary>
    protected static  bool  IsShiftKeyDown { get {return ModifierKeys.HasFlag(Keys.Shift);} }

    #region Mouse & Scroll events
    /// <summary>TODO</summary>
    protected override void OnMouseClick(MouseEventArgs e) {
      if (e==null) throw new ArgumentNullException("e");
      TraceFlags.Mouse.Trace(" - {0}.OnMouseClick - Shift: {1}; Ctl: {2}; Alt: {3}", 
                                      Name, IsShiftKeyDown, IsCtlKeyDown, IsAltKeyDown);

      var eventArgs = new HexEventArgs( GetHexCoords(e.Location), e, ModifierKeys);

           if (e.Button == MouseButtons.Middle)   base.OnMouseClick(eventArgs);
      else if (e.Button == MouseButtons.Right)    OnMouseRightClick(eventArgs);
      else if (IsAltKeyDown  && !IsCtlKeyDown)    OnMouseAltClick(eventArgs);
      else if (IsCtlKeyDown)                      OnMouseCtlClick(eventArgs);
      else                                        OnMouseLeftClick(eventArgs);
    }
    /// <summary>TODO</summary>
    protected override void OnMouseMove(MouseEventArgs e) {
      if (e==null) throw new ArgumentNullException("e");
      var newHex = GetHexCoords(e.Location);
      if ( newHex != HotspotHex)
        OnHotspotHexChange(new HexEventArgs(newHex));
      HotspotHex = newHex;

      base.OnMouseMove(e);
    }
    /// <summary>TODO</summary>
    protected override void OnMouseWheel(MouseEventArgs e) {
      if (e==null) throw new ArgumentNullException("e");
      TraceFlags.ScrollEvents.Trace(" - {0}.OnMouseWheel: {1}", Host.Name, e.ToString()); 
      if( Control.ModifierKeys.HasFlag(Keys.Control)) {
        ScaleIndex += Math.Sign(e.Delta);  
      } else {
        var orientation = IsShiftKeyDown ? ScrollOrientation.HorizontalScroll
                                         : ScrollOrientation.VerticalScroll;
        WheelPanel(orientation, -e.Delta);
      }
      Invalidate();
    }

    /// <summary>TODO</summary>
    protected virtual void OnMouseAltClick(HexEventArgs e) {
      if (e==null) throw new ArgumentNullException("e");
      var handler = MouseAltClick;
      if( handler != null ) handler(this, e);
    }
    /// <summary>TODO</summary>
    protected virtual void OnMouseCtlClick(HexEventArgs e) {
      if (e==null) throw new ArgumentNullException("e");
      var handler = MouseCtlClick;
      if( handler != null ) handler(this, e);
    }
    /// <summary>TODO</summary>
    protected virtual void OnMouseLeftClick(HexEventArgs e) {
      if (e==null) throw new ArgumentNullException("e");
      var handler = MouseLeftClick;
      if( handler != null ) handler(this, e);
    }
    /// <summary>TODO</summary>
    protected virtual void OnMouseRightClick(HexEventArgs e) {
      if (e==null) throw new ArgumentNullException("e");
      var handler = MouseRightClick;
      if( handler != null ) handler(this, e);
    }

    /// <summary>TODO</summary>
    protected virtual void OnHotspotHexChange(HexEventArgs e) {
      if (e==null) throw new ArgumentNullException("e");
      var handler = HotspotHexChange;
      if( handler != null ) handler(this, e);
    }
    /// <summary>TODO</summary>
    protected virtual void OnScaleChange(EventArgs e) {
      if (e==null) throw new ArgumentNullException("e");
      var handler = ScaleChange;
      if( handler != null ) handler(this, e);
    }

    int mouseWheelHorizontalRemainder = 0;
    int mouseWheelVerticalRemainder   = 0;
    void WheelPanel(ScrollOrientation orientation, int delta) {
      if( orientation == ScrollOrientation.VerticalScroll )
        mouseWheelVerticalRemainder   = WheelPanelDetail(orientation, VerticalScroll,  delta,
            (p,amount) => new Point(-p.X, -p.Y + amount), mouseWheelVerticalRemainder);
      else
        mouseWheelHorizontalRemainder = WheelPanelDetail(orientation, HorizontalScroll,delta,
            (p,amount) => new Point(-p.X + amount, -p.Y), mouseWheelHorizontalRemainder);
    }
    int WheelPanelDetail(ScrollOrientation orientation, ScrollProperties scroll, int delta,
      Func<Point,int,Point> newAutoScroll, int remainder) {
      if (scroll==null) throw new ArgumentNullException("scroll");
      if (newAutoScroll==null) throw new ArgumentNullException("newAutoScroll");
      const int mouseWheelDelta = 120;

      var scrollLines = SystemInformation.MouseWheelScrollLines;
      var steps       = Math.Sign(delta) * ( (Math.Abs(delta+remainder) * scrollLines) / mouseWheelDelta );
      remainder       = Math.Sign(delta) * ( (Math.Abs(delta+remainder) * scrollLines) % mouseWheelDelta );

      if (steps != 0) {
        var oldValue = scroll.Value;
        AutoScrollPosition = newAutoScroll(AutoScrollPosition, scroll.SmallChange * steps);
        OnScroll( new ScrollEventArgs(ScrollEventType.ThumbPosition, oldValue, scroll.Value, orientation) );
      }
      return remainder;
    }
    /// <summary>TODO</summary>
    public void ScrollPanel(ScrollEventType type, ScrollOrientation orientation, int sign) {
      if( sign != 0 ) {
        ScrollProperties          scroll;
        Func<Point,int,int,Point> func;
        if( orientation == ScrollOrientation.VerticalScroll ) {
          scroll = VerticalScroll;
          func   = (p,sgn,stp) => new Point(-p.X, -p.Y + stp * sgn);
        } else {
          scroll = HorizontalScroll;
          func   = (p,sgn,stp) => new Point(-p.X + stp * sgn, -p.Y);
        }

        var step = type.HasFlag(ScrollEventType.LargeIncrement) || type.HasFlag(ScrollEventType.LargeDecrement)
                 ? scroll.LargeChange
                 : scroll.SmallChange;
        var oldValue = scroll.Value;
        AutoScrollPosition = func(AutoScrollPosition, sign, step);
        OnScroll( new ScrollEventArgs(type, oldValue, scroll.Value, orientation) );
      }
    }
    #endregion
  }
}
