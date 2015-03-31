using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorDemo
{
    public class RegistryModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<Products>().ToSelf();
            
            Bind<IPriceHandler>().To<Products>().WhenInjectedInto<PriceVAT>();
            Bind<IPriceHandler>().To<PriceVAT>().WhenInjectedInto<PricePromotion>();
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel(new RegistryModule());

            kernel.Bind<IHandle<int>>().To<IntHandlerTwo>();
            kernel.Bind<IHandle<int>>().To<IntHandlerNine>();
            kernel.Bind<IHandle<int>>().To<IntHandlerOne>();
            kernel.Bind<IHandle<string>>().To<StringHandler>();
            kernel.Bind(typeof(IHandlerDecorator<>)).To(typeof(HandlerDecorator<>));

            


            kernel.Bind<IValidationRule>().To<MyClass1>();
            kernel.Bind<IValidationRule>().To<MyClass2>();
            kernel.Bind<IValidationRuleCompositeConstr>().To<ValidationRuleCompositeOriginal>();

            //IEnumerable<IValidationRule> rules = kernel.GetAll<IValidationRule>();
            //Ninject.Parameters.ConstructorArgument therules = new Ninject.Parameters.ConstructorArgument("therules", rules);

            IHandlerDecorator<int> decor = kernel.Get<IHandlerDecorator<int>>();
            decor.Handle(5);


            IValidationRuleCompositeConstr composite = kernel.Get<IValidationRuleCompositeConstr>();            
            Console.WriteLine(string.Format("{0}", composite.IsValid()));

            var product = kernel.Get<PricePromotion>();            
            Console.WriteLine(string.Format("Price : {0}", product.getPrice()));
            Console.ReadKey();

            //var level = new Random().Next(1, 20);

            //var ninjaBuilder = NinjaBuilder
            //    .CreateNinjaBuilder()
            //    .AtLevel(level)
            //    .WithShurikens(10)
            //    .WithSkill("hideinshadows")
            //        .When(() => level > 5);

            //var ninja = ninjaBuilder.Build();

            //Console.WriteLine("Ninja Level is: {0}", ninja.Level);
            //Console.WriteLine("Ninja Skill is: {0}", !string.IsNullOrEmpty(ninja.Skill) ? ninja.Skill : "This ninja is not experienced enough for a skill.");

            //Console.ReadLine();

            ////Test for Growl Notification
            //Product product = ProductBuilder
            //    .CreateProduct()
            //    .Named("Darth Vader")
            //    .ManufacturedBy("Kenner")
            //    .Costs(.99)
            //    .Priced(4.99);

            //Console.WriteLine("Product - Name: {0} Price: {1} Manufacturer: {2}", product.Name, product.Price, product.Manufacturer);
            //Console.Read();
        }
    }

    public interface IPriceHandler
    {
        int getPrice();
    }

    public class Products : IPriceHandler
    {
        public int Price { get; set; }
        public int getPrice()
        {
            return 0;
        }
    }


    // use ninject for this factory
    public abstract class PriceDecorator : IPriceHandler
    {
        protected IPriceHandler _product;

        public PriceDecorator(IPriceHandler product)
        {
            _product = product;
        }

        public  int getPrice()
        {
            return _product.getPrice();
        }
    }

    public class PriceVAT : IPriceHandler
    {
        IPriceHandler _product;

        public PriceVAT(IPriceHandler product)
        {
            _product = product;
        }

        public  int getPrice()
        {
            Console.WriteLine("applying tax ..");
            return _product.getPrice() + 2;
        }
    }

    public class PricePromotion : IPriceHandler
    {
        IPriceHandler _product;
        public PricePromotion(IPriceHandler product)
        {
            _product = product;
        }
        public  int getPrice()
        {
            Console.WriteLine("using promotion ...");
            return _product.getPrice() + 5;
        }
    }
}
