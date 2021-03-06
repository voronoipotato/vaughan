namespace VaughanTests
    module GuitarTests =
        open Xunit
        open FsUnit
        open FsUnit.Xunit
        open FsCheck
        open FsCheck.Xunit
        open Vaughan.Domain
        open Vaughan.Chords
        open Vaughan.Guitar
        open Vaughan.ScaleHarmonizer
        open Vaughan.Scales
        open VaughanTests.DiatonicScalesArbitrary

        let cIonian = createScaleNotes Ionian C

        let cMaj = triadsHarmonizer ScaleDegree.I cIonian

        [<Property(Arbitrary = [| typeof<DiatonicScales> |])>]
        let ``Should map diatonic closed triads to guitar fretboard`` (scaleType: ScaleType) (scaleDegree: ScaleDegree) (root: Note) (bassString: GuitarString) () =
            ((bassString <> FirstString || bassString = SecondString))
                ==> lazy (
                            let scale = createScaleNotes scaleType root
                            let chord = triadsHarmonizer scaleDegree scale
                            let guitarChord = createGuitarChord bassString chord
                            let maxFret = guitarChord.Frets |> List.map (fun f -> f.Fret) |> List.max
                            let minFret = guitarChord.Frets |> List.map (fun f -> f.Fret) |> List.min
                            maxFret - minFret < 6)

        [<Fact>]
        let ``Should map c major to guitar fretboard``() =
            (createGuitarChord SixthString cMaj).Frets |> should equal [
                        {GuitarString=SixthString; Fret=8; Note=C};
                        {GuitarString=FifthString; Fret=7; Note=E};
                        {GuitarString=FourthString; Fret=5; Note=G};
                    ]

        [<Fact>]
        let ``Should map c major open to guitar fretboard on fifth string``() =
            (createGuitarChord FifthString (cMaj |> toOpen)).Frets |> should equal [
                        {GuitarString=FifthString; Fret=3; Note=C};
                        {GuitarString=FourthString; Fret=2; Note=E};
                        {GuitarString=ThirdString; Fret=0; Note=G};
                        {GuitarString=SecondString; Fret=1; Note=C};
                        {GuitarString=FirstString; Fret=0; Note=E};
                    ]

        [<Fact>]
        let ``Should map c major to guitar fretboard on fourth string``() =
            (createGuitarChord FourthString cMaj).Frets |> should equal [
                        {GuitarString=FourthString; Fret=10; Note=C};
                        {GuitarString=ThirdString; Fret=9; Note=E};
                        {GuitarString=SecondString; Fret=8; Note=G};
                    ]

        [<Fact>]
        let ``Should map c major to guitar fretboard on third string``() =
            (createGuitarChord ThirdString cMaj).Frets |> should equal [
                        {GuitarString=ThirdString; Fret=5; Note=C};
                        {GuitarString=SecondString; Fret=5; Note=E};
                        {GuitarString=FirstString; Fret=3; Note=G};
                    ]

        [<Fact>]
        let ``Should map F major to guitar fretboard on sixth string``() =
            let fMaj = triadsHarmonizer ScaleDegree.IV cIonian
            (createGuitarChord SixthString fMaj).Frets |> should equal [
                        {GuitarString=SixthString; Fret=13; Note=F};
                        {GuitarString=FifthString; Fret=12; Note=A};
                        {GuitarString=FourthString; Fret=10; Note=C};
                    ]

        [<Property(Arbitrary = [| typeof<DiatonicScales> |])>]
        let ``Should map diatonic closed sevent chords to guitar fretboard`` (scaleType: ScaleType) (scaleDegree: ScaleDegree) (root: Note) (bassString: GuitarString) () =
            ((bassString = SixthString || bassString = FifthString || bassString = FourthString))
                ==> lazy (
                            let scale = createScaleNotes scaleType root
                            let chord = seventhsHarmonizer scaleDegree scale
                            let guitarChord = createGuitarChord bassString chord
                            let maxFret = guitarChord.Frets |> List.map (fun f -> f.Fret) |> List.max
                            let minFret = guitarChord.Frets |> List.map (fun f -> f.Fret) |> List.min
                            maxFret - minFret < 6)

        [<Property(Arbitrary = [| typeof<DiatonicScales> |])>]
        let ``Should map diatonic closed seventh drop 2 chords to guitar fretboard`` (scaleType: ScaleType) (scaleDegree: ScaleDegree) (root: Note) (bassString: GuitarString) () =
            ((bassString = SixthString || bassString = FifthString || bassString = FourthString))
                ==> lazy (
                            let scale = createScaleNotes scaleType root
                            let chord = seventhsHarmonizer scaleDegree scale |> toDrop2
                            let guitarChord = createGuitarChord bassString chord
                            let maxFret = guitarChord.Frets |> List.map (fun f -> f.Fret) |> List.max
                            let minFret = guitarChord.Frets |> List.map (fun f -> f.Fret) |> List.min
                            maxFret - minFret < 6)

        [<Fact>]
        let ``Should map C major 7 drop 2 to guitar fretboard on fifth string closed``() =
            let chord = seventhsHarmonizer ScaleDegree.I cIonian |> toDrop2
            (createGuitarChord FifthString chord).Frets |> should equal [
                        {GuitarString=FifthString; Fret=3; Note=C};
                        {GuitarString=FourthString; Fret=5; Note=G};
                        {GuitarString=ThirdString; Fret=4; Note=B};
                        {GuitarString=SecondString; Fret=5; Note=E};
                    ]

        [<Fact>]
        let ``Should map A major 7 drop 2 to guitar fretboard on fifth string closed``() =
            let scale = createScaleNotes Ionian A
            let chord = seventhsHarmonizer ScaleDegree.I scale |> toDrop2
            (createGuitarChord FifthString chord).Frets |> should equal [
                        {GuitarString=FifthString; Fret=12; Note=A};
                        {GuitarString=FourthString; Fret=14; Note=E};
                        {GuitarString=ThirdString; Fret=13; Note=GSharp};
                        {GuitarString=SecondString; Fret=14; Note=CSharp};
                    ]

        [<Property(Arbitrary = [| typeof<DiatonicScales> |])>]
        let ``Should map diatonic closed sevent drop 3 chords to guitar fretboard`` (scaleType: ScaleType) (scaleDegree: ScaleDegree) (root: Note) (bassString: GuitarString) () =
            (bassString = SixthString || bassString = FifthString)
                ==> lazy (
                            let scale = createScaleNotes scaleType root
                            let chord = seventhsHarmonizer scaleDegree scale |> toDrop3
                            let guitarChord = createGuitarChord bassString chord
                            let maxFret = guitarChord.Frets |> List.map (fun f -> f.Fret) |> List.max
                            let minFret = guitarChord.Frets |> List.map (fun f -> f.Fret) |> List.filter (fun f -> f <> -1) |> List.min
                            maxFret - minFret < 6)

        [<Fact>]
        let ``Should map C major 7 drop 3 to guitar fretboard on sixth string closed``() =
            let chord = seventhsHarmonizer ScaleDegree.I cIonian |> toDrop3
            (createGuitarChord SixthString chord).Frets |> should equal [
                        {GuitarString=SixthString; Fret=8; Note=C};
                        {GuitarString=FifthString; Fret=(-1); Note=A};
                        {GuitarString=FourthString; Fret=9; Note=B};
                        {GuitarString=ThirdString; Fret=9; Note=E};
                        {GuitarString=SecondString; Fret=8; Note=G};
                    ]

        [<Fact>]
        let ``Should map C major9 ignoring 5th to guitar fretboard on fifth string closed``() =
            let chord = chord C Major9
                        |> skipFunction Fifth

            (createGuitarChord FifthString chord).Frets |> should equal [
                        {GuitarString=FifthString; Fret=3; Note=C};
                        {GuitarString=FourthString; Fret=2; Note=E};
                        {GuitarString=ThirdString; Fret=4; Note=B};
                        {GuitarString=SecondString; Fret=3; Note=D};
                    ]

        [<Fact>]
        let ``Should map C9 ignoring 5th to guitar fretboard on fifth string closed``() =
            let chord = chord C Dominant9
                        |> skipFunction Fifth

            (createGuitarChord FifthString chord).Frets |> should equal [
                        {GuitarString=FifthString; Fret=3; Note=C};
                        {GuitarString=FourthString; Fret=2; Note=E};
                        {GuitarString=ThirdString; Fret=3; Note=BFlat};
                        {GuitarString=SecondString; Fret=3; Note=D};
                    ]

    module GuitarTabTests =
        open Xunit
        open FsUnit
        open FsUnit.Xunit
        open FsCheck
        open FsCheck.Xunit
        open System
        open Vaughan.Domain
        open Vaughan.Chords
        open Vaughan.Guitar
        open Vaughan.GuitarTab
        open Vaughan.ScaleHarmonizer
        open Vaughan.Scales
        open VaughanTests.DiatonicScalesArbitrary

        let cIonian = createScaleNotes Ionian C
        let cMaj = triadsHarmonizer ScaleDegree.I cIonian

        let createTriad (scaleType: ScaleType) (scaleDegree: ScaleDegree) (root: Note) (bassString: GuitarString) =
            let scale = createScaleNotes scaleType root
            let chord = triadsHarmonizer scaleDegree scale
            createGuitarChord bassString chord

        let createSeventhChord (scaleType: ScaleType) (scaleDegree: ScaleDegree) (root: Note) (bassString: GuitarString) =
            let scale = createScaleNotes scaleType root
            let chord = seventhsHarmonizer scaleDegree scale
            createGuitarChord bassString chord

        [<Property(Arbitrary = [| typeof<DiatonicScales> |])>]
        let ``Should map diatonic closed triad to guitar tab`` (scaleType: ScaleType) (scaleDegree: ScaleDegree) (root: Note) (bassString: GuitarString) () =
            (bassString <> SecondString && bassString <> FirstString)
                ==> lazy (
                            let guitarChord = createTriad scaleType scaleDegree root bassString
                            let frets = guitarChord.Frets
                            let tab = tabify guitarChord
                            tab.Contains (string frets.[0].Fret)
                            && tab.Contains (string frets.[1].Fret)
                            && tab.Contains (string frets.[2].Fret))

        [<Property(Arbitrary = [| typeof<DiatonicScales> |])>]
        let ``Should map diatonic closed seventh chord to guitar tab`` (scaleType: ScaleType) (scaleDegree: ScaleDegree) (root: Note) (bassString: GuitarString) () =
            (bassString <> ThirdString && bassString <> SecondString && bassString <> FirstString)
                ==> lazy (
                            let guitarChord = createSeventhChord scaleType scaleDegree root bassString
                            let frets = guitarChord.Frets
                            let tab = tabify guitarChord
                            tab.Contains (string frets.[0].Fret)
                            && tab.Contains (string frets.[1].Fret)
                            && tab.Contains (string frets.[2].Fret)
                            && tab.Contains (string frets.[3].Fret))

        [<Fact>]
        let ``Should draw C major 7 drop 2 to guitar fretboard on fifth string closed ``() =
            let guitarChord =
                (cIonian
                |> seventhsHarmonizer ScaleDegree.I
                |> toDrop2
                |> createGuitarChord FifthString)
            guitarChord |> tabify |> should equal ("      CMaj7   " + Environment.NewLine +
                                            "E||-------------||" + Environment.NewLine +
                                            "B||----5--------||" + Environment.NewLine +
                                            "G||----4--------||" + Environment.NewLine +
                                            "D||----5--------||" + Environment.NewLine +
                                            "A||----3--------||" + Environment.NewLine +
                                            "E||-------------||" + Environment.NewLine)

        [<Fact>]
        let ``Should draw A major 7 to guitar fretboard on fifth string closed ``() =
            let guitarChord =
                (createScaleNotes Ionian A
                |> seventhsHarmonizer ScaleDegree.I
                |> toDrop2
                |> createGuitarChord FifthString)
            guitarChord |> tabify |> should equal ("      AMaj7   " + Environment.NewLine +
                                            "E||-------------||" + Environment.NewLine +
                                            "B||----14-------||" + Environment.NewLine +
                                            "G||----13-------||" + Environment.NewLine +
                                            "D||----14-------||" + Environment.NewLine +
                                            "A||----12-------||" + Environment.NewLine +
                                            "E||-------------||" + Environment.NewLine)

        [<Fact>]
        let ``Should draw c major to guitar fretboard on sixth string``() =
            let guitarChord = createGuitarChord SixthString cMaj
            guitarChord |> tabify |> should equal ("      CMaj   " + Environment.NewLine +
                                            "E||------------||" + Environment.NewLine +
                                            "B||------------||" + Environment.NewLine +
                                            "G||------------||" + Environment.NewLine +
                                            "D||----5-------||" + Environment.NewLine +
                                            "A||----7-------||" + Environment.NewLine +
                                            "E||----8-------||" + Environment.NewLine)

        [<Fact>]
        let ``Should draw C major 7 drop 3 to guitar fretboard on sixth string closed``() =
            let guitarChord =
                (cIonian
                |> seventhsHarmonizer ScaleDegree.I
                |> toDrop3
                |> createGuitarChord SixthString)
            guitarChord |> tabify |> should equal ("      CMaj7   " + Environment.NewLine +
                                            "E||-------------||" + Environment.NewLine +
                                            "B||----8--------||" + Environment.NewLine +
                                            "G||----9--------||" + Environment.NewLine +
                                            "D||----9--------||" + Environment.NewLine +
                                            "A||-------------||" + Environment.NewLine +
                                            "E||----8--------||" + Environment.NewLine)

        [<Fact>]
        let ``Should draw C major 7 drop 3 to guitar fretboard on fifth string closed``() =
            let guitarChord =
                (cIonian
                |> seventhsHarmonizer ScaleDegree.I
                |> toDrop3
                |> createGuitarChord FifthString)
            guitarChord |> tabify |> should equal ("      CMaj7   " + Environment.NewLine +
                                            "E||----3--------||" + Environment.NewLine +
                                            "B||----5--------||" + Environment.NewLine +
                                            "G||----4--------||" + Environment.NewLine +
                                            "D||-------------||" + Environment.NewLine +
                                            "A||----3--------||" + Environment.NewLine +
                                            "E||-------------||" + Environment.NewLine)

        [<Fact>]
        let ``Should map C9 ignoring 5th to guitar fretboard on fifth string closed``() =
            let guitarChord = chord C Dominant9
                              |> skipFunction Fifth
                              |> createGuitarChord FifthString

            guitarChord |> tabify |> should equal ("      C9   " + Environment.NewLine +
                                            "E||----------||" + Environment.NewLine +
                                            "B||----3-----||" + Environment.NewLine +
                                            "G||----3-----||" + Environment.NewLine +
                                            "D||----2-----||" + Environment.NewLine +
                                            "A||----3-----||" + Environment.NewLine +
                                            "E||----------||" + Environment.NewLine)

        [<Fact>]
        let ``Should tabify multiple chords``() =
            let cIonian = createScaleNotes Ionian C
            let cMaj7 = seventhsHarmonizer ScaleDegree.I cIonian
            let dMin7 = seventhsHarmonizer ScaleDegree.II cIonian
            let eMin7 = seventhsHarmonizer ScaleDegree.III cIonian
            let fMaj7 = seventhsHarmonizer ScaleDegree.IV cIonian

            let guitarChords =
                [cMaj7; dMin7; eMin7; fMaj7]
                |> List.map (
                    toDrop2 >> (createGuitarChord FifthString))

            tabifyAll guitarChords |> should equal
                                ("      CMaj7   DMin7   EMin7   FMaj7   " + Environment.NewLine +
                                "E||-------------------------------------||" + Environment.NewLine +
                                "B||----5-------6-------8-------10-------||" + Environment.NewLine +
                                "G||----4-------5-------7-------9--------||" + Environment.NewLine +
                                "D||----5-------7-------9-------10-------||" + Environment.NewLine +
                                "A||----3-------5-------7-------8--------||" + Environment.NewLine +
                                "E||-------------------------------------||" + Environment.NewLine)

        [<Fact>]
        let ``Should tabify multiple chained chords using operators``() =
            [(G=>Major)] /./ (C=>Major) /./ (A=>Minor) /./ (D=>Major)
            |> List.map (fun c -> createGuitarChord SixthString (toOpen c))
            |> tabifyAll |> should equal
                                ("      GMaj   CMaj   AMin   DMaj   " + Environment.NewLine +
                                "E||----3------0------0------2-------||" + Environment.NewLine +
                                "B||----0------1------1------3-------||" + Environment.NewLine +
                                "G||----0------0------2------2-------||" + Environment.NewLine +
                                "D||----0------2------2------0-------||" + Environment.NewLine +
                                "A||----2------3------0------0-------||" + Environment.NewLine +
                                "E||----3------0------0------2-------||" + Environment.NewLine)

        [<Fact>]
        let ``Should tabify multiple chained chords using only operators``() =
            [(!*(G=>Major) |~ SixthString);
             (!*(C=>Major) |~ FifthString);
             (!*(A=>Minor) |~ FifthString);
             (!*(D=>Major) |~ FourthString)]
            |> tabifyAll |> should equal
                                ("      GMaj   CMaj   AMin   DMaj   " + Environment.NewLine +
                                "E||----3------0------0------2-------||" + Environment.NewLine +
                                "B||----0------1------1------3-------||" + Environment.NewLine +
                                "G||----0------0------2------2-------||" + Environment.NewLine +
                                "D||----0------2------2------0-------||" + Environment.NewLine +
                                "A||----2------3------0--------------||" + Environment.NewLine +
                                "E||----3----------------------------||" + Environment.NewLine)

        [<Fact>]
        let ``Should draw shape of C major 7 drop 3 on sixth string``() =
            let guitarChord =
                (cIonian
                |> seventhsHarmonizer ScaleDegree.I
                |> toDrop3
                |> createGuitarChord SixthString)
            guitarChord |> shapify |> should equal ("CMaj7" + Environment.NewLine +
                                            "EADGBE" + Environment.NewLine +
                                            "8X998X" + Environment.NewLine)


        [<Fact>]
        let ``Should draw shape of C major 7 drop 2 on fifth string``() =
            let guitarChord =
                (cIonian
                |> seventhsHarmonizer ScaleDegree.I
                |> toDrop2
                |> createGuitarChord FifthString)
            guitarChord |> shapify |> should equal ("CMaj7" + Environment.NewLine +
                                            "EADGBE" + Environment.NewLine +
                                            "X3545X" + Environment.NewLine)