using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificationPattern
{
    public class SpecificationPrinter<T>
    {
        private static bool NeedBrackets(params ISpecification<T>[] specs)
        {
            return specs.Any(spec => !(spec is ValueSpecification<T>));
        }

        public string Visit(AndSpecification<T> spec)
        {
            var condition = string.Format("{0} AND {1}", Visit((dynamic) spec.left), Visit((dynamic) spec.right));
            if (NeedBrackets(spec.left, spec.right)) condition = string.Format("({0})", condition);
            return condition;
        }

        public string Visit(AndNotSpecification<T> spec)
        {
            var condition = string.Format("NOT ({0} AND {1})", Visit((dynamic)spec.left), Visit((dynamic)spec.right));
            return condition;
        }

        public string Visit(OrSpecification<T> spec)
        {
            var condition = string.Format("{0} OR {1}", Visit((dynamic)spec.left), Visit((dynamic)spec.right));
            if (NeedBrackets(spec.left, spec.right)) condition = string.Format("({0})", condition);
            return condition;
        }

        public string Visit(OrNotSpecification<T> spec)
        {
            var condition = string.Format("NOT ({0} OR {1})", Visit((dynamic)spec.left), Visit((dynamic)spec.right));
            if (NeedBrackets(spec.left, spec.right)) condition = string.Format("({0})", condition);
            return condition;
        }

        public string Visit(NotSpecification<T> spec)
        {
            return string.Format("NOT ({0})", Visit((dynamic)spec.other));
        }

        public string Visit(ValueSpecification<T> spec)
        {
            return string.Format("{0}", spec.value);
        }
    }
}
