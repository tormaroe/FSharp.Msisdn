Msisdn is .NET library that facilitates working with international telephone numbers in F#. It follows the [ITU-T](http://en.wikipedia.org/wiki/ITU-T) recommendation [E.164](http://en.wikipedia.org/wiki/E.164).

Features:

* Provides a type for representing phone numbers. A value of this type will ALWAYS be a valid phone number (it may not exist, but it will be valid).
* Validates MSISDN strings
* Normalizes MSISDN strings to its canonical representation
* Lets you extract country calling code and country name from Msisdn

## Install with NuGet

    PM> Install-Package FSharp.Msisdn

## Usage

You create an Msisdn by providing a string describing the phone number, including country calling code prefix, to the `create` function.

	// Create a fictitious Norwegian mobile number
    match Msisdn.create "4799999999" with
    | Some msisdn -> doSomethingWith msisdn
    | None -> printfn "Not a valid MSISDN"

Note that you don't have to (actually you're not allowed to) open the Msisdn module. The module will automatically be available everywhere in a project referencing FSharp.Msisdn, but you have to use qualified access.

## Msisdn module API

### T

	type T = Msisdn of string

A single case discriminated union wrapping a phone number string. YOU DON'T NEED TO REFER TO THIS TYPE DIRECTLY, AND SHOULD NOT DO SO.

### create

    val create : string -> T option

Creates an Msisdn option. If s is a valid number, returns `Some Msisdn.T`. If not valid, returns `None`. `null` is never a valid number.

By using an option type, `create` forces the caller to handle the case of invalid input, which is a good thing!

`create` will strip away and ignore certain characters normally found in human representations of phone numbers. These are all valid inputs:

    "47 99 99 99 99"   // spaces are ok
    "0047 99999999"    // leading zeros are ok
    "+47 999-99-999"   // plus and hyphens are ok
    "[47](999){99999}" // brackets are ok
    "47\t99999999\r\n" // even tabs and newlines are ok

### apply

    val apply : (string -> 'a) -> T -> 'a

Apply function f to the canonical string representation of an Msisdn.

### value

    val value : T -> string

Get the canonical string representation of an Msisdn.

When you retrieve the value from an Msisdn, the canonical string representation is given:

    Msisdn.create "0047 (999) 99-999"
    |> Option.get
    |> Msisdn.value // evaluates to "4799999999"

### isValid

    val isValid : string -> bool

Checks if string is a valid input for an Msisdn.
Verification is done for you when you create
a new Msisdn using the `create` function.

### canonicalize

    val canonicalize : string -> string

Transforms a valid Msisdn string to its canonical form.
This is done for you when you create a new Msisdn using
the `create` function.

### equals

    val equals : T -> T -> bool

### compareTo

    val compareTo : T -> T -> int

### countryCode

    val countryCode : T -> string * string

Get the country calling code prefix and the country name
from an Msisdn.

    let prefix, country = Msisdn.create "+4799999999"
                          |> Option.get
                          |> Msisdn.countryCode
    // val prefix = "47"
    // val country = "Norway"

## More examples

This is how you may use FSharp.Msisdn to get the canonical representation from a list potensially containing some invalid numbers:

    let numbers = ["47 91 92 93 94"
                   "1-555-32-654"
                   "00362347657"
                   "+87899877889x"]
    
    numbers
    |> List.map Msisdn.create
    |> List.choose (Option.map Msisdn.value)

    // Result:
    // ["4791929394"; "155532654"; "362347657"]

Of course the same thing can be done even more easily using `canonicalize` and `isValid` directly:

    numbers
    |> List.map Msisdn.canonicalize
    |> List.filter Msisdn.isValid

If you need to group your numbers by country code, you could do something like this:

    numbers
    |> List.map Msisdn.create
    |> List.choose id
    |> Seq.groupBy 
        (fun x -> let cc, _ = Msisdn.countryCode x in cc)
    
    // Result:
    // [("47", seq [Msisdn "4791929394"])
    //  ("1", seq [Msisdn "155532654"])
    //  ("36", seq [Msisdn "362347657"])]


## The MIT License (MIT)

Copyright (c) 2015 Torbjørn Marø

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.