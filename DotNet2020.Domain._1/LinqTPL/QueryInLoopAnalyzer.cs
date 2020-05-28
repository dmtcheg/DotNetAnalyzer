using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet2020.Domain._1
{
    /// <summary>
    /// При использовании Select в маппинге к связанной коллекции нужно всегда добавлять ToList. Это лечит n+1 в EF.
    /// Не должно быть IQueryable.ToList() в цикле. 
    /// Запросы к БД в цикле запрещены. Используйте .Contains или подзапросы.
    /// </summary>
    class QueryInLoopAnalyzer
    {
        public const string DiagnosticId = "ToListDiagnosticId";
        public const string CodeFixTitle = "make the line shorter";
        const string Title = "queries in loop";
        const string MessageFormat = "do not use IQueryable in loops";
        const string Category = "LinqTPL";
        const string Description = "do not use IQueryable in loops";
        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var semanticModel = context.SemanticModel;
            var statement = context.Node as StatementSyntax;
            StatementSyntax loopStatement;
            
            // другие способы получисть тип цикла? оператор ??= недоступен.
            loopStatement = statement as ForEachStatementSyntax;
            if (statement is ForStatementSyntax) loopStatement = statement as ForStatementSyntax;
            if (statement is WhileStatementSyntax) loopStatement = statement as WhileStatementSyntax;
            if (statement is DoStatementSyntax) loopStatement = statement as DoStatementSyntax;

            var queries = loopStatement.DescendantNodes(subNode =>
            {
                var symbol = semanticModel.GetSymbolInfo(subNode).Symbol;
                return symbol != null && symbol.ContainingSymbol.ToString() == "System.Linq.Queryable";
            });

            foreach(var q in queries)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, q.GetLocation(), q.GetText()));
            }
        }
    }
}