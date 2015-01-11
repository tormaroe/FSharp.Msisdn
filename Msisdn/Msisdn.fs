[<RequireQualifiedAccess>]
module Msisdn

open System.Text.RegularExpressions

type T = Msisdn of string

/// Apply function f to the canonical string representation 
/// of an Msisdn 
let apply f (Msisdn s) = f s

/// Get the canonical string representation of an Msisdn
let value = apply id

/// Equality
let equals left right =
    (value left) = (value right)

/// Comparison
let compareTo left right = 
    (value left).CompareTo (value right)

let private validationRegex = Regex ("^[0-9]{8,15}$", RegexOptions.Compiled)

/// Checks if s is a valid input for an Msisdn
let isValid = validationRegex.IsMatch

let private remove (s:string) x =
    s.Replace (string x, "")

let private removeUnwantedChars s =
    " \t\r\n()[]{}+-".ToCharArray () |> Seq.fold remove s 

let rec private removeLeadingZeros (x:string) =
    if x.StartsWith ("0")
    then x.Substring (1) |> removeLeadingZeros
    else x

/// Transforms a valid Msisdn string to its canonical form
let canonicalize =
    removeUnwantedChars >> removeLeadingZeros

/// Creates an Msisdn option.
/// If s is a valid number, returns Some Msisdn.T.
/// If not valid, returns None
let create s =
    if s = null 
    then None
    else
        let s' = canonicalize s
        if isValid s' then Some (Msisdn s') else None

let private unassigned = "unassigned"

