using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using static RBTCreditControl.Repository.Paging;

namespace RBTCreditControl.Repository
{
  public static class LinqExtentions
    {
        public static IQueryable<TResult> Transform<TResult>(this IQueryable source)
        {
            var resultType = typeof(TResult);
            var resultProperties = resultType.GetProperties().Where(p => p.CanWrite);

            ParameterExpression s = Expression.Parameter(source.ElementType, "s");

            var memberBindings =
            resultProperties.Select(p =>
            Expression.Bind(typeof(TResult).GetMember(p.Name)[0], Expression.Property(s, p.Name))).OfType<MemberBinding>();

            Expression memberInit = Expression.MemberInit(
            Expression.New(typeof(TResult)),
            memberBindings
            );

            var memberInitLambda = Expression.Lambda(memberInit, s);

            var typeArgs = new[]
            {
                source.ElementType,
                memberInit.Type
            };

            var mc = Expression.Call(typeof(Queryable), "Select", typeArgs, source.Expression, memberInitLambda);

            var query = source.Provider.CreateQuery<TResult>(mc);

            return query;
        }

        public static IEnumerable<TResult> Transform<TResult>(this System.Collections.IEnumerable source)
        {
            return source.AsQueryable().Transform<TResult>();
        }

        /// <summary>
        /// This is the universal extension method for IQueryable<T> that returns one page of results
        /// and some numbers that describe the result set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query,
                                         int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();


            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }
    }
}
