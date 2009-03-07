// This activity is taken from:
// http://www.masteringbiztalk.com/blogs/jon/PermaLink,guid,7be9fb53-0ddf-4633-b358-01c3e9999088.aspx
namespace Dropthings.Business.Activities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Reflection;
    using System.Workflow.Activities;
    using System.Workflow.Activities.Rules;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;
    using System.Workflow.ComponentModel.Design;
    using System.Workflow.ComponentModel.Serialization;
    using System.Workflow.Runtime;
    using System.Workflow.Runtime.Hosting;

    [Designer(typeof(CallWorkflowDesigner),typeof(IDesigner))]
    public partial class CallWorkflowActivity : Activity
    {
        #region Fields

        public static DependencyProperty ParametersProperty = System.Workflow.ComponentModel.DependencyProperty.Register("Parameters", typeof(WorkflowParameterBindingCollection), typeof(CallWorkflowActivity),new PropertyMetadata(DependencyPropertyOptions.Metadata|DependencyPropertyOptions.ReadOnly));
        public static DependencyProperty TypeProperty = System.Workflow.ComponentModel.DependencyProperty.Register("Type", typeof(Type), typeof(CallWorkflowActivity),new PropertyMetadata(DependencyPropertyOptions.Metadata));

        #endregion Fields

        #region Constructors

        public CallWorkflowActivity()
        {
            InitializeComponent();
            base.SetReadOnlyPropertyValue(CallWorkflowActivity.ParametersProperty, new WorkflowParameterBindingCollection(this));
        }

        #endregion Constructors

        #region Properties

        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public WorkflowParameterBindingCollection Parameters
        {
            get
            {
                return ((WorkflowParameterBindingCollection)(base.GetValue(CallWorkflowActivity.ParametersProperty)));
            }
        }

        [Description("The workflow Type to call synchronously")]
        [Category("Misc")]
        [Browsable(true)]
        [Editor(typeof(TypeBrowserEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Type Type
        {
            get
            {
                return ((Type)(base.GetValue(CallWorkflowActivity.TypeProperty)));
            }
            set
            {
                base.SetValue(CallWorkflowActivity.TypeProperty, value);
            }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext aec)
        {
            //get the services needed
            //custom service to run the workflow
            CallWorkflowService ws = aec.GetService<CallWorkflowService>();

            //Queuing service to setup the queue so the service can "callback"
            WorkflowQueuingService qs = aec.GetService<WorkflowQueuingService>();
            //create the queue the service can call back on when the child workflow is done
            //you might want the queuename to be something different
            string qn = String.Format("{0}:{1}:{2}", this.WorkflowInstanceId.ToString(), Type.Name, this.Name);
            WorkflowQueue q = qs.CreateWorkflowQueue(qn, false);
            q.QueueItemAvailable += new EventHandler<QueueEventArgs>(q_QueueItemAvailable);
            //copy the params to a new collection
            Dictionary<string, object> inparams = new Dictionary<string, object>();
            foreach (WorkflowParameterBinding bp in this.Parameters)
            {
                PropertyInfo pi = Type.GetProperty(bp.ParameterName);
                if(pi.CanWrite)
                    inparams.Add(bp.ParameterName, bp.Value);
            }
            //ask the service to start the workflow
            ws.StartWorkflow(Type, inparams, this.WorkflowInstanceId, qn);
            return ActivityExecutionStatus.Executing;
        }

        void q_QueueItemAvailable(object sender, QueueEventArgs e)
        {
            ActivityExecutionContext aec = sender as ActivityExecutionContext;
            if (aec != null)
            {
                WorkflowQueuingService qs = aec.GetService<WorkflowQueuingService>();

                WorkflowQueue q = qs.GetWorkflowQueue(e.QueueName);
                //get the outparameters from the workflow
                object o = q.Dequeue();
                //delete the queue
                qs.DeleteWorkflowQueue(e.QueueName);
                Dictionary<string,object> outparams = o as Dictionary<string,object>;
                if(outparams!=null)
                {
                    foreach (KeyValuePair<string, object>  item in outparams)
                    {
                        if (this.Parameters.Contains(item.Key))
                        {
                            //modify the value
                            this.Parameters[item.Key].SetValue(WorkflowParameterBinding.ValueProperty, item.Value);
                        }

                    }
                }
                aec.CloseActivity();
            }
        }

        #endregion Methods
    }

    public class CallWorkflowDesigner : ActivityDesigner
    {
        #region Methods

        protected override void OnActivityChanged(ActivityChangedEventArgs e)
        {
            if (e.Member.Name == "Type")
                TypeDescriptor.Refresh(e.Activity);
            else
                base.OnActivityChanged(e);
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            CallWorkflowActivity a = this.Activity as CallWorkflowActivity;
            if (a.Type != null)
            {
                //get the properties and add them as properties
                PropertyInfo[] pis = a.Type.GetProperties();
                foreach (PropertyInfo pi in pis)
                {
                    if (pi.DeclaringType == a.Type)
                    {
                        //add a new parameter
                        properties[pi.Name] = new CallWorkflowParameterBindingPropertyDescriptor(pi.Name, pi.PropertyType, new Attribute[] { new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden), new BrowsableAttribute(true), new CategoryAttribute("Parameters"), new EditorAttribute(typeof(BindUITypeEditor), typeof(UITypeEditor)) });
                    }
                }

            }
        }

        #endregion Methods
    }

    public class CallWorkflowParameterBindingPropertyDescriptor : PropertyDescriptor
    {
        #region Fields

        Type _type = null;

        #endregion Fields

        #region Constructors

        public CallWorkflowParameterBindingPropertyDescriptor(string propertyName, Type parameterType, Attribute[] attrs)
            : base(propertyName, attrs)
        {
            this._type = parameterType;
        }

        #endregion Constructors

        #region Properties

        public override Type ComponentType
        {
            get
            {

                return typeof(CallWorkflowActivity);
            }
        }

        public override TypeConverter Converter
        {
            get
            {
                return new ActivityBindTypeConverter();
            }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get
            {
                if (_type != null)
                    return _type;
                else
                    return typeof(ActivityBind);
            }
        }

        #endregion Properties

        #region Methods

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            WorkflowParameterBindingCollection parameters = ((CallWorkflowActivity)component).Parameters;
            if (parameters != null && parameters.Contains(this.Name))
            {
                if (parameters[this.Name].IsBindingSet(WorkflowParameterBinding.ValueProperty))
                    return parameters[this.Name].GetBinding(WorkflowParameterBinding.ValueProperty);
                else
                    return parameters[this.Name].GetValue(WorkflowParameterBinding.ValueProperty);
            }
            return null;
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
            if (component != null)
            {
                ISite site = GetSite(component);
                IComponentChangeService changeService = null;
                if (site != null)
                    changeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
                // Raise the OnComponentChanging event
                changeService.OnComponentChanging(component, this);

                // Save the old value
                object oldValue = GetValue(component);

                try
                {
                    WorkflowParameterBindingCollection parameters = ((CallWorkflowActivity)component).Parameters;
                    if (parameters != null)
                    {
                        if (value == null)
                            // Remove the binding from the ParameterBindings collection
                            parameters.Remove(this.Name);
                        else
                        {
                            // Add the binding to the ParameterBindings collection
                            WorkflowParameterBinding binding = null;
                            if (parameters.Contains(this.Name))
                                binding = parameters[this.Name];
                            else
                            {
                                binding = new WorkflowParameterBinding(this.Name);
                                parameters.Add(binding);
                            }

                            // Set the binding value on the ParameterBindings collection correspondent binding item
                            if (value is ActivityBind)
                                binding.SetBinding(WorkflowParameterBinding.ValueProperty, value as ActivityBind);
                            else
                                binding.SetValue(WorkflowParameterBinding.ValueProperty, value);
                        }
                    }
                    // Raise the OnValueChanged event
                    OnValueChanged(component, EventArgs.Empty);
                }
                catch (Exception)
                {
                    value = oldValue;
                    throw;
                }
                finally
                {
                    if (changeService != null)
                        // Raise the OnComponentChanged event
                        changeService.OnComponentChanged(component, this, oldValue, value);
                }
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        #endregion Methods
    }

    public class CallWorkflowService : WorkflowRuntimeService
    {
        #region Methods

        public void StartWorkflow(Type workflowType,Dictionary<string,object> inparms,Guid caller,IComparable qn)
        {
            WorkflowRuntime wr = this.Runtime;
            WorkflowInstance wi = wr.CreateWorkflow(workflowType,inparms);
            wi.Start();
            ManualWorkflowSchedulerService ss = wr.GetService<ManualWorkflowSchedulerService>();
            if (ss != null)
                ss.RunWorkflow(wi.InstanceId);
            EventHandler<WorkflowCompletedEventArgs> d  = null;
            d = delegate(object o, WorkflowCompletedEventArgs e)
            {
                if (e.WorkflowInstance.InstanceId ==wi.InstanceId)
                {
                    wr.WorkflowCompleted -= d;
                    WorkflowInstance c = wr.GetWorkflow(caller);
                    c.EnqueueItem(qn, e.OutputParameters, null, null);
                }
            };
            EventHandler<WorkflowTerminatedEventArgs> te = null;
            te = delegate(object o, WorkflowTerminatedEventArgs e)
            {
                if (e.WorkflowInstance.InstanceId == wi.InstanceId)
                {
                    wr.WorkflowTerminated -= te;
                    WorkflowInstance c = wr.GetWorkflow(caller);
                    c.EnqueueItem(qn, new Exception("Called Workflow Terminated", e.Exception), null, null);
                }
            };
            wr.WorkflowCompleted += d;
            wr.WorkflowTerminated += te;
        }

        #endregion Methods
    }
}