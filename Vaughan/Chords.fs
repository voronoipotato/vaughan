namespace Vaughan

    module Chords =
        open Domain
        open Notes
        open Infrastructure

        type private ChordAttributes = {Name:string; Quality:ChordQuality; Formula:Interval list}

        let private chordFormula =
            [
                {Name="Maj"; Quality=Major; Formula=[MajorThird; PerfectFifth]}
                {Name="Aug"; Quality=Augmented; Formula=[MajorThird; AugmentedFifth]}
                {Name="Min"; Quality=Minor; Formula=[MinorThird; PerfectFifth]}
                {Name="Dim"; Quality=Diminished; Formula=[MinorThird; DiminishedFifth]}
                {Name="Maj7"; Quality=Major7; Formula=[MajorThird; PerfectFifth; MajorSeventh]}
                {Name="Maj9"; Quality=Major9; Formula=[MajorThird; PerfectFifth; MajorSeventh; MajorNinth]}
                {Name="Maj9(#11)"; Quality=Major9Sharp11; Formula=[MajorThird; PerfectFifth; MajorSeventh; MajorNinth; AugmentedEleventh]}
                {Name="Maj11"; Quality=Major11; Formula=[MajorThird; PerfectFifth; MajorSeventh; PerfectEleventh]}
                {Name="Maj13"; Quality=Major13; Formula=[MajorThird; PerfectFifth; MajorSeventh; MajorThirteenth]}
                {Name="Maj13(#11)"; Quality=Major13Sharp11; Formula=[MajorThird; PerfectFifth; MajorSeventh; MajorThirteenth; AugmentedEleventh]}
                {Name="6"; Quality=Major6; Formula=[MajorThird; PerfectFifth; MajorSixth]}
                {Name="6add9"; Quality=Major6Add9; Formula=[MajorThird; PerfectFifth; MajorSixth; MajorNinth]}
                {Name="6(b5)add9"; Quality=Major6Flat5Add9; Formula=[MajorThird; DiminishedFifth; MajorSixth; MajorNinth]}
                {Name="Aug7"; Quality=Augmented7; Formula=[MajorThird; AugmentedFifth; MajorSeventh]}
                {Name="Min7"; Quality=Minor7; Formula=[MinorThird; PerfectFifth; MinorSeventh]}
                {Name="Min9"; Quality=Minor9; Formula=[MinorThird; PerfectFifth; MinorSeventh; MajorNinth]}
                {Name="Min6"; Quality=Minor6; Formula=[MinorThird; PerfectFifth; MajorSixth]}
                {Name="Min6Add9"; Quality=Minor6Add9; Formula=[MinorThird; PerfectFifth; MajorSixth; MajorNinth]}
                {Name="Min7b5"; Quality=Minor7b5; Formula=[MinorThird; DiminishedFifth; MinorSeventh]}
                {Name="MinMaj7"; Quality=MinorMaj7; Formula=[MinorThird; PerfectFifth; MajorSeventh]}
                {Name="MinMaj9"; Quality=MinorMaj9; Formula=[MinorThird; PerfectFifth; MajorSeventh; MajorNinth]}
                {Name="Min7(b9)"; Quality=MinorMaj9; Formula=[MinorThird; PerfectFifth; MinorSeventh; MinorNinth]}
                {Name="Min7(b5b9)"; Quality=MinorMaj9; Formula=[MinorThird; DiminishedFifth; MinorSeventh; MinorNinth]}
                {Name="Dim7"; Quality=Diminished7; Formula=[MinorThird; DiminishedFifth; DiminishedSeventh]}
                {Name="Dim7"; Quality=Diminished7; Formula=[MinorThird; DiminishedFifth; MajorSixth]}
                {Name="7"; Quality=Dominant7; Formula=[MajorThird; PerfectFifth; MinorSeventh]}
                {Name="7(b5)"; Quality=Dominant7Flat5; Formula=[MajorThird; DiminishedFifth; MinorSeventh]}
                {Name="7(b9)"; Quality=Dominant7Flat9; Formula=[MajorThird; PerfectFifth; MinorSeventh; MinorNinth]}
                {Name="7(#9)"; Quality=Dominant7Sharp9; Formula=[MajorThird; PerfectFifth; MinorSeventh; AugmentedNinth]}
                {Name="7(b5b9)"; Quality=Dominant7Flat5Flat9; Formula=[MajorThird; DiminishedFifth; MinorSeventh; MinorNinth]}
                {Name="7(b5#9)"; Quality=Dominant7Flat5Sharp9; Formula=[MajorThird; DiminishedFifth; MinorSeventh; AugmentedNinth]}
                {Name="9"; Quality=Dominant9; Formula=[MajorThird; PerfectFifth; MinorSeventh; MajorNinth]}
                {Name="11"; Quality=Dominant11; Formula=[MajorThird; PerfectFifth; MinorSeventh; MajorNinth; PerfectEleventh]}
                {Name="13"; Quality=Dominant13; Formula=[MajorThird; PerfectFifth; MinorSeventh; MajorNinth; PerfectEleventh; MajorThirteenth]}
                {Name="Sus2"; Quality=Sus2; Formula=[MajorSecond; PerfectFifth]}
                {Name="Sus2Dim"; Quality=Sus2Diminished; Formula=[MajorSecond; DiminishedFifth]}
                {Name="Sus2Aug"; Quality=Sus2Augmented; Formula=[MajorSecond; AugmentedFifth]}
                {Name="Sus4"; Quality=Sus4; Formula=[PerfectFourth; PerfectFifth]}
                {Name="Sus4Dim"; Quality=Sus4Diminished; Formula=[PerfectFourth; DiminishedFifth]}
                {Name="Sus4Aug"; Quality=Sus4Augmented; Formula=[PerfectFourth; AugmentedFifth]}
            ]

        let private qualityForIntervals intervals =
            (chordFormula
            |> List.filter (fun c -> c.Formula = intervals)
            |> List.head).Quality

        let private intervalsForQuality quality =
            (chordFormula
            |> List.filter (fun c -> c.Quality = quality)
            |> List.head).Formula

        let private nameForQuality quality =
            (chordFormula
            |> List.filter (fun c -> c.Quality = quality)
            |> List.head).Name

        let private functionForInterval = function
            | Unisson -> Root
            | MajorThird | MinorThird | MajorSecond | MinorSecond | PerfectFourth | AugmentedFourth -> Third
            | PerfectFifth | DiminishedFifth | AugmentedFifth  -> Fifth
            | MajorSixth -> Sixth
            | MajorSeventh | MinorSeventh | DiminishedSeventh -> Seventh
            | MajorNinth | MinorNinth | AugmentedNinth -> Ninth
            | PerfectEleventh | AugmentedEleventh -> Eleventh
            | MajorThirteenth -> Thirteenth
            | _ -> Root

        let private note chordNote =
            fst chordNote

        let private noteFunction chordNote =
            snd chordNote

        let private noteForFunction chord chordNoteFunction =
            note (chord.Notes |> List.find (fun n -> noteFunction n = chordNoteFunction))

        let private adjustIntervalForFunctionsAboveSeventh interval noteFunction =
            match noteFunction with
            | Ninth | Eleventh | Thirteenth -> fromDistance ((toDistance interval) + (toDistance PerfectOctave))
            | _ -> interval

        let private intervalsForChord chord =
            let root = noteForFunction chord Root
            chord.Notes
            |> List.map (fun n -> adjustIntervalForFunctionsAboveSeventh (intervalBetween root (note n)) (noteFunction n))
            |> List.skip 1

        let private invertOpenOrClosed chord =
            {chord with Notes= rotateByOne chord.Notes;}

        let private invertDrop2 chord =
            {
                chord with Notes = [chord.Notes |> List.last]
                                   @ 
                                   (chord.Notes
                                    |> List.take (chord.Notes.Length - 1)
                                    |> rotateByOne
                                    |> rotateByOne)
            }

        let private invertDrop3 chord =
            {chord with Notes= chord.Notes |> rotateByOne |> rotateByOne |> swapSecondTwo;}

        let name chord =
            noteName (noteForFunction chord Root)
            + nameForQuality (qualityForIntervals(intervalsForChord chord))

        let invert chord =
            match chord.ChordType with
            | Closed | Open | Triad -> invertOpenOrClosed chord
            | Drop2 -> invertDrop2 chord
            | Drop3 -> invertDrop3 chord

        let bass chord =
            note (chord.Notes |> List.head)

        let lead chord =
            note (chord.Notes |> List.last)

        let noteNames chord =
            chord.Notes |> List.map (note >> noteName)

        let chord root quality =
            {
                Notes= [(root, Root)] @ (intervalsForQuality quality |> List.map (fun i -> ((transpose root i), functionForInterval i)));
                ChordType = Closed
                Name =  noteName root + nameForQuality (qualityForIntervals(intervalsForQuality quality))
            }

        let (=>) root quality =
            chord root quality

        let add chords chord =
            chord :: chords |> rotateByOne

        let (/./) chords chord =
            add chords chord

        let toDrop2 chord =
            if chord.Notes.Length = 4
            then {chord with Notes = chord.Notes |> swapFirstTwo |> rotateByOne; ChordType=Drop2}
            else chord

        let toDrop3 chord =
            if chord.Notes.Length = 4
            then {chord with Notes= (chord |> toDrop2 |> toDrop2).Notes; ChordType=Drop3}
            else chord

        let toTriad chord =
            if chord.Notes.Length = 3 then {chord with ChordType=Triad}
            else chord

        let toOpen chord =
            {chord with ChordType=Open}

        let ( !* ) c =
            toOpen c

        let toClosed chord =
            {chord with ChordType=Closed}

        let skipFunction functionToSkipp chord =
            {chord with Notes = chord.Notes |> List.filter (fun nf -> snd nf <> functionToSkipp)}