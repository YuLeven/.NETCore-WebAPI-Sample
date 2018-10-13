using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HaruGaKita.Infrastructure.Interfaces;

#pragma warning disable 1591
namespace HaruGaKita.Infrastructure.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes => throw new NotImplementedException();

        public List<string> IncludeStrings => throw new NotImplementedException();

        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
    }
}