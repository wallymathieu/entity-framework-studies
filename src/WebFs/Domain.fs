module WebFs.Domain
open System
open System.Collections.Generic
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema

[<AllowNullLiteral>][<Table("OrdersToProducts")>]
type ProductOrder(order,product)=
    new()=ProductOrder(null, null)
    [<ForeignKey("OrderId")>]
    member val Order : Order=order with get, set
    [<ForeignKey("ProductId")>]
    member val Product : Product=product with get, set
and [<AllowNullLiteral>] Order(id,orderDate,customer,version) =
    new()=Order(0,DateTime.MinValue,null,0)
    [<Column("Id")>][<Key>]
    member val OrderId =id with get, set
    member val OrderDate =orderDate with get, set
    member val Customer : Customer=customer with get, set
    member val Products =List<ProductOrder>() with get, set
    member val Version = version with get, set
    
and [<AllowNullLiteral>] Customer(id,firstname,lastname,version)=
    new()=Customer(0,"","",0)
    [<Column("Id")>][<Key>]
    member val CustomerId  =id with get, set
    member val Firstname =firstname with get, set
    member val Lastname =lastname with get, set
    member val Orders = List<Order>() with get, set
    member val Version = version with get, set
and [<AllowNullLiteral>] Product(id,cost,name,version)=
    new()=Product(0,0.0,"",0)
    [<Column("Id")>][<Key>]
    member val ProductId =id with get, set
    member val Cost =cost with get, set
    [<Column("Name")>]
    member val ProductName = name with get, set
    member val Orders = List<ProductOrder>() with get, set
    member val Version = version with get, set

open Microsoft.EntityFrameworkCore
open System.Linq
open System.Runtime.CompilerServices

type CoreDbContext(options:DbContextOptions)=
    inherit DbContext(options)
    default this.OnModelCreating(modelBuilder:ModelBuilder)=
        modelBuilder.Entity<ProductOrder>().HasKey([| "ProductId"; "OrderId" |]) |> ignore
        
        base.OnModelCreating(modelBuilder)

    [<DefaultValue>]val mutable private customers: DbSet<Customer>
    [<DefaultValue>]val mutable private orders: DbSet<Order>
    [<DefaultValue>]val mutable private products: DbSet<Product>
    member this.Customers with get()=this.customers and set v = this.customers<-v
    member this.Orders with get()=this.orders and set v = this.orders<-v
    member this.Products with get()=this.products and set v = this.products<-v

[<Extension>]
type ProductSalesQueryHandler()=
    /// All of the products that has orders associated with them
    static member WhereThereAreOrders(self:IQueryable<Product>, to':DateTime, from':DateTime) =
            self.Where(fun p -> p.Orders.Any(fun po ->
                                                    from' <= po.Order.OrderDate
                                                    && po.Order.OrderDate <= to'))
