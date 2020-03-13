// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Appeler ConfigureAwait sur la tâche attendue", Justification = "THe performance gains in this scenario are not important enough for the increased code and loss of readability", Scope = "module")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Qualité du code", "IDE0051:Supprimer les membres privés non utilisés", Justification = "Used by serialization", Scope = "member", Target = "~P:Freebox.Data.ApiInfo._apiVersion")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Styles d'affectation de noms", Justification = "Already an ApiVersion attribute", Scope = "member", Target = "~P:Freebox.Data.ApiInfo._apiVersion")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Qualité du code", "IDE0051:Supprimer les membres privés non utilisés", Justification = "Used by serialization", Scope = "member", Target = "~P:Freebox.Data.ApiInfo._isHttpsAvailable")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Styles d'affectation de noms", Justification = "Already an ApiVersion attribute", Scope = "member", Target = "~P:Freebox.Data.ApiInfo._isHttpsAvailable")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Qualité du code", "IDE0051:Supprimer les membres privés non utilisés", Justification = "Used by serialization", Scope = "member", Target = "~P:Freebox.Data.ApiInfo._httpsPort")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Styles d'affectation de noms", Justification = "Already an ApiVersion attribute", Scope = "member", Target = "~P:Freebox.Data.ApiInfo._httpsPort")]


[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Qualité du code", "IDE0051:Supprimer les membres privés non utilisés", Justification = "Used by reflection", Scope = "member", Target = "~P:Freebox.Data.Modules.RRD.Requests.Fetch._dateStart")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Qualité du code", "IDE0051:Supprimer les membres privés non utilisés", Justification = "Used by reflection", Scope = "member", Target = "~P:Freebox.Data.Modules.RRD.Requests.Fetch._dateEnd")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Styles d'affectation de noms", Justification = "Already an ApiVersion attribute", Scope = "member", Target = "~P:Freebox.Data.Modules.RRD.Requests.Fetch._dateStart")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Styles d'affectation de noms", Justification = "Already an ApiVersion attribute", Scope = "member", Target = "~P:Freebox.Data.Modules.RRD.Requests.Fetch._dateEnd")]