module WebFs.Models

open CoreFs

[<CLIMutable>]
type EditCustomer={Firstname:string; Lastname:string}
[<CLIMutable>]
type EditProduct={Name:string; Cost:float}
[<CLIMutable>]
type AddProductToOrderModel={ProductId:ProductId}

