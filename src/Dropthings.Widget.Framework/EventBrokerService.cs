using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dropthings.Widget.Framework
{
    public class EventBrokerService
    {
        public List<WeakReference> Subscribers = new List<WeakReference>();

        public void AddListener(IEventListener listerner)
        {
            this.Subscribers.Add(new WeakReference(listerner));
        }

        public void RaiseEvent(object sender, EventArgs e)
        {
            foreach (WeakReference listener in this.Subscribers)
            {
                try
                {
                    if (listener.IsAlive)
                    {
                        (listener.Target as IEventListener).AcceptEvent(sender, e);
                    }
                }
                catch (NotImplementedException)
                {
                }
                finally
                {
                }
            }
        }
    }
}
