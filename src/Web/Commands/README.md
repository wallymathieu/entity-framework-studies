# Why use Command Handler?

## History

Entity framework uses [unit of work](https://martinfowler.com/eaaCatalog/unitOfWork.html)
as a pattern. This pattern does not work well with [DAL](https://en.wikipedia.org/wiki/Data_access_layer)
or common [service class](https://en.wikipedia.org/wiki/Service_(systems_architecture)).
As mentioned on Wikipedia, an indication how it should be organised is to call a
service `CreateProductService` instead of `ProductService`.

## Pattern

Unit of work keeps a list of entities. If you reuse the unit of work outside
of unit, this can lead into unintended side effects. Entitiy framework will
detect multiple threaded usage of a specific `DbContext` and throw an exception.

In order to ensure that all of the work is actually a single "unit of work"
it is good to compose the service as a command handler. That way you can use a
`DbContext` for all of the actions meant to modify the database in one class.

### How does it look

```c#
    public class CreateProductCommandHandler
    {
        ...
        public async Task<Product> Handle(CreateProductCommand command)
        {
            // a useful side benefit of this pattern is that you can store
            // the commands in a separate persistent store in order to
            // be able to have business analytics:
            await _commandLog.Store(command);

            // additional validation logic 
            var entity = new Product {
                Cost = command.Cost, 
                Name = command.Name,
                ... // additional properties
            };
            _context.Products.Add(entity);
            // the context has been used, should not be used in other commands:
            await _context.SaveChangesAsync(); 

            await _serviceBus.PublishEvent<ProductCreated>(new
            {
                ... // what to send out on service bus
            });
            return entity;
        }
    }
```

In order to be able to compose queries it's better to use extension methods for 
database queries.
```c#
    public static class ProductSalesQueryHandler
    {
        /// <summary>
        /// All of the products that has orders associated with them
        /// </summary>
        public static IQueryable<Product> WhereThereAreOrders(
            this IQueryable<Product> self, DateTime @to, DateTime @from) =>
                self.Where(p => p.ProductOrders.Any(po =>
                                                     @from <= po.Order.OrderDate
                                                     && po.Order.OrderDate <= @to));
    }
```

This makes it possible to use the query in commands and in controllers on the
same context.


## Going from there?

A service as described as a command handler service is essentially a method with
side effects. You can see `Create Product` as a method that has the signature
`Func<CreateProductCommand, Task<Product>>`. Task in c# can be seen as a way to
tell if code has external side effects (if you use Task for that). In f# the
signature would be `CreateProductCommand -> Task<Product>` (or Async<Product>).

## More about the pattern

Ayende has written about this pattern: 
 - [Command](https://ayende.com/blog/159873/design-patterns-in-the-test-of-time-command)
 - [Command, Redux](https://ayende.com/blog/159969/design-patterns-in-the-test-of-time-command-redux)



