using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SoftDeleteQueryFilterExample.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SoftDeleteQueryFilterExample.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplySoftDeleteQueryFilter(this ModelBuilder bobTheBuilder)
        {
            //get all entities which has implemented the IDeletableEntity interface  
            var entities = bobTheBuilder.Model.GetEntityTypes().Where(x => typeof(IDeletableEntity).IsAssignableFrom(x.ClrType))?.ToList();

            //loop through all the entities and apply the lambda expression after building it for each entity
            entities.ForEach(x =>
            {
                bobTheBuilder.Entity(x.ClrType).HasQueryFilter(BuildLambdaExpression<IDeletableEntity>(y => !y.IsDeleted, x.ClrType));
            });
        }

        private static LambdaExpression BuildLambdaExpression<TInterface>(Expression<Func<TInterface, bool>> expression, Type type)
        {
            //create expression parameter
            var paramater = Expression.Parameter(type);

            //create expression body
            var body = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), paramater, expression.Body);

            //create and return the lambda expression
            return Expression.Lambda(body, paramater);
        }
    }
}
