using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#pragma warning disable 1591
namespace HaruGaKita.Persistence.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
    }
}