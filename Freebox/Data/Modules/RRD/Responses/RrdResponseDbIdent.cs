using Freebox.Data.Modules.RRD.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freebox.Data.Modules.RRD.Responses
{
    public abstract class RrdResponseDbIdent
    {
       internal abstract RrdDb Db { get; }
    }
}
