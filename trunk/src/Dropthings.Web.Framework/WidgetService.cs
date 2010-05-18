#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Web.Framework
{
    using System.Web.Script.Services;
    using System.Web.Services;


    using Dropthings.Business.Facade;
    using Dropthings.Business.Facade.Context;
    using Dropthings.Util;
    using OmarALZabir.AspectF;
    
    /// <summary>
    /// Summary description for WidgetService
    /// </summary>
    public class WidgetService : WebServiceBase
    {
        #region Constructors

        public WidgetService()
        {
            //Uncomment the following line if using designed components
            //InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
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

        //[WebMethod]
        //[ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        //public void AssignPermission(string widgetPermissions)
        //{
        //    AspectF.Define
        //        .Log(Services.Get<ILogger>(), "AssignPermission {0}", widgetPermissions)
        //        .MustBeNonDefault<string>(widgetPermissions).Do(() =>
        //    {
        //        var facade = Services.Get<Facade>();
        //        {
        //            facade.AssignWidgetPermission(widgetPermissions);
        //        }
        //    });

        //}

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ChangePageLayout(int newLayout)
        {
            AspectF.Define.Log(Services.Get<ILogger>(), "ChangePageLayout {0}", newLayout).Do(() =>
            {
                var facade = Services.Get<Facade>();
                {
                    facade.ModifyPageLayout(newLayout);
                }
            });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ChangeWidgetTitle(int widgetId, string newTitle)
        {
            AspectF.Define.Log(Services.Get<ILogger>(), "ChangeWidgetTitle {0} {1}", widgetId, newTitle)
                .MustBeNonDefault<int>(widgetId)
                .MustBeNonNull(newTitle)
                .Do(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.ChangeWidgetInstanceTitle(widgetId, newTitle);
                    }
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public string CollaspeWidgetInstance(int widgetId, string postbackUrl)
        {
            return AspectF.Define.Log(Services.Get<ILogger>(), "CollaspeWidgetInstance {0} {1}", widgetId, postbackUrl)
                .MustBeNonDefault<int>(widgetId)
                .MustBeNonNull(postbackUrl)
                .Return<string>(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.ExpandWidget(widgetId, false);
                    }

                    return postbackUrl;
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void DeleteWidgetInstance(int widgetId)
        {
            AspectF.Define.Log(Services.Get<ILogger>(), "DeleteWidgetInstance {0}", widgetId)
                .MustBeNonDefault<int>(widgetId)
                .Do(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.DeleteWidgetInstance(widgetId);
                    }
                    
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public string ExpandWidgetInstance(int widgetId, string postbackUrl)
        {
            return AspectF.Define.Log(Services.Get<ILogger>(), "ExpandWidgetInstance {0} {1}", widgetId, postbackUrl)
                .MustBeNonDefault<int>(widgetId)
                .Return<string>(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.ExpandWidget(widgetId, true);
                    }
                    
                    return postbackUrl;
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, XmlSerializeString = true)]
        public string GetWidgetState(int widgetId)
        {
            return AspectF.Define.Log(Services.Get<ILogger>(), "GetWidgetState {0}", widgetId)
                .MustBeNonDefault<int>(widgetId)
                .Return<string>(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        return facade.GetWidgetInstanceState(widgetId);
                    }
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void MaximizeWidgetInstance(int widgetId)
        {
            AspectF.Define.Log(Services.Get<ILogger>(), "DeleteWidgetInstance {0}", widgetId)
                .MustBeNonDefault<int>(widgetId)
                .Do(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.MaximizeWidget(widgetId, true);
                    }
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
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

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ResizeWidgetInstance(int widgetId, int width, int height)
        {
            AspectF.Define.Log(Services.Get<ILogger>(), "ResizeWidgetInstance {0} {1} {2}", widgetId, width, height)
                .MustBeNonDefault<int>(widgetId, height)
                .Do(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.ResizeWidgetInstance(widgetId, width, height);
                    }                    
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void RestoreWidgetInstance(int widgetId)
        {
            AspectF.Define.Log(Services.Get<ILogger>(), "ResizeWidgetInstance {0}", widgetId)
                .MustBeNonDefault<int>(widgetId)
                .Do(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.MaximizeWidget(widgetId, false);
                    }
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void SaveWidgetState(int widgetId, string state)
        {
            AspectF.Define.Log(Services.Get<ILogger>(), "SaveWidgetState {0} {1}", widgetId, state)
                .MustBeNonDefault<int>(widgetId).MustBeNonDefault<string>(state)
                .Do(() =>
                {
                    var facade = Services.Get<Facade>();
                    {
                        facade.SaveWidgetInstanceState(widgetId, state);
                    }
                });
        }

        #endregion Methods
    }
}