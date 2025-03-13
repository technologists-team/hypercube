using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Hypercube.Analyzers;

public abstract class NamingConventionAnalyzer : DiagnosticAnalyzer
{
    protected abstract string DiagnosticId { get; }
    protected abstract LocalizableString Title { get; }
    protected abstract LocalizableString MessageFormat { get; }
    protected abstract string AttributeName { get; }
    protected abstract string RegexPattern { get; }

    private DiagnosticDescriptor Rule => new(
        DiagnosticId,
        Title,
        MessageFormat,
        "Naming",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        
        context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
    }

    private void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not ClassDeclarationSyntax declaration)
            return;

        var symbol = ModelExtensions.GetDeclaredSymbol(context.SemanticModel, declaration)!;
        if (symbol is null)
            return;

        var attribute = symbol
            .GetAttributes()
            .FirstOrDefault(attribute => attribute.AttributeClass?.Name == AttributeName);

        if (attribute is null || Regex.IsMatch(symbol.Name, RegexPattern))
            return;
        
        context.ReportDiagnostic(Diagnostic.Create(Rule, declaration.Identifier.GetLocation(), symbol.Name));
    }
}