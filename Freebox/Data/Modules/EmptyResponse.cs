using System;
using System.Collections.Generic;
using System.Text;

namespace Freebox.Data.Modules;

/// <summary>
/// Class used when no responses are expected beyond the common ones present in the <see cref="ApiResponseBase"/> base class
/// </summary>
public class EmptyResponse : IFreeboxApiResponse
{
}