using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Dropthings.RestApi
{ 
    [ServiceContract(Namespace = "http://dropthings.omaralzabir.com/rest", Name = "ProxyServiceRest")]
    interface IWidgetServiceRest 
    {
    }
}
