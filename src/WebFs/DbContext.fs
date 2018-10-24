module WebFs.Domain
open System
open System.Collections.Generic
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema
open CoreFs
open Microsoft.EntityFrameworkCore
open System.Linq
open System.Runtime.CompilerServices

type CoreDbContext(options:DbContextOptions)=
    inherit DbContext(options)
    default this.OnModelCreating(modelBuilder:ModelBuilder)=
        modelBuilder.Entity<ProductOrder>()
                    .ToTable("OrdersToProducts")
                    .HasKey([| "ProductId"; "OrderId" |]) |> ignore
        modelBuilder.Entity<ProductOrder>()
                    .HasAnnotation("Order", ForeignKeyAttribute("OrderId"))
                    .HasAnnotation("Product", ForeignKeyAttribute("ProductId"))
                    |> ignore
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

        base.OnModelCreating(modelBuilder)

    [<DefaultValue>]val mutable private customers: DbSet<Customer>
    [<DefaultValue>]val mutable private orders: DbSet<Order>
    [<DefaultValue>]val mutable private products: DbSet<Product>
    member this.Customers with get()=this.customers and set v = this.customers<-v
    member this.Orders with get()=this.orders and set v = this.orders<-v
    member this.Products with get()=this.products and set v = this.products<-v

[<Extension>]
type QueryableExtensions()=
    [<Extension>]
    static member IncludeProducts(self:IQueryable<Order>) =
        self.Include(fun o->o.Products).Include("Products.Product")

