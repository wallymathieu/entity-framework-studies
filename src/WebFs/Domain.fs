module WebFs.Domain
open System
open System.Collections.Generic
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema

[<AllowNullLiteral>]
type ProductOrder()=
    [<Column("Id")>][<Key>]
    member val ProductOrderId : int=0 with get, set
    member val Order : Order=null with get, set
    member val Product : Product=null with get, set
    member val Version = 0 with get, set
and [<AllowNullLiteral>] Order() =
    [<Column("Id")>][<Key>]
    member val OrderId : int=0 with get, set
    member val OrderDate =DateTime.MinValue with get, set
    member val Customer : Customer=null with get, set
    member val ProductOrders =List<ProductOrder>() with get, set
    member val Version = 0 with get, set
    
and [<AllowNullLiteral>] Customer()=
    [<Column("Id")>][<Key>]
    member val CustomerId : int=0 with get, set
    member val Firstname ="" with get, set
    member val Lastname ="" with get, set
    member val Orders = List<Order>() with get, set
    member val Version = 0 with get, set
and [<AllowNullLiteral>] Product()=
    [<Column("Id")>][<Key>]
    member val ProductId : int=0 with get, set
    member val Cost : float=0.0 with get, set
    [<Column("Name")>]
    member val ProductName = "" with get, set
    member val ProductOrders = List<ProductOrder>() with get, set
    member val Version = 0 with get, set

open Microsoft.EntityFrameworkCore
open System.Linq
open System.Runtime.CompilerServices

type CoreDbContext(options:DbContextOptions)=
    inherit DbContext(options)
    [<DefaultValue>]val mutable customers: DbSet<Customer>
    [<DefaultValue>]val mutable orders: DbSet<Order>
    [<DefaultValue>]val mutable products: DbSet<Product>
    member this.Customers with get()=this.customers and set v = this.customers<-v
    member this.Orders with get()=this.orders and set v = this.orders<-v
    member this.Products with get()=this.products and set v = this.products<-v

[<Extension>]
type ProductSalesQueryHandler()=
    /// All of the products that has orders associated with them
    static member WhereThereAreOrders(self:IQueryable<Product>, to':DateTime, from':DateTime) =
            self.Where(fun p -> p.ProductOrders.Any(fun po ->
                                                    from' <= po.Order.OrderDate
                                                    && po.Order.OrderDate <= to'))
