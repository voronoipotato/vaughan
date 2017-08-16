namespace Vaughan

    module Infrastructure =
        let rotateByOne = function
            | [] -> []
            | f::t -> t @ [f]

        let swapFirstTwo = function
            | [] -> []
            | f::s::r -> s::f::r
            | f -> f

        let swapSecondTwo = function
            | [] -> []
            | f::s::t::r -> f::t::s::r
            | f -> f

        let circularSequenceFromList (lst:'a list) =
            let rec next () =
                seq {
                    for element in lst do
                        yield element
                    yield! next()
                }
            next()

        let private sequenceToIndexValueTupleSequence sequence =
            sequence |> Seq.mapi (fun i v -> i, v)

        let filterOddIndexElements sequence =
            sequence
            |> sequenceToIndexValueTupleSequence
            |> Seq.filter (fun (i, _) -> i % 2 = 0)
            |> Seq.map snd

        let rec min (minOf:'a->'a->'a) (list:'a list) =
            match list with
            | [] -> None
            | [x] -> Some(x)
            | c1::c2::rest -> min minOf ((minOf c1 c2)::rest)

        let cappedMinimum number cap =
            if number < cap then cap else number

        let minimumPositive number =
            cappedMinimum number 0

        let cappedMaximum number cap =
            if number > cap then cap else number

        let combineAll listOfLists =
            let prefix fs pfx = pfx :: fs
            let prefixWith pfxs fs = List.map (prefix fs) pfxs
            let prefixAll pfxs fs = List.collect (prefixWith pfxs) fs
            List.foldBack prefixAll listOfLists [[]]