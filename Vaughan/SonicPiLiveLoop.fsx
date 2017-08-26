﻿#r "../packages/Bespoke-OSC-Library/lib/Bespoke.Common.Osc.dll"

#load "Infrastructure.fs"
#load "Domain.fs"
#load "Notes.fs"
#load "Chords.fs"
#load "SonicPi.fs"

open Vaughan.Domain
open Vaughan.SonicPi

Statments
    [
        UseBpm 120<bpm>;
        LiveLoop("Foo", 
                    [
                        PlaySample(LoopingSample Garzul, []);
                        UseSynth TheProphet;
                        PlayNote(G, Great, [Release(8.0<beat>)]);
                        Rest 8<beat>
                    ])
    ]
|> toSonicPiScript
|> sonicPiRun

sonicPiStop