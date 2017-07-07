namespace Vaughan

    module ChordVoiceLeading =
        open Chords
        open Domain
        open Notes
        open Infrastructure

        let private isLeadFunctionOnChordDesiredFunction chord desiredNoteFunction desiredPosition =
            snd (chord.Notes |> desiredPosition) = desiredNoteFunction

        let rec private repeatInversion chord times =
            match times with
            | 0 -> chord
            | _ -> repeatInversion (chord |> invert) (times - 1)

        let private allInversions chord =
            let notesInChord = chord.Notes |> List.length
            [for index in 1 .. notesInChord do yield repeatInversion chord index]

        let private inversionForFunction chord desiredNoteFunction desiredPosition =
            allInversions chord
            |> List.filter (fun c -> isLeadFunctionOnChordDesiredFunction c desiredNoteFunction desiredPosition)
            |> List.head

        let private invertionWithNoteClosestToNote chord note desiredPosition =
            (allInversions chord
            |> min (fun c1 c2 ->
                if (measureAbsoluteSemitones (desiredPosition c1) note) < (measureAbsoluteSemitones (desiredPosition c2) note)
                then c1 else c2)).Value

        let inversionForFunctionAsLead chord desiredNoteFunction =
            inversionForFunction chord desiredNoteFunction List.last

        let inversionForFunctionAsBass chord desiredNoteFunction =
            inversionForFunction chord desiredNoteFunction List.head

        let invertionWithLeadClosestToNote chord note =
            invertionWithNoteClosestToNote chord note lead

        let invertionWithBassClosestToNote chord note =
            invertionWithNoteClosestToNote chord note bass