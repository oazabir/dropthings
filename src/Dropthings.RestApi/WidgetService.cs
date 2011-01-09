using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
//using Microsoft.ServiceModel.Web;
using System.ServiceModel.Web;
using OmarALZabir.AspectF;
using Dropthings.Util;
using Dropthings.Business.Facade;

namespace Dropthings.RestApi
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]    
    public partial class WidgetService : IWidgetService
    {
        public void AddWidgetInstance(int widgetId, int toZone, int toRow)
        {
            AspectF.Define
                .Log(Services.Get<ILogger>(), "AddWidgetInstance {0} {1} {2}", widgetId, toZone, toRow)
                .MustBeNonDefault<int>(widgetId, toZone).Do(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.AddWidgetInstance(widgetId, toRow, 0, toZone);
                    }

                });
        }

        public void ChangeWidgetTitle(int widgetInstanceId, string newTitle)
        {
            AspectF.Define.Log(Services.Get<ILogger>(), "ChangeWidgetTitle {0} {1}", widgetInstanceId, newTitle)
                .MustBeNonDefault<int>(widgetInstanceId)
                .MustBeNonNull(newTitle)
                .Do(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.ChangeWidgetInstanceTitle(widgetInstanceId, newTitle);
                    }
                });
        }

        public void CollaspeWidgetInstance(int widgetInstanceId)
        {
            AspectF.Define.Log(Services.Get<ILogger>(), "CollaspeWidgetInstance {0} {1}", widgetInstanceId)
                .MustBeNonDefault<int>(widgetInstanceId)
                .Do(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.ExpandWidget(widgetInstanceId, false);
                    }
                });
        }

        public void DeleteWidgetInstance(int widgetInstanceId)
        {
            AspectF.Define.Log(Services.Get<ILogger>(), "DeleteWidgetInstance {0}", widgetInstanceId)
                .MustBeNonDefault<int>(widgetInstanceId)
                .Do(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.DeleteWidgetInstance(widgetInstanceId);
                    }

                });
        }

        public void ExpandWidgetInstance(int widgetInstanceId)
        {
            AspectF.Define.Log(Services.Get<ILogger>(), "ExpandWidgetInstance {0} {1}", widgetInstanceId)
                .MustBeNonDefault<int>(widgetInstanceId)
                .Do(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.ExpandWidget(widgetInstanceId, true);
                    }
                });
        }

        public string GetWidgetState(int widgetInstanceId)
        {
            return AspectF.Define.Log(Services.Get<ILogger>(), "GetWidgetState {0}", widgetInstanceId)
                .MustBeNonDefault<int>(widgetInstanceId)
                .Return<string>(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        return facade.GetWidgetInstanceState(widgetInstanceId);
                    }
                });
        }

        public void MaximizeWidgetInstance(int widgetInstanceId)
        {
            AspectF.Define.Log(Services.Get<ILogger>(), "DeleteWidgetInstance {0}", widgetInstanceId)
                .MustBeNonDefault<int>(widgetInstanceId)
                .Do(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.MaximizeWidget(widgetInstanceId, true);
                    }
                });
        }

        public void MoveWidgetInstance(int instanceId, int toZoneId, int toRow)
        {
            AspectF.Define.Log(Services.Get<ILogger>(), "DeleteWidgetInstance {0}", instanceId)
                .MustBeNonDefault<int>(instanceId, toZoneId)
                .Do(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.MoveWidgetInstance(instanceId, toZoneId, toRow);
                    }
                });
        }

        public void ResizeWidgetInstance(int widgetInstanceId, int width, int height)
        {
            AspectF.Define.Log(Services.Get<ILogger>(), "ResizeWidgetInstance {0} {1} {2}", widgetInstanceId, width, height)
                .MustBeNonDefault<int>(widgetInstanceId, height)
                .Do(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.ResizeWidgetInstance(widgetInstanceId, width, height);
                    }
                });
        }

        public void RestoreWidgetInstance(int widgetInstanceId)
        {
            AspectF.Define.Log(Services.Get<ILogger>(), "ResizeWidgetInstance {0}", widgetInstanceId)
                .MustBeNonDefault<int>(widgetInstanceId)
                .Do(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.MaximizeWidget(widgetInstanceId, false);
                    }
                });
        }

        public void SaveWidgetState(int widgetInstanceId, string state)
        {
            AspectF.Define.Log(Services.Get<ILogger>(), "SaveWidgetState {0} {1}", widgetInstanceId, state)
                .MustBeNonDefault<int>(widgetInstanceId).MustBeNonDefault<string>(state)
                .Do(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.SaveWidgetInstanceState(widgetInstanceId, state);
                    }
                });
        }
    }
}
