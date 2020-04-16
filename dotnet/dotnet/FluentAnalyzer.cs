using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dotnet
{
    class FluentAnalyzer
    {
        public static string DiagnosticId => "FluentDiagnosticId";
        public static string CodeFixTitle => "Fluent";
        public static string Title => "Fluent";
        public static string MessageFormat => "Fluent";
        public static string Category => "Fluent";
        public static string Description => "Fluent";
        public static DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);


        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var expression = context.Node as MemberAccessExpressionSyntax;


            /// dotToken с какого уровня вложенности надо смотреть? 
            var dotToken = expression
                .ChildTokens()
                .FirstOrDefault(t => t.IsKind(SyntaxKind.DotToken));

            if (expression.ChildNodes().Any(n => n.IsKind(SyntaxKind.InvocationExpression)) &&
                dotToken != null &&
                !dotToken.LeadingTrivia.Any(t => t.IsKind(SyntaxKind.WhitespaceTrivia)))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule,
                    expression.GetLocation(),
                    expression.GetText()));
            }
        }

        public Task<Solution> ChangeSolution()
        {
            throw new NotImplementedException();
        }
    }

    class Foo
    {
        private void LinqExample(IEnumerable<int> query)
        {
            query.Select(x => x.GetHashCode() * 2)
                .Where(x => x > 0)
                .Count();     
        }
    }
}