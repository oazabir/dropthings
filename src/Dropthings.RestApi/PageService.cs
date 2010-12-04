using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OmarALZabir.AspectF;
using Dropthings.Util;
using Dropthings.Business.Facade;
using Dropthings.Business.Facade.Context;
using System.ServiceModel.Activation;

namespace Dropthings.RestApi
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class PageService : Dropthings.RestApi.IPageService
    {
        public void ChangePageLayout(int newLayout)
        {
            AspectF.Define.HowLong(Services.Get<ILogger>(),
                "Begin: ChangePageLayout {0}".FormatWith(newLayout), "End: ChangePageLayout {0}")
                .Do(() =>
                {
                    Services.Get<Facade>().ModifyTabLayout(newLayout);
                    
                });
        }

        public void MoveTab(int pageId, int orderNo)
        {
            AspectF.Define.HowLong(Services.Get<ILogger>(),
                "Begin: MoveTab {0} {1}".FormatWith(pageId, orderNo), "End: MoveTab {0}")
                .Do(() =>
                {
                    Services.Get<Facade>().MoveTab(pageId, orderNo);
                    
                });
        }
    }
}
