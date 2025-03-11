using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using DiagnosticDescriptor = Microsoft.CodeAnalysis.DiagnosticDescriptor;
using DiagnosticSeverity = Microsoft.CodeAnalysis.DiagnosticSeverity;
using LanguageNames = Microsoft.CodeAnalysis.LanguageNames;
using SyntaxKind = Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace Hypercube.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class EngineCoreUsageAnalyzer : DiagnosticAnalyzer
{
    private const string EngineCoreAttributeName = "EngineCoreAttribute";

    public static readonly DiagnosticDescriptor EngineCoreUsageRule = new(
        id: Id.EngineCoreUsageNotAllowed,
        title: "EngineCore usage without permission",
        messageFormat: "{0} is marked with [EngineCore] but usage is not allowed",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Usage of EngineCore members is not allowed unless explicitly permitted."
    );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [EngineCoreUsageRule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        
        context.RegisterSyntaxNodeAction(AnalyzeMethodInvocation, SyntaxKind.InvocationExpression);
        context.RegisterSyntaxNodeAction(AnalyzeFieldOrPropertyAccess, SyntaxKind.SimpleMemberAccessExpression);
        context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, SyntaxKind.ObjectCreationExpression);
    }

    private void AnalyzeMethodInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;
        var methodSymbol = context.SemanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;

        CheckEngineCoreUsage(context, methodSymbol, invocation.GetLocation());
    }

    private void AnalyzeFieldOrPropertyAccess(SyntaxNodeAnalysisContext context)
    {
        var memberAccess = (MemberAccessExpressionSyntax)context.Node;
        var symbol = context.SemanticModel.GetSymbolInfo(memberAccess).Symbol;

        if (symbol is IFieldSymbol or IPropertySymbol)
            CheckEngineCoreUsage(context, symbol, memberAccess.GetLocation());
    }

    private void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context)
    {
        var objectCreation = (ObjectCreationExpressionSyntax)context.Node;
        var typeSymbol = context.SemanticModel.GetSymbolInfo(objectCreation).Symbol as INamedTypeSymbol;

        CheckEngineCoreUsage(context, typeSymbol, objectCreation.GetLocation());
    }

    private void CheckEngineCoreUsage(SyntaxNodeAnalysisContext context, ISymbol? symbol, Location location)
    {
        if (symbol is null || !HasEngineCoreAttribute(symbol))
            return;
        
        var diagnostic = Diagnostic.Create(EngineCoreUsageRule, location, symbol.Name);
        context.ReportDiagnostic(diagnostic);
    }

    private bool HasEngineCoreAttribute(ISymbol? symbol)
    {
        return symbol?.GetAttributes()
            .Any(attr => attr.AttributeClass?.Name == EngineCoreAttributeName) ?? false;
    }
}