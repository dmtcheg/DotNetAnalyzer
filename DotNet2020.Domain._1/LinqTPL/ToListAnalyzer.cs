using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._1.LinqTPL
{
    /// <summary>
    /// При использовании Select в маппинге к связанной коллекции нужно всегда добавлять ToList. Это лечит n+1 в EF.
    /// Не должно быть IQueryable.ToList() в цикле. 
    /// Запросы к БД в цикле запрещены. Используйте .Contains или подзапросы.
    /// </summary>
    class ToListAnalyzer
    {
        public const string DiagnosticId = "ToListDiagnosticId";
        public const string CodeFixTitle = "make the line shorter";
        const string Title = "warning descr";
        const string MessageFormat = "";
        const string Category = "LinqTPL";
        const string Description = "";
        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, 
            Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);



    }
}
