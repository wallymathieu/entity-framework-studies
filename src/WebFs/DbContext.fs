﻿module WebFs.Domain
open System
open CoreFs
open Microsoft.EntityFrameworkCore
open EntityFrameworkCore.FSharp.Extensions
open System.Linq
open System.Runtime.CompilerServices
open System.Threading.Tasks
open Microsoft.EntityFrameworkCore.Storage.ValueConversion
open Microsoft.FSharp.Linq.RuntimeHelpers
open System.Linq.Expressions

type FSharpValueConverter()=
    static member inline Create<'wrappertype,'simpletype>( ctor: ('simpletype->'wrappertype), unwrap: ('wrappertype->'simpletype) )=
        let toExpression (``f# lambda`` : Quotations.Expr<'a>) =
            ``f# lambda``
            |> LeafExpressionConverter.QuotationToExpression
            |> unbox<Expression<'a>>
        let from' = <@ Func<_, _>(ctor) @> |> toExpression
        let to' = <@ Func<_, _>(unwrap) @> |> toExpression
        ValueConverter<'wrappertype, 'simpletype>(convertFromProviderExpression= from', convertToProviderExpression= to')

type CoreDbContext(options:DbContextOptions)=
    inherit DbContext(options)
    default this.OnConfiguring(options: DbContextOptionsBuilder) =
        options.UseFSharpTypes() |> ignore
    default this.OnModelCreating(modelBuilder:ModelBuilder) =
        modelBuilder.RegisterOptionTypes()
        let orderIdConverter = FSharpValueConverter.Create(OrderId, OrderId.unwrap)
        let productIdConverter = FSharpValueConverter.Create(ProductId, ProductId.unwrap)
        let customerIdConverter = FSharpValueConverter.Create(CustomerId, CustomerId.unwrap)
        modelBuilder.Entity<Order>()
                    .Property(fun o->o.OrderId).HasColumnName("Id")
                    .HasConversion(orderIdConverter)
                    .UsePropertyAccessMode(PropertyAccessMode.PreferProperty) |> ignore
        modelBuilder.Entity<Customer>()
                    .Property(fun o->o.CustomerId).HasColumnName("Id")
                    .HasConversion(customerIdConverter)
                    .UsePropertyAccessMode(PropertyAccessMode.PreferProperty) |> ignore
        modelBuilder.Entity<Product>(fun e->
                        e.Property(fun o->o.Cost) |> ignore
                        e.Property(fun o->o.ProductName).HasColumnName("Name") |> ignore
                        e.Property(fun o->o.ProductId).HasColumnName("Id")
                         .HasConversion(productIdConverter)
                         .UsePropertyAccessMode(PropertyAccessMode.PreferProperty) |> ignore
                        e.HasMany<Order>(fun p->p.Orders)
                         .WithMany(fun o->o.Products)
                         .UsingEntity<ProductOrder>() |> ignore
                    ) |> ignore

        modelBuilder.Entity<ProductOrder>()
                    .ToTable("OrdersToProducts")
                    .HasKey([| "ProductId"; "OrderId" |]) |> ignore
        base.OnModelCreating(modelBuilder)

    [<DefaultValue>]val mutable private customers: DbSet<Customer>
    [<DefaultValue>]val mutable private orders: DbSet<Order>
    [<DefaultValue>]val mutable private products: DbSet<Product>
    [<DefaultValue>]val mutable private productOrders: DbSet<ProductOrder>
    member this.Customers with get()=this.customers and set v = this.customers<-v
    member this.Orders with get()=this.orders and set v = this.orders<-v
    member this.Products with get()=this.products and set v = this.products<-v
    member this.ProductOrders with get()=this.productOrders and set v = this.productOrders<-v


[<Extension>]
type QueryableExtensions()=
    [<Extension>]
    static member IncludeProducts(self:IQueryable<Order>) =
        self.Include(fun o->o.Products)

