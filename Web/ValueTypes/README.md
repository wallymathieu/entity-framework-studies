# What are value types?

Value types are types that help you organise your code, but don't have an identity of their own.

Common value types are enums.

I prefer to wrap existing identifiers in order to make it clearer what they are. This is helpful if your app grows and 
you deal with multiple identifiers. You can see this pattern in functional languages as something called [newtype](https://wiki.haskell.org/Newtype). 
In Haskell you might wrap make it even more clear by having `UserName` as a type instead of a `String`.

Due to the complexity involved in making wrapped types in c# (that behave in expected manner), I usually reserve the usage for identifiers.
