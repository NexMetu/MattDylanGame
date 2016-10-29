﻿#region License - Copyright (C) 2012-2013 Pieter Geerkens, all rights reserved.
/////////////////////////////////////////////////////////////////////////////////////////
//                PG Software Solutions Inc. - Hex-Grid Utilities
//
// Use of this software is permitted only as described in the attached file: license.txt.
/////////////////////////////////////////////////////////////////////////////////////////
#endregion
namespace PGNapoleonics.HexGridExample2 {
  partial class HexgridExampleForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HexgridExampleForm));
      this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
      this.statusLabel = new System.Windows.Forms.ToolStripLabel();
      this.toolStrip2 = new System.Windows.Forms.ToolStrip();
      this.buttonTransposeMap = new System.Windows.Forms.ToolStripButton();
      this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
      this.buttonFieldOfView = new System.Windows.Forms.ToolStripButton();
      this.buttonRangeLine = new System.Windows.Forms.ToolStripButton();
      this.buttonPathArrow = new System.Windows.Forms.ToolStripButton();
      this.lblPathCutover = new System.Windows.Forms.ToolStripLabel();
      this.txtPathCutover = new System.Windows.Forms.ToolStripTextBox();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.lblLandmark = new System.Windows.Forms.ToolStripLabel();
      this.menuItemLandmarks = new System.Windows.Forms.ToolStripComboBox();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.menuItemDebug = new System.Windows.Forms.ToolStripDropDownButton();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.menuItemHelp = new System.Windows.Forms.ToolStripDropDownButton();
      this.menuItemHelpContents = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this.menuItemHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
      this.hexgridPanel = new PGNapoleonics.HexgridPanel.HexgridPanel(this.components);
      this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
      this.toolStripContainer1.ContentPanel.SuspendLayout();
      this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
      this.toolStripContainer1.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.toolStrip2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.hexgridPanel)).BeginInit();
      this.SuspendLayout();
      // 
      // toolStripContainer1
      // 
      // 
      // toolStripContainer1.BottomToolStripPanel
      // 
      this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.toolStrip1);
      // 
      // toolStripContainer1.ContentPanel
      // 
      this.toolStripContainer1.ContentPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.toolStripContainer1.ContentPanel.Controls.Add(this.hexgridPanel);
      this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(770, 370);
      this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
      this.toolStripContainer1.Name = "toolStripContainer1";
      this.toolStripContainer1.Size = new System.Drawing.Size(770, 420);
      this.toolStripContainer1.TabIndex = 0;
      this.toolStripContainer1.Text = "toolStripContainer1";
      // 
      // toolStripContainer1.TopToolStripPanel
      // 
      this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip2);
      // 
      // toolStrip1
      // 
      this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.statusLabel});
      this.toolStrip1.Location = new System.Drawing.Point(3, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(582, 25);
      this.toolStrip1.TabIndex = 0;
      // 
      // toolStripLabel1
      // 
      this.toolStripLabel1.Name = "toolStripLabel1";
      this.toolStripLabel1.Size = new System.Drawing.Size(45, 22);
      this.toolStripLabel1.Text = "Status: ";
      // 
      // statusLabel
      // 
      this.statusLabel.AutoSize = false;
      this.statusLabel.Name = "statusLabel";
      this.statusLabel.Size = new System.Drawing.Size(525, 22);
      this.statusLabel.Text = "toolStripLabel2";
      this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // toolStrip2
      // 
      this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
      this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonTransposeMap,
            this.toolStripComboBox1,
            this.buttonFieldOfView,
            this.buttonRangeLine,
            this.buttonPathArrow,
            this.lblPathCutover,
            this.txtPathCutover,
            this.toolStripSeparator2,
            this.lblLandmark,
            this.menuItemLandmarks,
            this.toolStripSeparator1,
            this.menuItemDebug,
            this.toolStripSeparator3,
            this.menuItemHelp});
      this.toolStrip2.Location = new System.Drawing.Point(3, 0);
      this.toolStrip2.Name = "toolStrip2";
      this.toolStrip2.Size = new System.Drawing.Size(767, 25);
      this.toolStrip2.TabIndex = 0;
      // 
      // buttonTransposeMap
      // 
      this.buttonTransposeMap.CheckOnClick = true;
      this.buttonTransposeMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.buttonTransposeMap.Image = ((System.Drawing.Image)(resources.GetObject("buttonTransposeMap.Image")));
      this.buttonTransposeMap.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.buttonTransposeMap.Name = "buttonTransposeMap";
      this.buttonTransposeMap.Size = new System.Drawing.Size(65, 22);
      this.buttonTransposeMap.Text = "Transpose";
      this.buttonTransposeMap.ToolTipText = "Toggles transposition of the current map.";
      this.buttonTransposeMap.Click += new System.EventHandler(this.buttonTransposeMap_Click);
      // 
      // toolStripComboBox1
      // 
      this.toolStripComboBox1.AutoSize = false;
      this.toolStripComboBox1.CausesValidation = false;
      this.toolStripComboBox1.Items.AddRange(new object[] {
            "MazeMap",
            "TerrainMap"});
      this.toolStripComboBox1.Name = "toolStripComboBox1";
      this.toolStripComboBox1.Size = new System.Drawing.Size(121, 23);
      this.toolStripComboBox1.Sorted = true;
      this.toolStripComboBox1.Text = "Map:";
      this.toolStripComboBox1.ToolTipText = "Selects map to display.";
      this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_Click);
      // 
      // buttonFieldOfView
      // 
      this.buttonFieldOfView.Checked = true;
      this.buttonFieldOfView.CheckOnClick = true;
      this.buttonFieldOfView.CheckState = System.Windows.Forms.CheckState.Checked;
      this.buttonFieldOfView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.buttonFieldOfView.Image = ((System.Drawing.Image)(resources.GetObject("buttonFieldOfView.Image")));
      this.buttonFieldOfView.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.buttonFieldOfView.Name = "buttonFieldOfView";
      this.buttonFieldOfView.Size = new System.Drawing.Size(33, 22);
      this.buttonFieldOfView.Text = "FOV";
      this.buttonFieldOfView.ToolTipText = "Toggles display of current Field-of-View";
      this.buttonFieldOfView.Click += new System.EventHandler(this.buttonFieldOfView_Click);
      // 
      // buttonRangeLine
      // 
      this.buttonRangeLine.CheckOnClick = true;
      this.buttonRangeLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.buttonRangeLine.Image = ((System.Drawing.Image)(resources.GetObject("buttonRangeLine.Image")));
      this.buttonRangeLine.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.buttonRangeLine.Name = "buttonRangeLine";
      this.buttonRangeLine.Size = new System.Drawing.Size(69, 22);
      this.buttonRangeLine.Text = "Range Line";
      this.buttonRangeLine.ToolTipText = "Toggles (a) display of Range Line; and (b) Field-of-View source between StartHex " +
    "and HotspotHex.";
      this.buttonRangeLine.CheckedChanged += new System.EventHandler(this.buttonRangeLine_Click);
      // 
      // buttonPathArrow
      // 
      this.buttonPathArrow.Checked = true;
      this.buttonPathArrow.CheckOnClick = true;
      this.buttonPathArrow.CheckState = System.Windows.Forms.CheckState.Checked;
      this.buttonPathArrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.buttonPathArrow.Image = ((System.Drawing.Image)(resources.GetObject("buttonPathArrow.Image")));
      this.buttonPathArrow.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.buttonPathArrow.Name = "buttonPathArrow";
      this.buttonPathArrow.Size = new System.Drawing.Size(70, 22);
      this.buttonPathArrow.Text = "Path Arrow";
      this.buttonPathArrow.ToolTipText = "Toggle display of Path Arrow.";
      this.buttonPathArrow.CheckedChanged += new System.EventHandler(this.buttonPathArrow_Click);
      // 
      // lblPathCutover
      // 
      this.lblPathCutover.Name = "lblPathCutover";
      this.lblPathCutover.Size = new System.Drawing.Size(79, 22);
      this.lblPathCutover.Text = "Path Cutover:";
      this.lblPathCutover.ToolTipText = resources.GetString("lblPathCutover.ToolTipText");
      // 
      // txtPathCutover
      // 
      this.txtPathCutover.Name = "txtPathCutover";
      this.txtPathCutover.Size = new System.Drawing.Size(40, 25);
      this.txtPathCutover.Tag = 20;
      this.txtPathCutover.Text = "20";
      this.txtPathCutover.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.txtPathCutover.ToolTipText = resources.GetString("txtPathCutover.ToolTipText");
      this.txtPathCutover.TextChanged += new System.EventHandler(this.txtPathCutover_TextChanged);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // lblLandmark
      // 
      this.lblLandmark.Name = "lblLandmark";
      this.lblLandmark.Size = new System.Drawing.Size(63, 22);
      this.lblLandmark.Text = "Landmark:";
      this.lblLandmark.ToolTipText = "Landmark from which shortest-paths distances are to be displayed.";
      // 
      // menuItemLandmarks
      // 
      this.menuItemLandmarks.Items.AddRange(new object[] {
            "None"});
      this.menuItemLandmarks.Name = "menuItemLandmarks";
      this.menuItemLandmarks.Size = new System.Drawing.Size(100, 25);
      this.menuItemLandmarks.ToolTipText = "Landmark from which shortest-paths distances are to be displayed.";
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // menuItemDebug
      // 
      this.menuItemDebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.menuItemDebug.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.menuItemDebug.Name = "menuItemDebug";
      this.menuItemDebug.Size = new System.Drawing.Size(87, 22);
      this.menuItemDebug.Text = "&Debug Trace";
      this.menuItemDebug.ToolTipText = "Build with DEBUG to enable this feature.";
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
      // 
      // menuItemHelp
      // 
      this.menuItemHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.menuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemHelpContents,
            this.toolStripSeparator4,
            this.menuItemHelpAbout});
      this.menuItemHelp.Image = ((System.Drawing.Image)(resources.GetObject("menuItemHelp.Image")));
      this.menuItemHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.menuItemHelp.Name = "menuItemHelp";
      this.menuItemHelp.ShowDropDownArrow = false;
      this.menuItemHelp.Size = new System.Drawing.Size(36, 22);
      this.menuItemHelp.Text = "&Help";
      // 
      // menuItemHelpContents
      // 
      this.menuItemHelpContents.Name = "menuItemHelpContents";
      this.menuItemHelpContents.ShortcutKeys = System.Windows.Forms.Keys.F1;
      this.menuItemHelpContents.Size = new System.Drawing.Size(190, 22);
      this.menuItemHelpContents.Text = "&Contents";
      this.menuItemHelpContents.Click += new System.EventHandler(this.menuItemHelpContents_Click);
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size(187, 6);
      // 
      // menuItemHelpAbout
      // 
      this.menuItemHelpAbout.Name = "menuItemHelpAbout";
      this.menuItemHelpAbout.Size = new System.Drawing.Size(190, 22);
      this.menuItemHelpAbout.Text = "&ABout HexgridUtilities";
      // 
      // hexgridPanel
      // 
      this.hexgridPanel.AutoScroll = true;
      this.hexgridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.hexgridPanel.Host = null;
      this.hexgridPanel.IsTransposed = false;
      this.hexgridPanel.Location = new System.Drawing.Point(0, 0);
      this.hexgridPanel.Name = "hexgridPanel";
      this.hexgridPanel.ScaleIndex = 0;
      this.hexgridPanel.Scales = ((System.Collections.ObjectModel.ReadOnlyCollection<float>)(resources.GetObject("hexgridPanel.Scales")));
      this.hexgridPanel.Size = new System.Drawing.Size(766, 366);
      this.hexgridPanel.TabIndex = 0;
      this.hexgridPanel.HotspotHexChange += new System.EventHandler<PGNapoleonics.HexgridPanel.HexEventArgs>(this.PanelBoard_HotSpotHexChange);
      this.hexgridPanel.MouseCtlClick += new System.EventHandler<PGNapoleonics.HexgridPanel.HexEventArgs>(this.PanelBoard_GoalHexChange);
      this.hexgridPanel.MouseLeftClick += new System.EventHandler<PGNapoleonics.HexgridPanel.HexEventArgs>(this.PanelBoard_StartHexChange);
      this.hexgridPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.hexgridPanel_MouseMove);
      // 
      // HexgridExampleForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(770, 420);
      this.Controls.Add(this.toolStripContainer1);
      this.Name = "HexgridExampleForm";
      this.Text = "HexGridExample2";
      this.Load += new System.EventHandler(this.HexGridExampleForm_Load);
      this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
      this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
      this.toolStripContainer1.ContentPanel.ResumeLayout(false);
      this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
      this.toolStripContainer1.TopToolStripPanel.PerformLayout();
      this.toolStripContainer1.ResumeLayout(false);
      this.toolStripContainer1.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.toolStrip2.ResumeLayout(false);
      this.toolStrip2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.hexgridPanel)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    private PGNapoleonics.HexgridPanel.HexgridPanel hexgridPanel;
    private System.Windows.Forms.ToolStrip toolStrip2;
    private System.Windows.Forms.ToolStripButton buttonTransposeMap;
    private System.Windows.Forms.ToolStripLabel statusLabel;
    private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
    private System.Windows.Forms.ToolStripButton buttonFieldOfView;
    private System.Windows.Forms.ToolStripLabel lblPathCutover;
    private System.Windows.Forms.ToolStripTextBox txtPathCutover;
    private System.Windows.Forms.ToolStripDropDownButton menuItemDebug;
    private System.Windows.Forms.ToolStripComboBox menuItemLandmarks;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripLabel lblLandmark;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton buttonPathArrow;
    private System.Windows.Forms.ToolStripButton buttonRangeLine;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripDropDownButton menuItemHelp;
    private System.Windows.Forms.ToolStripMenuItem menuItemHelpContents;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripMenuItem menuItemHelpAbout;
  }
}