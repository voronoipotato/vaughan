﻿namespace VaughanTests
    module SonicPiTests =
        open NUnit.Framework
        open FsCheck
        open FsCheck.NUnit
        open Swensen.Unquote
        open Vaughan.Domain
        open Vaughan.Notes
        open Vaughan.Chords
        open Vaughan.SonicPi

        [<Test>]
        let ``Should generate SonicPi DSL for use synth``() =
            UseSynth(Fm) |> toSonicPiScript =! "use_synth :fm"

        [<Test>]
        let ``Should generate SonicPi DSL for synth block withempty block``() =
            WithSynth(Fm, []) |> toSonicPiScript =! sprintf "with_synth :fm do\nend"

        [<Test>]
        let ``Should generate SonicPi DSL for synth block with one instruction block``() =
            WithSynth(Fm, [PlayNote(C, OneLine, [])]) 
            |> toSonicPiScript =! "with_synth :fm do\nplay 48\nend"

        [<Test>]
        let ``Should generate SonicPi DSL for synth block with two instructions block``() =
            WithSynth(Fm, [PlayNote(C, OneLine, []); PlayNote(C, OneLine, [])]) 
            |> toSonicPiScript =! "with_synth :fm do\nplay 48\nplay 48\nend"

        [<Test>]
        let ``Should generate SonicPi DSL for sleep``() =
            Sleep(1<s>) |> toSonicPiScript =! "sleep 1"

        [<Test>]
        let ``Should generate SonicPi DSL for play note``() =
            PlayNote(C, OneLine, [])|> toSonicPiScript =! "play 48" 
        
        [<Test>]
        let ``Should generate SonicPi DSL for play chord``() =
            PlayChord(chord C Major, TwoLine, []) |> toSonicPiScript =! "play [60,64,67]"

        [<Test>]
        let ``Should generate SonicPi DSL for play chord major 7th``() =
            PlayChord(chord C Major7, TwoLine, []) |> toSonicPiScript =! "play [60,64,67,71]"

        [<Test>]
        let ``Should generate SonicPi DSL for fx empty block``() =
            WithFx(Reverb,[]) |> toSonicPiScript =! "with_fx :reverb do\nend"

        [<Test>]
        let ``Should generate SonicPi DSL for fx one instruction block``() =
            WithFx(Reverb,[PlayNote(C, OneLine, [])]) 
            |> toSonicPiScript =! "with_fx :reverb do\nplay 48\nend"

        [<Test>]
        let ``Should generate SonicPi DSL for fx two instructions block``() =
            WithFx(Reverb,[PlayNote(C, OneLine, []); PlayNote(C, OneLine, [])]) 
            |> toSonicPiScript =! "with_fx :reverb do\nplay 48\nplay 48\nend"

        [<Test>]
        let ``Should generate SonicPi DSL with one instruction block``() =
            Statments([PlayNote(C, OneLine, []);]) 
            |> toSonicPiScript =! "play 48\n"

        [<Test>]
        let ``Should generate SonicPi DSL with two instructions block``() =
            Statments([UseSynth(Fm); PlayNote(C, OneLine, []);]) 
            |> toSonicPiScript =! "use_synth :fm\nplay 48\n"

        [<Test>]
        let ``Should generate SonicPi DSL for play note with amplitude 2``() =
            PlayNote(C, OneLine, [Amplitude(2.0<loud>)])|> toSonicPiScript =! "play 48,amp:2.00"

        [<Test>]
        let ``Should generate SonicPi DSL for play note with panning rigth``() =
            PlayNote(C, OneLine, [Panning(1.0<pan>)])|> toSonicPiScript =! "play 48,pan:1.00"

        [<Test>]
        let ``Should generate SonicPi DSL for play note with release 2``() =
            PlayNote(C, OneLine, [Release(2.0<beat>)])|> toSonicPiScript =! "play 48,release:2.00"

        [<Test>]
        let ``Should generate SonicPi DSL for play note with attack 2``() =
            PlayNote(C, OneLine, [Attack(2.0<beat>)])|> toSonicPiScript =! "play 48,attack:2.00"

        [<Test>]
        let ``Should generate SonicPi DSL for play note with attack level 2``() =
            PlayNote(C, OneLine, [AttackLevel(2.0<beat>)])|> toSonicPiScript =! "play 48,attack_level:2.00"

        [<Test>]
        let ``Should generate SonicPi DSL for play note with sustain 2``() =
            PlayNote(C, OneLine, [Sustain(2.0<beat>)])|> toSonicPiScript =! "play 48,sustain:2.00"

        [<Test>]
        let ``Should generate SonicPi DSL for play note with sustain level 2``() =
            PlayNote(C, OneLine, [SustainLevel(2.0<beat>)])|> toSonicPiScript =! "play 48,sustain_level:2.00"

        [<Test>]
        let ``Should generate SonicPi DSL for play note with decay 2``() =
            PlayNote(C, OneLine, [Decay(2.0<beat>)])|> toSonicPiScript =! "play 48,decay:2.00"

        [<Test>]
        let ``Should generate SonicPi DSL for play note with decay level 2``() =
            PlayNote(C, OneLine, [DecayLevel(2.0<beat>)])|> toSonicPiScript =! "play 48,decay_level:2.00"

        [<Test>]
        let ``Should generate SonicPi DSL for play note with amplitude and panning``() =
            PlayNote(C, OneLine, [Amplitude(2.0<loud>); Panning(1.0<pan>)])|> toSonicPiScript =! "play 48,amp:2.00,pan:1.00"