/// Get country code and name from an Msisdn
let countryCode x =
    let s = value x
    match s.[0] with

    (* Zone 1: North American Numbering Plan Area *)
    | '1' -> "1", "North America" // !

    (* Zone 2: mostly Africa *)
    | '2' -> 
        match s.[1] with
        | '0' -> "20", "Egypt"
        | '1' -> 
            match s.[2] with
            | '0' -> "210", unassigned
            | '1' -> "211", "South Sudan"
            | '2' -> 
                match s.Substring(0, 7) with
                | "2125288" -> "212", "Western Sahara"
                | "2125289" -> "212", "Western Sahara"
                | _         -> "212", "Morocco"
            | '3' -> "213", "Algeria"
            | '4' -> "214", unassigned
            | '5' -> "215", unassigned
            | '6' -> "216", "Tunisia"
            | '7' -> "217", unassigned
            | '8' -> "218", "Libya"
            | '9' -> "219", unassigned
        | '2' -> 
            match s.[2] with
            | '0' -> "220", "Gambia"
            | '1' -> "221", "Senegal"
            | '2' -> "222", "Mauritania"
            | '3' -> "223", "Mali"
            | '4' -> "224", "Guinea"
            | '5' -> "225", "Côte d'Ivoire"
            | '6' -> "226", "Burkina Faso"
            | '7' -> "227", "Niger"
            | '8' -> "228", "Togo"
            | '9' -> "229", "Benin"
        | '3' -> 
            match s.[2] with
            | '0' -> "230", "Mauritius"
            | '1' -> "231", "Liberia"
            | '2' -> "232", "Sierra Leone"
            | '3' -> "233", "Ghana"
            | '4' -> "234", "Nigeria"
            | '5' -> "235", "Chad"
            | '6' -> "236", "Central African Republic"
            | '7' -> "237", "Cameroon"
            | '8' -> "238", "Cape Verde"
            | '9' -> "239", "São Tomé and Príncipe"
        | '4' -> 
            match s.[2] with
            | '0' -> "240", "Equatorial Guinea"
            | '1' -> "241", "Gabon"
            | '2' -> "242", "Republic of the Congo"
            | '3' -> "243", "Democratic Republic of the Congo"
            | '4' -> "244", "Angola"
            | '5' -> "245", "Guinea-Bissau"
            | '6' -> "246", "British Indian Ocean Territory"
            | '7' -> "247", "Ascension Island"
            | '8' -> "248", "Seychelles"
            | '9' -> "249", "Sudan"
        | '5' ->
            match s.[2] with
            | '0' -> "250", "Rwanda"
            | '1' -> "251", "Ethiopia"
            | '2' -> "252", "Somalia"
            | '3' -> "253", "Djibouti"
            | '4' -> "254", "Kenya"
            | '5' -> 
                match s.Substring(0, 5) with
                | "25524" -> "255", "Zanzibar"
                | _       -> "255", "Tanzania"
            | '6' -> "256", "Uganda"
            | '7' -> "257", "Burundi"
            | '8' -> "258", "Mozambique"
            | '9' -> "259", unassigned
        | '6' ->
            match s.[2] with
            | '0' -> "260", "Zambia"
            | '1' -> "261", "Madagascar"
            | '2' -> 
                match s.Substring(0, 6) with
                | "262269" -> "262", "Mayotte"
                | "262639" -> "262", "Mayotte"
                | _ -> "262", "Réunion"
            | '3' -> "263", "Zimbabwe"
            | '4' -> "264", "Namibia"
            | '5' -> "265", "Malawi"
            | '6' -> "266", "Lesotho"
            | '7' -> "267", "Botswana"
            | '8' -> "268", "Swaziland"
            | '9' -> "269", "Comoros"
        | '7' -> "27", "South Africa"
        | '8' -> "28", unassigned
        | '9' -> 
            match s.[2] with
            | '0' -> 
                match s.[3] with
                | '8' -> "290", "Tristan da Cunha"
                | _   -> "290", "Saint Helena"
            | '1' -> "291", "Eritrea"
            | '2' -> "292", unassigned
            | '3' -> "293", unassigned
            | '4' -> "294", unassigned
            | '5' -> "295", unassigned
            | '6' -> "296", unassigned
            | '7' -> "297", "Aruba"
            | '8' -> "298", "Faroe Islands"
            | '9' -> "299", "Greenland"
    
    (* Zones 3: Europe *)
    | '3' -> 
        match s.[1] with
        | '0' -> "30", "Greece"
        | '1' -> "31", "Netherlands"
        | '2' -> "32", "Belgium"
        | '3' -> "33", "France"
        | '4' -> "34", "Spain"
        | '5' -> 
            match s.[2] with
            | '0' -> "350", "Gibraltar"
            | '1' -> "351", "Portugal"
            | '2' -> "352", "Luxembourg"
            | '3' -> "353", "Ireland"
            | '4' -> "354", "Iceland"
            | '5' -> "355", "Albania"
            | '6' -> "356", "Malta"
            | '7' -> "357", "Cyprus"
            | '8' -> 
                if s.Substring(0, 5) = "35818"
                then "358", "Åland Islands"
                else "358", "Finland"
            | '9' -> "359", "Bulgaria"
        | '6' -> "36", "Hungary"
        | '7' -> 
            match s.[2] with
            | '0' -> "370", "Lithuania"
            | '1' -> "371", "Latvia"
            | '2' -> "372", "Estonia"
            | '3' ->
                match s.[3] with
                | '2' -> "373", "Transnistria"
                | '5' -> "373", "Transnistria" 
                | _   -> "373", "Moldova"
            | '4' -> 
                match s.Substring(0, 5) with
                | "37447" -> "374", "Nagorno-Karabakh"
                | "37497" -> "374", "Nagorno-Karabakh"
                | _       -> "374", "Armenia"
            | '5' -> "375", "Belarus"
            | '6' -> "376", "Andorra"
            | '7' -> "377", "Monaco"
            | '8' -> "378", "San Marino"
            | '9' -> "379", "Vatican City"
        | '8' -> 
            match s.[2] with
            | '0' -> "380", "Ukraine"
            | '1' -> "381", "Serbia"
            | '2' -> "382", "Montenegro"
            | '3' -> "383", "Kosovo"
            | '4' -> "384", unassigned
            | '5' -> "385", "Croatia"
            | '6' -> "386", "Slovenia"
            | '7' -> "387", "Bosnia and Herzegovina"
            | '8' -> "388", unassigned
            | '9' -> "389", "Macedonia"
        | '9' -> "39", "Italy"
    
    (* Zones 4: Europe *)
    | '4' ->
        match s.[1] with
        | '0' -> "40", "Romania"
        | '1' -> "41", "Switzerland"
        | '2' -> 
            match s.[2] with
            | '0' -> "420", "Czech Republic"
            | '1' -> "421", "Slovakia"
            | '2' -> "422", unassigned
            | '3' -> "423", "Liechtenstein"
            | '4' -> "424", unassigned
            | '5' -> "425", unassigned
            | '6' -> "426", unassigned
            | '7' -> "427", unassigned
            | '8' -> "428", unassigned
            | '9' -> "429", unassigned
        | '3' -> "43", "Austria"
        | '4' ->
            match s.Substring(0, 6) with
            | "441481" -> "44", "Guernsey"
            | "441534" -> "44", "Jersey"
            | "441624" -> "44", "Isle of Man"
            | _        -> "44", "United Kingdom"
        | '5' -> "45", "Denmark"
        | '6' -> "46", "Sweden"
        | '7' -> "47", "Norway" // !
        | '8' -> "48", "Poland"
        | '9' -> "49", "Germany"
    
    (* Zone 5: mostly Latin America *)
    | '5' ->
        match s.[1] with
        | '0' ->
            match s.[2] with
            | '0' -> "500", "Falkland Islands"
            | '1' -> "501", "Belize"
            | '2' -> "502", "Guatemala"
            | '3' -> "503", "El Salvador"
            | '4' -> "504", "Honduras"
            | '5' -> "505", "Nicaragua"
            | '6' -> "506", "Costa Rica"
            | '7' -> "507", "Panama"
            | '8' -> "508", "Saint-Pierre and Miquelon"
            | '9' -> "509", "Haiti"
        | '1' -> "51", "Peru"
        | '2' -> "52", "Mexico"
        | '3' -> "53", "Cuba"
        | '4' -> "54", "Argentina"
        | '5' -> "55", "Brazil"
        | '6' -> "56", "Chile"
        | '7' -> "57", "Colombia"
        | '8' -> "58", "Venezuela"
        | '9' -> 
            match s.[3] with
            | '0' -> "590", "Guadeloupe"
            | '1' -> "591", "Bolivia"
            | '2' -> "592", "Guyana"
            | '3' -> "593", "Ecuador"
            | '4' -> "594", "French Guiana"
            | '5' -> "595", "Paraguay"
            | '6' -> "596", "Martinique"
            | '7' -> "597", "Suriname"
            | '8' -> "598", "Uruguay"
            | '9' ->
                match s.[4] with
                | '0' -> "599", unassigned
                | '1' -> "599", unassigned
                | '2' -> "599", unassigned
                | '3' -> "599", "Sint Eustatius"
                | '4' -> "599", "Saba"
                | '5' -> "599", unassigned
                | '6' -> "599", unassigned
                | '7' -> "599", "Bonaire"
                | '8' -> "599", unassigned
                | '9' -> "599", "Curaçao"
    
    (* Zone 6: Southeast Asia and Oceania *)
    | '6' ->
        match s.[1] with
        | '0' -> "60", "Malaysia"
        | '1' ->
            match s.Substring(0, 7) with
            | "6189162" -> "61", "Cocos Islands"
            | "6189164" -> "61", "Christmas Island"
            | _         -> "61", "Australia"
        | '2' -> "62", "Indonesia"
        | '3' -> "63", "Philippines"
        | '4' -> "64", "New Zealand"
        | '5' -> "65", "Singapore"
        | '6' -> "66", "Thailand"
        | '7' -> 
            match s.[2] with
            | '0' -> "670", "East Timor"
            | '1' -> "671", unassigned
            | '2' -> "672", "Australian External Territories" // !
            | '3' -> "673", "Brunei"
            | '4' -> "674", "Nauru"
            | '5' -> "675", "Papua New Guinea"
            | '6' -> "676", "Tonga"
            | '7' -> "677", "Solomon Islands"
            | '8' -> "678", "Vanuatu"
            | '9' -> "679", "Fiji"
        | '8' ->
            match s.[2] with
            | '0' -> "680", "Palau"
            | '1' -> "681", "Wallis and Futuna"
            | '2' -> "682", "Cook Islands"
            | '3' -> "683", "Niue"
            | '4' -> "684", unassigned
            | '5' -> "685", "Samoa"
            | '6' -> "686", "Kiribati"
            | '7' -> "687", "New Caledonia"
            | '8' -> "688", "Tuvalu"
            | '9' -> "689", "French Polynesia"
        | '9' -> 
            match s.[2] with
            | '0' -> "690", "Tokelau"
            | '1' -> "691", "Federated States of Micronesia"
            | '2' -> "692", "Marshall Islands"
            | '3' -> "693", unassigned
            | '4' -> "694", unassigned
            | '5' -> "695", unassigned
            | '6' -> "696", unassigned
            | '7' -> "697", unassigned
            | '8' -> "698", unassigned
            | '9' -> "699", unassigned
    
    (* Zone 7: Former Soviet Union *)
    | '7' -> 
        match s.[1] with 
        | '6' -> "76", "Kazakhstan"
        | '7' -> "77", "Kazakhstan"
        | _   -> "7", "Russia"
    
    (* Zone 8: East Asia and special services *)
    | '8' ->
        match s.[1] with
        | '0' -> 
            match s.[2] with
            | '0' -> "800", "International Freephone"
            | '1' -> "801", unassigned
            | '2' -> "802", unassigned
            | '3' -> "803", unassigned
            | '4' -> "804", unassigned
            | '5' -> "805", unassigned
            | '6' -> "806", unassigned
            | '7' -> "807", unassigned
            | '8' -> "808", "Shared Cost Services"
            | '9' -> "809", unassigned
        | '1' -> "81", "Japan"
        | '2' -> "82", "South Korea"
        | '3' -> "83", unassigned
        | '4' -> "84", "Vietnam"
        | '5' -> 
            match s.[2] with
            | '0' -> "850", "North Korea"
            | '1' -> "851", unassigned
            | '2' -> "852", "Hong Kong"
            | '3' -> "853", "Macau"
            | '4' -> "854", unassigned
            | '5' -> "855", "Cambodia"
            | '6' -> "856", "Laos"
            | '7' -> "857", unassigned
            | '8' -> "858", unassigned
            | '9' -> "859", unassigned
        | '6' -> "86", "China"
        | '7' -> 
            match s.[2] with
            | '0' -> "870", "Inmarsat \"SNAC\" service"
            | '1' -> "871", unassigned
            | '2' -> "872", unassigned
            | '3' -> "873", unassigned
            | '4' -> "874", unassigned
            | '5' -> "875", "Maritime Mobile service"
            | '6' -> "876", "Maritime Mobile service"
            | '7' -> "877", "Maritime Mobile service"
            | '8' -> "878", "Universal Personal Telecommunications services"
            | '9' -> "879", "reserved for national non-commercial purposes"
        | '8' -> 
            match s.[2] with
            | '0' -> "880", "Bangladesh"
            | '1' -> "881", "Global Mobile Satellite System"
            | '2' -> "882", "International Networks"
            | '3' -> "883", "International Networks"
            | '4' -> "884", unassigned
            | '5' -> "885", unassigned
            | '6' -> "886", "Taiwan"
            | '7' -> "887", unassigned
            | '8' -> "888", "Telecommunications for Disaster Relief by OCHA"
            | '9' -> "889", unassigned
        | '9' -> "89", unassigned
    
    (* Zone 9: mostly Asia *)
    | '9' -> 
        match s.[1] with
        | '0' -> 
            match s.Substring(0,5) with
            | "90392" | "90533" | "90542" -> "90", "Northern Cyprus"
            | _ -> "90", "Turkey"
        | '1' -> "91", "India"
        | '2' -> "92", "Pakistan"
        | '3' -> "93", "Afghanistan"
        | '4' -> "94", "Sri Lanka"
        | '5' -> "95", "Burma"
        | '6' ->
            match s.[2] with
            | '0' -> "960", "Maldives"
            | '1' -> "961", "Lebanon"
            | '2' -> "962", "Jordan"
            | '3' -> "963", "Syria"
            | '4' -> "964", "Iraq"
            | '5' -> "965", "Kuwait"
            | '6' -> "966", "Saudi Arabia"
            | '7' -> "967", "Yemen"
            | '8' -> "968", "Oman"
            | '9' -> "969", unassigned
        | '7' ->
            match s.[2] with
            | '0' -> "970", "Palestine"
            | '1' -> "971", "United Arab Emirates"
            | '2' -> "972", "Israel"
            | '3' -> "973", "Bahrain"
            | '4' -> "974", "Qatar"
            | '5' -> "975", "Bhutan"
            | '6' -> "976", "Mongolia"
            | '7' -> "977", "Nepal"
            | '8' -> "978", unassigned
            | '9' -> "979", "International Premium Rate Service"
        | '8' -> "98", "Iran"
        | '9' ->
            match s.[2] with
            | '0' -> "990", unassigned
            | '1' -> "991", "International Telecommunications Public Correspondence Service trial (ITPCS)"
            | '2' -> "992", "Tajikistan"
            | '3' -> "993", "Turkmenistan"
            | '4' -> "994", "Azerbaijan"
            | '5' -> 
                match s.Substring(0, 5) with
                | "99534" -> "995", "South Ossetia"
                | "99544" -> "995", "Abkhazia"
                | _       -> "995", "Georgia"
            | '6' -> "996", "Kyrgyzstan"
            | '7' -> "997", unassigned
            | '8' -> "998", "Uzbekistan"
            | '9' -> "999", "reserved for future global service"