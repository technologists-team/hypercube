using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Hypercube.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ComponentNamingConventionAnalyzer : NamingConventionAnalyzer
{
    protected override string DiagnosticId => Id.ComponentNamingConvention;
    protected override LocalizableString Title => "Component naming convention";
    protected override LocalizableString MessageFormat => "Class {0} marked with [RegisterComponent] must end with Component";
    protected override string AttributeName => "RegisterComponentAttribute";
    protected override string RegexPattern => "Component$";
}