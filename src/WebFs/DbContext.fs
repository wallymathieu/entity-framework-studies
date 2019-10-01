module WebFs.Domain
open System
open System.Collections.Generic
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema
open CoreFs
open Microsoft.EntityFrameworkCore
open System.Linq
open System.Runtime.CompilerServices
open System.Threading.Tasks
open FSharp.Control.Tasks.Builders

[<Interface>]
type ICoreDbContext=
    abstract member Customers: DbSet<Customer> with get
    abstract member Orders: DbSet<Order> with get
    abstract member Products: DbSet<Product> with get
    abstract member SaveChangesAsync: unit->Task<unit>
    abstract member AddAsync<'t> : 't -> Task<unit> 

type CoreDbContext(options:DbContextOptions)=
    inherit DbContext(options)
    default this.OnModelCreating(modelBuilder:ModelBuilder)=
        modelBuilder.Entity<Order>()
                    .Property(fun o->o.OrderId).HasColumnName("Id") |> ignore
        modelBuilder.Entity<Customer>()
                    .Property(fun o->o.CustomerId).HasColumnName("Id") |> ignore
        modelBuilder.Entity<Product>()
                    .Property(fun o->o.ProductId).HasColumnName("Id") |> ignore
        modelBuilder.Entity<Product>()
                    .Property(fun o->o.ProductName).HasColumnName("Name") |> ignore
        modelBuilder.Entity<Product>()
                    .Property(fun o->o.Cost) |> ignore
        modelBuilder.Entity<ProductOrder>()
                    .ToTable("OrdersToProducts")
                    .HasKey([| "ProductId"; "OrderId" |]) |> ignore
        modelBuilder.Entity<ProductOrder>().HasAnnotation("Order", ForeignKeyAttribute("OrderId"))
            |> ignore
        modelBuilder.Entity<ProductOrder>().HasAnnotation("Product", ForeignKeyAttribute("ProductId"))
            |> ignore
        base.OnModelCreating(modelBuilder)

    [<DefaultValue>]val mutable private customers: DbSet<Customer>
    [<DefaultValue>]val mutable private orders: DbSet<Order>
    [<DefaultValue>]val mutable private products: DbSet<Product>
    member this.Customers with get()=this.customers and set v = this.customers<-v
    member this.Orders with get()=this.orders and set v = this.orders<-v
    member this.Products with get()=this.products and set v = this.products<-v

    interface ICoreDbContext with 
      member this.SaveChangesAsync() = task { let! _ = this.SaveChangesAsync()
        return () }
      member this.AddAsync(t) = task { let! _ = this.AddAsync t
        return () }
      member this.Customers = this.customers
      member this.Orders = this.orders
      member this.Products = this.products

[<Extension>]
type QueryableExtensions()=
    [<Extension>]
    static member IncludeProducts(self:IQueryable<Order>) =
        self.Include(fun o->o.Products).Include("Products.Product")

