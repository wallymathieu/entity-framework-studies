# Why use Command Handler?

Entity framework uses [unit of work](https://martinfowler.com/eaaCatalog/unitOfWork.html) as a pattern. This pattern does not work well with [DAL](https://en.wikipedia.org/wiki/Data_access_layer) or common [service class](https://en.wikipedia.org/wiki/Service_(systems_architecture)). As mentioned on Wikipedia, an indication how it should be organised is to call a service `CreateProductService` instead of `ProductService`.
 