module Tests

open NUnit.Framework
open FsUnit

let makeMsisdnAndGetValue =
    Msisdn.create >> Option.get >> Msisdn.value

[<Test>] 
let ``Should remove leading zeros`` () =
    makeMsisdnAndGetValue "004790696698" |> should equal "4790696698"
    
[<Test>] 
let ``Should remove whitespace`` () =
    makeMsisdnAndGetValue "47  906 96 698" |> should equal "4790696698"
    makeMsisdnAndGetValue "\t47\t90696698" |> should equal "4790696698"
    makeMsisdnAndGetValue "4790696698\r\n" |> should equal "4790696698"
    
[<Test>] 
let ``Should remove brackets`` () =
    makeMsisdnAndGetValue "(47) 906 96 698 (" |> should equal "4790696698"
    makeMsisdnAndGetValue "[47] {906} 96 698" |> should equal "4790696698"
    
[<Test>] 
let ``Should remove plus and hyphen`` () =
    makeMsisdnAndGetValue "+4790696698" |> should equal "4790696698"
    makeMsisdnAndGetValue "47-90696698" |> should equal "4790696698"
    
[<Test>]
let ``Don't allow unexpected characters`` () =
    Msisdn.create "4790696698x" |> should equal None
    Msisdn.create "4790696_698" |> should equal None

[<Test>]
let ``All zeros is no go`` () =
    Msisdn.create "00000000000" |> should equal None

[<Test>] 
let ``Should not allow MSISDN from null`` () =
    Msisdn.create null |> should equal None

[<Test>] 
let ``Should be equal even if string representations were not`` () =
    Msisdn.equals 
        (Option.get <| Msisdn.create "4790696698")
        (Option.get <| Msisdn.create "47 90 69 66 98")
    |> should equal true

[<Test>] 
let ``Should accept numbers of 15 digits, but no more`` () =
    makeMsisdnAndGetValue "123456789012345" |> should equal "123456789012345"
    makeMsisdnAndGetValue "123456789012345 " |> should equal "123456789012345"
    Msisdn.create "1234567890123456" |> should equal None

let ccc =
    Msisdn.create >> Option.get >> Msisdn.countryCode

[<Test>]
let ``Country calling codes`` () =
    ccc "4790696698" |> should equal ("47", "Norway")

// compareTo has no test
