#region Header

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

#endregion Header

namespace ForEachActivity
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;
    using System.Workflow.ComponentModel.Design;

    [ToolboxBitmapAttribute(typeof(ForEach), "Resources.ForEach.png")]
    [ActivityValidator(typeof(ForEachValidator))]
    [DesignerAttribute(typeof(ForEachDesigner), typeof(IDesigner))]
    public sealed partial class ForEach : CompositeActivity
    {
        #region Fields

        // Define dependency property objects for all properties and events of this activity.
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable), typeof(ForEach));
        public static readonly DependencyProperty IteratingEvent = DependencyProperty.Register("Iterating", typeof(EventHandler), typeof(ForEachActivity.ForEach));

        private static readonly DependencyProperty EnumeratorProperty = DependencyProperty.Register("Enumerator", typeof(IEnumerator), typeof(ForEach));

        private Activity _DynamicActivity = null;

        #endregion Fields

        #region Constructors

        public ForEach()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Events

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [ValidationOptionAttribute(ValidationOption.Optional)]
        [DescriptionAttribute("The Iterating event is fired once on every iteration before the child activity is executed.")]
        [CategoryAttribute("Event")]
        [BrowsableAttribute(true)]
        public event EventHandler Iterating
        {
            add
            {
                base.AddHandler(IteratingEvent, value);
            }
            remove
            {
                base.RemoveHandler(IteratingEvent, value);
            }
        }

        #endregion Events

        #region Properties

        // This is a read-only property that returns the current running child activity at runtime.
        // For each iteration, a new child activity instance is created. Instances created during
        // ealier iterations will have their Status remain as Closed.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Workflow.ComponentModel.Activity DynamicActivity
        {
            get
            {
                if (this.DesignMode)
                    return null;

                // OMAR: Hack to return the dynamic activity if it was set specifically
                if (null != this._DynamicActivity)
                    return this._DynamicActivity;

                if (this.EnabledActivities.Count > 0)
                {
                    Activity[] dynamicChildren = this.GetDynamicActivities(this.EnabledActivities[0]);
                    if (dynamicChildren.Length != 0)
                        return dynamicChildren[0];
                }
                return null;
            }
            set
            {
                // OMAR: Hack to store the dynamic activity from fast workflow runner
                this._DynamicActivity = value;
            }
        }

        // This is a private property whose value can be serialized at runtime.
        // The object that implements the IEnumerator interface must be Serializable
        // and be able to maintaine its state between serialization/deserialization.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Collections.IEnumerator Enumerator
        {
            get
            {
                return (IEnumerator)base.GetValue(EnumeratorProperty);
            }
            set
            {
                base.SetValue(EnumeratorProperty, value);
            }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [BrowsableAttribute(true)]
        [DescriptionAttribute("The Items property specifies the collection whose items are enumerated.")]
        [CategoryAttribute("Activity")]
        public System.Collections.IEnumerable Items
        {
            get
            {
                return base.GetValue(ItemsProperty) as IEnumerable;
            }
            set
            {
                // The Items property can not be changed once the activity starts executing.
                // The collection itself also can not be modified after execution starts. Exception
                // will be thrown if user attempts to modity the collection such as add/remove items.
                if (!this.DesignMode && this.ExecutionStatus != ActivityExecutionStatus.Initialized)
                    throw new InvalidOperationException("The Items property can not be changed after the activity has started executing.");

                base.SetValue(ItemsProperty, value);
            }
        }

        #endregion Properties

        #region Methods

        public void RaiseEvent()
        {
            EventHandler[] invocationList = this.GetInvocationList<EventHandler>(IteratingEvent);
            foreach (EventHandler handler in invocationList)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///	This override function cancels the execution of the child activity if cancel is called
        /// on the ForEach activity itself.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override ActivityExecutionStatus Cancel(ActivityExecutionContext context)
        {
            if (context == null)
                throw new ArgumentNullException("activity execution context is null.");

            // If there is no child activity, the ForEach activity is closed.
            if (this.EnabledActivities.Count == 0)
                return ActivityExecutionStatus.Closed;

            Activity childActivity = this.EnabledActivities[0];
            ActivityExecutionContext childContext =
                context.ExecutionContextManager.GetExecutionContext(childActivity);

            if (childContext != null)
            {
                if (childContext.Activity.ExecutionStatus == ActivityExecutionStatus.Executing)
                    // Cancel the executing child activity.
                    childContext.CancelActivity(childContext.Activity);

                return ActivityExecutionStatus.Canceling;
            }
            return ActivityExecutionStatus.Closed;
        }

        /// <summary>
        ///	This is the main execution logic for the ForEach Activity.  The activity and its
        /// child is executed once for each item in the collection.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext context)
        {
            if (context == null)
                throw new ArgumentNullException("activity execution context is null.");

            if (this.Items != null)
            {
                this.Enumerator = this.Items.GetEnumerator();

                // Then, execute the child activity once for each item contained in the Items collection.
                // If the return value is false, we're at the end of the collection, activity is closed,
                // otherwise, activity is executing.
                if (ExecuteNext(context))
                    return ActivityExecutionStatus.Executing;
            }

            return ActivityExecutionStatus.Closed;
        }

        protected override void OnClosed(IServiceProvider prov)
        {
            // We're done, clean up the private instance properties.
            // We do this to minimize runtime serialization overhead.
            base.RemoveProperty(EnumeratorProperty);
            base.OnClosed(prov);
        }

        // This function executes the ForEach activity.  It advances the current index of the collection.
        // If the end of the collection is reached, return false.  Otherwise, it executes any child activity
        // and return true.
        private bool ExecuteNext(ActivityExecutionContext context)
        {
            // First, move to the next position.
            if (!this.Enumerator.MoveNext())
                return false;

            // Execute the child activity.
            if (this.EnabledActivities.Count > 0)
            {
                // Add the child activity to the execution context and setup the event handler to
                // listen to the child Close event.
                // A new instance of the child activity is created for each iteration.
                ActivityExecutionContext innerContext =
                    context.ExecutionContextManager.CreateExecutionContext(this.EnabledActivities[0]);
                innerContext.Activity.Closed += this.OnChildClose;

                // Fire the Iterating event.
                base.RaiseEvent(IteratingEvent, this, EventArgs.Empty);

                // Execute the child activity again.
                innerContext.ExecuteActivity(innerContext.Activity);
            }
            else
            {
                // an empty foreach loop.
                // If the ForEach activity is still executing, then execute the next one.
                if (this.ExecutionStatus == ActivityExecutionStatus.Executing)
                {
                    if (!ExecuteNext(context))
                        context.CloseActivity();
                }
            }
            return true;
        }

        // When a child activity is closed, it's removed from the execution context,
        // and the execution proceed to the next iteration.
        private void OnChildClose(Object sender, ActivityExecutionStatusChangedEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("OnChildClose parameter 'e' is null.");
            if (sender == null)
                throw new ArgumentNullException("OnChildClose parameter 'sender' is null.");

            ActivityExecutionContext context = sender as ActivityExecutionContext;

            if (context == null)
                throw new ArgumentException("OnChildClose parameter 'sender' is not ActivityExecutionContext.");

            ForEach foreachActivity = context.Activity as ForEach;

            if (foreachActivity == null)
                throw new ArgumentException("OnChildClose parameter 'sender' does not contain a 'ForEach' activity.");

            // Remove the event handler first.
            e.Activity.Closed -= this.OnChildClose;

            // Then remove the child activity from the execution context.
            context.ExecutionContextManager.CompleteExecutionContext(context.ExecutionContextManager.GetExecutionContext(e.Activity));

            // Move on to the next iteration.
            if (this.ExecutionStatus == ActivityExecutionStatus.Canceling)
            {
                context.CloseActivity();
            }
            else if (this.ExecutionStatus == ActivityExecutionStatus.Executing)
            {
                if (!ExecuteNext(context))
                    context.CloseActivity();
            }
        }

        #endregion Methods

        #region Other

        // This is a read-only property that returns the current collection item at runtime.
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public object CurrentItem
        //{
        //    get
        //    {
        //        if (this.DesignMode || this.Enumerator == null)
        //            return null;
        //        return this.Enumerator.Current;
        //    }
        //}

        #endregion Other
    }
}