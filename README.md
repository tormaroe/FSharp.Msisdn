Msisdn is .NET library that facilitates working with international telephone numbers in F#. It follows the [ITU-T](http://en.wikipedia.org/wiki/ITU-T) recommendation [E.164](http://en.wikipedia.org/wiki/E.164).

## Create an Msisdn

You create a value of the `Msisdn.T` type by providing a string describing the phone number, including country calling code prefix, to the `create` function.

	// Create a fictitious Norwegian mobile number
    match Msisdn.create "4799999999" with
    | Some msisdn -> doSomethingWith msisdn
    | None -> printfn "Not a valid MSISDN"

`create` will strip away and ignore certain characters normally found in human representations of phone numbers. These are all valid inputs to `create`:

    "47 99 99 99 99"   // spaces are ok
    "0047 99999999"    // leading zeros are ok
    "+47 999-99-999"   // plus and hyphens are ok
    "[47](999){99999}" // brackets are ok
    "47\t99999999\r\n" // even tabs and newlines are ok

When you retrieve the value from an Msisdn, the canonical string representation is given:

    Msisdn.create "0047 (999) 99-999"
    |> Option.get
    |> Msisdn.value // evaluates to "4799999999"

## Country calling codes

You can get the country calling code prefix and name from an Msisdn:


    let prefix, country = Msisdn.create "+4799999999"
                          |> Option.get
                          |> Msisdn.countryCode
    // val prefix = "47"
    // val country = "Norway"

## Msisdn module API

	(* A single case union type wrapping a phone
	   number string. YOU DON'T NEED TO REFER TO
	   THIS TYPE DIRECTLY, AND SHOULD NOT DO SO. *)
	type T = Msisdn of string

    (* Creates an Msisdn option
       If s is a valid number, returns Some Msisdn.T
       If not valid, returns None *)
    val create : string -> T option

    (* Apply function f to the canonical string  
	   representation of an Msisdn. *)
    val apply : (string -> 'a) -> T -> 'a

    (* Get the canonical string representation of an Msisdn. *)
    val value : T -> string

    val equals : T -> T -> bool

    val compareTo : T -> T -> int

    (* Checks if s is a valid input for an Msisdn.
       Verification is done for you when you create
       a new Msisdn using the create function. *)
    val isValid : string -> bool

    (* Transforms a valid Msisdn string to its canonical form.
       This is done for you when you create a new Msisdn using
       the create function. *)
    val canonicalize : string -> string

    (* Get the country calling code prefix and the country name
       from an Msisdn. *)
    val countryCode : T -> string * string