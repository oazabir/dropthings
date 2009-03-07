//--------------------------------------------------------------------------------
// This file is part of the Windows Workflow Foundation Sample Code
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// This source code is intended only as a supplement to Microsoft
// Development Tools and/or on-line documentation.  See these other
// materials for detailed information regarding Microsoft code samples.
//
// THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//--------------------------------------------------------------------------------
namespace ForEachActivity
{
    using System;
    using System.CodeDom;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Reflection;
    using System.Text;
    using System.Workflow.Activities;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Design;

    [ActivityDesignerTheme(typeof(ForEachDesignerTheme))]
    public class ForEachDesigner : SequentialActivityDesigner
    {
        #region Methods

        /// <summary>
        ///	We override this function re-enforce the rule that only one child activity 
        /// can be added.  
        /// </summary>
        /// <param name="insertLocation"></param>
        /// <param name="activitiesToInsert"></param>
        /// <returns></returns>
        public override bool CanInsertActivities(HitTestInfo insertLocation, ReadOnlyCollection<Activity> activitiesToInsert)
        {
            // We only allow one activity to be inserted.
            // ...If the current view is for this designer (and not the Faulting or canceling designer),
            // ...and there is one contained designer (i.e. child activity) then cancel this insert.
            if ((this == ActiveView.AssociatedDesigner) && (ContainedDesigners.Count > 0))
                return false;

            return base.CanInsertActivities(insertLocation, activitiesToInsert);
        }

        /// <summary>
        ///	We override this property to customizes the location of the lower connector so the downward flow arrow 
        /// can be above the lower loop line.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        protected override Rectangle[] GetConnectors()
        {
            //jamescon:  replaced
            //protected override Rectangle[] Connectors

            Rectangle[] connectors = base.GetConnectors();

            CompositeDesignerTheme designerTheme = DesignerTheme as CompositeDesignerTheme;
            Debug.Assert(designerTheme != null);
            if (Expanded && connectors.GetLength(0) > 0)
                connectors[connectors.GetLength(0) - 1].Height = connectors[connectors.GetLength(0) - 1].Height - (((designerTheme != null) ? designerTheme.ConnectorSize.Height : 0) / 3);

            return connectors;
        }

        protected override void Initialize(Activity activity)
        {
            base.Initialize(activity);

            this.HelpText = "Drop an Activity Here";
        }

        //jamescon:  I assume this method
        //protected override void OnLayoutSize(ActivityDesignerLayoutEventArgs e)
        /// <summary>
        ///	We override this function to give the designer a bigger size than the default
        /// size for the sequential activity designer.  This give us more space between the
        /// looping lines and the border of the shape.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override Size OnLayoutSize(ActivityDesignerLayoutEventArgs e)
        {
            Size containerSize = base.OnLayoutSize(e);

            CompositeDesignerTheme compositeDesignerTheme = e.DesignerTheme as CompositeDesignerTheme;
            Debug.Assert(compositeDesignerTheme != null);
            if (compositeDesignerTheme != null && Expanded)
            {
                containerSize.Width += 2 * compositeDesignerTheme.ConnectorSize.Width;
                containerSize.Height += compositeDesignerTheme.ConnectorSize.Height;
            }

            return containerSize;
        }

        /// <summary>
        ///	We override the OnPaint method to draw an arrow that indicates that the execution
        /// flows back to the beginning of this activity after one iteration is completed.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override void OnPaint(ActivityDesignerPaintEventArgs e)
        {
            // We begin with drawing the base sequntial activity shape.
            base.OnPaint(e);

            // We we draw the looping arrow.
            if (Expanded)
            {
                CompositeDesignerTheme compositeDesignerTheme = e.DesignerTheme as CompositeDesignerTheme;
                if (compositeDesignerTheme == null)
                    return;

                Rectangle bounds = Bounds;
                Rectangle textRectangle = TextRectangle;
                Rectangle imageRectangle = ImageRectangle;

                // First figure out where the upper looping arrow should end.  That's the point where the icon
                // image of the ForEach is inside the Bounds.
                Point connectionPoint = Point.Empty;
                if (!imageRectangle.IsEmpty)
                    connectionPoint = new Point(imageRectangle.Left - e.AmbientTheme.Margin.Width / 2, imageRectangle.Top + imageRectangle.Height / 2);
                else if (!textRectangle.IsEmpty)
                    connectionPoint = new Point(textRectangle.Left - e.AmbientTheme.Margin.Width / 2, textRectangle.Top + textRectangle.Height / 2);
                else
                    connectionPoint = new Point((bounds.Left + bounds.Width / 2) - e.AmbientTheme.Margin.Width / 2, bounds.Top + e.AmbientTheme.Margin.Height / 2);

                // Now contruct the 4 points where we would draw the looping arrow.  It starts at the lower-central line,
                // going to the lower right then up to upper right and finishes at the upper-central point.
                Point[] points = new Point[4];
                points[0].X = bounds.Left + bounds.Width / 2;
                points[0].Y = bounds.Bottom - compositeDesignerTheme.ConnectorSize.Height / 3;
                points[1].X = bounds.Left + compositeDesignerTheme.ConnectorSize.Width / 3;
                points[1].Y = bounds.Bottom - compositeDesignerTheme.ConnectorSize.Height / 3;
                points[2].X = bounds.Left + compositeDesignerTheme.ConnectorSize.Width / 3;
                points[2].Y = connectionPoint.Y;
                points[3].X = connectionPoint.X;
                points[3].Y = connectionPoint.Y;

                // Draw the loop.
                DrawConnectors(e.Graphics, compositeDesignerTheme.ForegroundPen, points, LineAnchor.None, LineAnchor.ArrowAnchor);
                // Draw the last section of the downward arrow that connections the beginning of
                // the looping arrow to the bottom of the activity shape.
                DrawConnectors(e.Graphics, compositeDesignerTheme.ForegroundPen, new Point[] { points[0], new Point(bounds.Left + bounds.Width / 2, bounds.Bottom) }, LineAnchor.None, LineAnchor.None);
            }
        }

        #endregion Methods
    }

    // A custom theme that gives the designer a more recognizable appearance.
    internal sealed class ForEachDesignerTheme : CompositeDesignerTheme
    {
        #region Constructors

        public ForEachDesignerTheme(WorkflowTheme theme)
            : base(theme)
        {
            this.ShowDropShadow = false;
            this.ConnectorStartCap = LineAnchor.None;
            this.ConnectorEndCap = LineAnchor.ArrowAnchor;
            this.ForeColor = Color.Black;
            this.BorderColor = ColorTranslator.FromHtml("#8C8E8C");
            this.BorderPen.Width = 2;
            this.BorderStyle = DashStyle.Solid;
            this.BackgroundStyle = LinearGradientMode.ForwardDiagonal;
            this.BackColorStart = Color.WhiteSmoke;
            this.BackColorEnd = Color.SteelBlue;
        }

        #endregion Constructors
    }
}