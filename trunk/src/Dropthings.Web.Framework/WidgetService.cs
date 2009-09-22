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
                .Log(ServiceLocator.Resolve<ILogger>(), "AddWidgetInstance {0} {1} {2}", widgetId, toZone, toRow)
                .MustBeNonDefault<int>(widgetId, toZone).Do(() =>
            {
                using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
                {
                    facade.AddWidget(widgetId, toRow, 0, toZone);
                }
                
            });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void AssignPermission(string widgetPermissions)
        {
            AspectF.Define
                .Log(ServiceLocator.Resolve<ILogger>(), "AssignPermission {0}", widgetPermissions)
                .MustBeNonDefault<string>(widgetPermissions).Do(() =>
            {
                using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
                {
                    facade.AssignWidgetPermission(widgetPermissions);
                }
            });

        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ChangePageLayout(int newLayout)
        {
            AspectF.Define.Log(ServiceLocator.Resolve<ILogger>(), "ChangePageLayout {0}", newLayout).Do(() =>
            {
                using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
                {
                    facade.ModifyPageLayout(newLayout);
                }
            });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ChangeWidgetTitle(int widgetId, string newTitle)
        {
            AspectF.Define.Log(ServiceLocator.Resolve<ILogger>(), "ChangeWidgetTitle {0} {1}", widgetId, newTitle)
                .MustBeNonDefault<int>(widgetId)
                .MustBeNonNull(newTitle)
                .Do(() =>
                {
                    using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
                    {
                        facade.ChangeWidgetInstanceTitle(widgetId, newTitle);
                    }
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public string CollaspeWidgetInstance(int widgetId, string postbackUrl)
        {
            return AspectF.Define.Log(ServiceLocator.Resolve<ILogger>(), "CollaspeWidgetInstance {0} {1}", widgetId, postbackUrl)
                .MustBeNonDefault<int>(widgetId)
                .MustBeNonNull(postbackUrl)
                .Return<string>(() =>
                {
                    using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
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
            AspectF.Define.Log(ServiceLocator.Resolve<ILogger>(), "DeleteWidgetInstance {0}", widgetId)
                .MustBeNonDefault<int>(widgetId)
                .Do(() =>
                {
                    using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
                    {
                        facade.DeleteWidgetInstance(widgetId);
                    }
                    
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public string ExpandWidgetInstance(int widgetId, string postbackUrl)
        {
            return AspectF.Define.Log(ServiceLocator.Resolve<ILogger>(), "ExpandWidgetInstance {0} {1}", widgetId, postbackUrl)
                .MustBeNonDefault<int>(widgetId)
                .Return<string>(() =>
                {
                    using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
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
            return AspectF.Define.Log(ServiceLocator.Resolve<ILogger>(), "GetWidgetState {0}", widgetId)
                .MustBeNonDefault<int>(widgetId)
                .Return<string>(() =>
                {
                    using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
                    {
                        return facade.GetWidgetInstanceState(widgetId);
                    }
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void MaximizeWidgetInstance(int widgetId)
        {
            AspectF.Define.Log(ServiceLocator.Resolve<ILogger>(), "DeleteWidgetInstance {0}", widgetId)
                .MustBeNonDefault<int>(widgetId)
                .Do(() =>
                {

                    using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
                    {
                        facade.MaximizeWidget(widgetId, true);
                    }
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void MoveWidgetInstance(int instanceId, int toZoneId, int toRow)
        {
            AspectF.Define.Log(ServiceLocator.Resolve<ILogger>(), "DeleteWidgetInstance {0}", instanceId)
                .MustBeNonDefault<int>(instanceId, toZoneId)
                .Do(() =>
                {
                    using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
                    {
                        facade.MoveWidgetInstance(instanceId, toZoneId, toRow);
                    }

                    
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ResizeWidgetInstance(int widgetId, int width, int height)
        {
            AspectF.Define.Log(ServiceLocator.Resolve<ILogger>(), "ResizeWidgetInstance {0} {1} {2}", widgetId, width, height)
                .MustBeNonDefault<int>(widgetId, width, height)
                .Do(() =>
                {
                    using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
                    {
                        facade.ResizeWidgetInstance(widgetId, width, height);
                    }                    
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void RestoreWidgetInstance(int widgetId)
        {
            AspectF.Define.Log(ServiceLocator.Resolve<ILogger>(), "ResizeWidgetInstance {0}", widgetId)
                .MustBeNonDefault<int>(widgetId)
                .Do(() =>
                {
                    using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
                    {
                        facade.MaximizeWidget(widgetId, false);
                    }
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void SaveWidgetState(int widgetId, string state)
        {
            AspectF.Define.Log(ServiceLocator.Resolve<ILogger>(), "SaveWidgetState {0} {1}", widgetId, state)
                .MustBeNonDefault<int>(widgetId).MustBeNonDefault<string>(state)
                .Do(() =>
                {
                    using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
                    {
                        facade.SaveWidgetInstanceState(widgetId, state);
                    }
                });
        }

        #endregion Methods
    }
}