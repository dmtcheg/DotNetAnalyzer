using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet
{
    static class LineLengthAnalyzer
    {
        public static string DiagnosticId => "LineLengthDiagnosticId";
        static string CodeFixTitle => "make the line shorter";
        static string Title => "line is too long";
        static string MessageFormat => $"line should be shorter than {MaxLength} symbols";
        static string Category => "Formatting";
        static string Description => $"line should be shorter than {MaxLength} symbols";
        public static DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        private static int MaxLength { get; set; } = 80;

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var node = context.Node; 
            var trivia = node.GetTrailingTrivia().FirstOrDefault(t => t.IsKind(SyntaxKind.EndOfLineTrivia));

            if (trivia != null &&
                (trivia.SpanStart - node.SpanStart) < MaxLength)
            {
                return;
            }

            context.ReportDiagnostic(
                Diagnostic.Create(
                    new DiagnosticDescriptor(
                        DiagnosticId,
                        Title,
                        MessageFormat,
                        Category,
                        DiagnosticSeverity.Warning,
                        true,
                        description: Description),
                    node.GetLocation(),
                    node.GetText()));
        }

        public static void AnalyzeTree(SyntaxTreeAnalysisContext context)
        {
            var root = context.Tree.GetRoot();
        }

        public static Task<Solution> ChangeSolution()
        {
            throw new NotImplementedException();
        }
    }
}