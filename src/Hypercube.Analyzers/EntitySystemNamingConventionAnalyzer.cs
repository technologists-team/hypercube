using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Hypercube.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class EntitySystemNamingConventionAnalyzer : NamingConventionAnalyzer
{
    protected override string DiagnosticId => Id.EntitySystemNamingConvention;
    protected override LocalizableString Title => "EntitySystem naming convention";
    protected override LocalizableString MessageFormat => "Class {0} marked with [RegisterEntitySystem] must end with System or EntitySystem";
    protected override string AttributeName => "RegisterEntitySystemAttribute";
    protected override string RegexPattern => "(System|EntitySystem)$";
}