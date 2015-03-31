using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Extensions.Conventions;
using System.Diagnostics;

namespace DecoratorDemo
{
    public static class KernelExtensions
    {
        public static void BindComposite<TComposite, TCompositeElement>(this StandardKernel container) where TComposite : TCompositeElement
        {
            container.Bind(x => x.FromAssemblyContaining(typeof(TComposite))
                .SelectAllClasses()
                .InheritedFrom<TCompositeElement>()
                .Excluding<TComposite>()
                .BindAllInterfaces()
                .Configure(c => c.WhenInjectedInto<TComposite>()));

            container.Bind<TCompositeElement>().To<TComposite>();
        }

        
    }
    public interface IValidationRule
    {
        bool IsValid();
    }
    public interface IValidationRuleComposite : IValidationRule
    {
        void ValidationRuleCompose(List<IValidationRule> validationRules);
    }

    public class MyClass1 : IValidationRule
    {
        public bool IsValid()
        {
            Console.WriteLine("Valid 1");
            return true;
        }
    }
    public class MyClass2 : IValidationRule
    {
        public bool IsValid()
        {
            Console.WriteLine("NOT Valid 2");
            return false;
        }
    }

    public interface IValidationRuleCompositeConstr : IValidationRule
    {

    }
    public class ValidationRuleCompositeOriginal : IValidationRuleCompositeConstr
    {
        private readonly IEnumerable<IValidationRule> _validationRules;

        public ValidationRuleCompositeOriginal(IEnumerable<IValidationRule> validationRules)
        {
            _validationRules = validationRules;
        }

        public bool IsValid()
        {
            return _validationRules.All(x => x.IsValid());
        }
    }
}
