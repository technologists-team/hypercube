using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Hypercube.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class EngineCoreUsageAnalyzer : DiagnosticAnalyzer
{
    private const string AttributeName = "EngineCoreAttribute";
    
    private static DiagnosticDescriptor Rule => new(
        Id.EngineCoreUsageNotAllowed,
        "EngineCore usage without permission",
        "{0} is marked with [EngineCore] but usage is not allowed",
        "Usage",
        DiagnosticSeverity.Error,
        true,
        "Usage of EngineCore members is not allowed unless explicitly permitted."
    );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

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
        if (AllowedUsage(context))
            return;
        
        if (context.Node is not InvocationExpressionSyntax invocation)
            return;

        if (ModelExtensions.GetSymbolInfo(context.SemanticModel, invocation).Symbol is not IMethodSymbol methodSymbol)
            return;
        
        if (HasEngineCoreAttribute(methodSymbol) || HasEngineCoreAttribute(methodSymbol.ContainingType))
        {
            var diagnostic = Diagnostic.Create(Rule, invocation.GetLocation(), methodSymbol.Name);
            context.ReportDiagnostic(diagnostic);
        }

        // Проверяем атрибут у возвращаемого типа
        CheckEngineCoreUsage(context, methodSymbol.ReturnType, invocation.GetLocation());

        // Проверяем атрибут у параметров метода
        foreach (var parameter in methodSymbol.Parameters)
        {
            CheckEngineCoreUsage(context, parameter.Type, invocation.GetLocation());
        }
    }

    private void AnalyzeFieldOrPropertyAccess(SyntaxNodeAnalysisContext context)
    {
        if (AllowedUsage(context))
            return;
        
        var memberAccess = (MemberAccessExpressionSyntax)context.Node;
        var symbol = ModelExtensions.GetSymbolInfo(context.SemanticModel, memberAccess).Symbol;

        if (symbol is IFieldSymbol fieldSymbol)
        {
            // Проверяем атрибут у поля и его содержащего типа
            if (HasEngineCoreAttribute(fieldSymbol) || HasEngineCoreAttribute(fieldSymbol.ContainingType))
            {
                var diagnostic = Diagnostic.Create(Rule, memberAccess.GetLocation(), fieldSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }

            // Проверяем атрибут у типа поля
            CheckEngineCoreUsage(context, fieldSymbol.Type, memberAccess.GetLocation());
        }
        else if (symbol is IPropertySymbol propertySymbol)
        {
            // Проверяем атрибут у свойства и его содержащего типа
            if (HasEngineCoreAttribute(propertySymbol) || HasEngineCoreAttribute(propertySymbol.ContainingType))
            {
                var diagnostic = Diagnostic.Create(Rule, memberAccess.GetLocation(), propertySymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }

            // Проверяем атрибут у типа свойства
            CheckEngineCoreUsage(context, propertySymbol.Type, memberAccess.GetLocation());
        }
    }

    private void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context)
    {
        if (AllowedUsage(context))
            return;
        
        var objectCreation = (ObjectCreationExpressionSyntax)context.Node;
        var typeSymbol = ModelExtensions.GetSymbolInfo(context.SemanticModel, objectCreation).Symbol as INamedTypeSymbol;

        if (typeSymbol != null)
        {
            // Проверяем атрибут у типа
            if (HasEngineCoreAttribute(typeSymbol))
            {
                var diagnostic = Diagnostic.Create(Rule, objectCreation.GetLocation(), typeSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }

            // Проверяем атрибут у параметров конструктора
            foreach (var constructor in typeSymbol.Constructors)
            {
                foreach (var parameter in constructor.Parameters)
                {
                    CheckEngineCoreUsage(context, parameter.Type, objectCreation.GetLocation());
                }
            }
        }
    }

    private void CheckEngineCoreUsage(SyntaxNodeAnalysisContext context, ISymbol? symbol, Location location)
    {
        if (symbol is null || !HasEngineCoreAttribute(symbol))
            return;
        
        var diagnostic = Diagnostic.Create(Rule, location, symbol.Name);
        context.ReportDiagnostic(diagnostic);
    }

    private static bool AllowedUsage(SyntaxNodeAnalysisContext context)
    {
        var attributes = context.ContainingSymbol?.ContainingAssembly.GetAttributes();
        if (attributes is null)
            return false;
        
        foreach(var data in attributes)
        {
            if (data.AttributeClass is not { } attribute)
                continue;
            
            if (attribute.Name != "AssemblyMetadataAttribute")
                continue;

            var arguments = data.ConstructorArguments;
            if (arguments.Length != 2)
                continue;

            var key = (string?) arguments[0].Value;
            var value = (string?) arguments[1].Value;
            
            if (key is null || value is null)
                continue;
            
            if (key != "AllowEngineCoreUsage")
                continue;
            
            if (!bool.TryParse(value, out var result))
                continue;

            return result;
        }
        
        return false;
    }

    private static bool HasEngineCoreAttribute(ISymbol? symbol)
    {
        return symbol?.GetAttributes()
            .Any(attr => attr.AttributeClass?.Name == AttributeName) ?? false;
    }
}