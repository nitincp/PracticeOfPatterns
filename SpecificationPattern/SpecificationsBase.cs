using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SpecificationPattern
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T entity);
        ISpecification<T> And(ISpecification<T> other);
        ISpecification<T> AndNot(ISpecification<T> other);
        ISpecification<T> Or(ISpecification<T> other);
        ISpecification<T> OrNot(ISpecification<T> other);
        ISpecification<T> Not();
    }

    public abstract class LinqSpecification<T> : CompositeSpecification<T>
    {
        public abstract Expression<Func<T, bool>> AsExpression();

        public override bool IsSatisfiedBy(T entity)
        {
            Func<T, bool> predicate = AsExpression().Compile();
            return predicate(entity);
        }
    }

    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T entity);

        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public ISpecification<T> AndNot(ISpecification<T> other)
        {
            return new AndNotSpecification<T>(this, other);
        }

        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        public ISpecification<T> OrNot(ISpecification<T> other)
        {
            return new OrNotSpecification<T>(this, other);
        }

        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }

    }

    public class ValueSpecification<T> : CompositeSpecification<T>
    {
        public readonly object value;

        public ValueSpecification(object value)
        {
            this.value = value;
        }

        public override bool IsSatisfiedBy(T entity)
        {
            return true;
        }
    }

    public class AndSpecification<T> : CompositeSpecification<T>
    {
        public readonly ISpecification<T> left;
        public readonly ISpecification<T> right;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.left = left;
            this.right = right;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return left.IsSatisfiedBy(candidate) && right.IsSatisfiedBy(candidate);
        }
    }

    public class AndNotSpecification<T> : CompositeSpecification<T>
    {
        public readonly ISpecification<T> left;
        public readonly ISpecification<T> right;

        public AndNotSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.left = left;
            this.right = right;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return left.IsSatisfiedBy(candidate) && right.IsSatisfiedBy(candidate) != true;
        }
    }

    public class OrSpecification<T> : CompositeSpecification<T>
    {
        public readonly ISpecification<T> left;
        public readonly ISpecification<T> right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.left = left;
            this.right = right;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return left.IsSatisfiedBy(candidate) || right.IsSatisfiedBy(candidate);
        }
    }
    public class OrNotSpecification<T> : CompositeSpecification<T>
    {
        public readonly ISpecification<T> left;
        public readonly ISpecification<T> right;

        public OrNotSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.left = left;
            this.right = right;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return left.IsSatisfiedBy(candidate) || right.IsSatisfiedBy(candidate) != true;
        }
    }

    public class NotSpecification<T> : CompositeSpecification<T>
    {
        public readonly ISpecification<T> other;

        public NotSpecification(ISpecification<T> other)
        {
            this.other = other;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return !other.IsSatisfiedBy(candidate);
        }
    }
}
